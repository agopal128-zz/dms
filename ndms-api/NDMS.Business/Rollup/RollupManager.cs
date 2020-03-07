using NDMS.Business.Common;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Rollup
{
    /// <summary>
    /// Manages actual roll up operation
    /// </summary>
    public class RollupManager
    {
        #region Field(s)
        /// <summary>
        /// Reference to ActualsModifier
        /// </summary>
        private ActualsModifier actualsModifier;

        /// <summary>
        /// Target repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Reference to sum of children rollup strategy
        /// </summary>
        private IRollupStrategy sumOfChildrenRollup;

        /// <summary>
        /// Reference to simple average of children rollup strategy
        /// </summary>
        private IRollupStrategy avgOfChildrenRollup;

        /// <summary>
        /// Reference to same as child rollup strategy
        /// </summary>
        private IRollupStrategy sameAsChildRollup;

        /// <summary>
        /// Recordables Calculator
        /// </summary>
        private ScorecardRecordablesCalculator recordablesCalculator;

        /// <summary>
        /// The target modifier
        /// </summary>
        private TargetModifier targetModifier;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Rollup an actual entry recursively
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        private void RollupRecursively(RollupInfo rollupInfo)
        {
            while (rollupInfo.ParentTargetId != null)
            {
                decimal? rolledupValue = CalculateRollupValue(rollupInfo);
                var currentRollupTarget = targetRepository.Get(rollupInfo.ParentTargetId.Value);
                
                // If parent actuals are calculated, just roll them up
                int? parentDailyActualId = null;
                int? parentMonthlyActualId = null;

                // Depending upon the tracking method, retrieve the actual entries 
                // of the parent for which we need to roll up
                if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    parentDailyActualId = dailyActualRepository.GetAll().Where(x =>
                       x.TargetId == rollupInfo.ParentTargetId &&
                       x.Date == rollupInfo.ActualEntry.Date).Select
                       (x => new { Id = x.Id }).FirstOrDefault()?.Id;

                    AddOrUpdateDailyRollupEntry(rolledupValue, parentDailyActualId,
                        rollupInfo);
                }
                else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    parentMonthlyActualId = monthlyActualRepository.GetAll().Where(x =>
                       x.TargetId == rollupInfo.ParentTargetId &&
                       x.Month == rollupInfo.ActualEntry.Date.Month).Select
                       (x => new { Id = x.Id }).FirstOrDefault()?.Id;

                    AddOrUpdateMonthlyRollupEntry(rolledupValue, parentMonthlyActualId,
                        rollupInfo);
                }

                if (rollupInfo.UpdateRecordable)
                {
                    recordablesCalculator.TrackRecordables(rollupInfo.ParentTargetId.Value, rollupInfo.ActualEntry, rollupInfo.Username);
                }

                // Change the parent target and scorecard id's to the next parent 
                // and call recursively                
                rollupInfo.ParentTargetId = currentRollupTarget.ParentTarget?.Id;
                rollupInfo.ParentScorecardId = currentRollupTarget.ParentTarget?.ScorecardId;
                rollupInfo.RollupMethodId = currentRollupTarget.ParentTarget?.RollUpMethodId;
                RollupRecursively(rollupInfo);
            }
        }

        /// <summary>
        /// Rollup recursively for succeeding days on previous day update.
        /// </summary>
        /// <param name="rollupInfo">The rollup information.</param>
        /// <param name="actualEntryStartDate">The actual entry start date.</param>
        /// <param name="actualEntryEndDate">The actual entry end date.</param>
        private void RollupRecursivelyForSucceedingDays(RollupInfo rollupInfo, DateTime actualEntryStartDate, DateTime actualEntryEndDate)
        {
            while (rollupInfo.ParentTargetId != null)
            {
                decimal? rolledupValue;
                DateTime startDate = actualEntryStartDate;
                // If parent actuals are calculated, just roll them up
                int? parentDailyActualId = null;
                int? parentMonthlyActualId = null;
                var currentRollupTarget = targetRepository.Get(rollupInfo.ParentTargetId.Value);

                // Depending upon the tracking method, retrieve the actual entries 
                // of the parent for which we need to roll up
                if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    while (startDate <= actualEntryEndDate)
                    {
                        rollupInfo.ActualEntry.Date = startDate;
                        rolledupValue = CalculateRollupValue(rollupInfo);

                        if (rolledupValue.HasValue)
                        {
                            parentDailyActualId = dailyActualRepository.GetAll().Where(x =>
                               x.TargetId == rollupInfo.ParentTargetId &&
                               x.Date == startDate).Select
                               (x => new { Id = x.Id }).FirstOrDefault()?.Id;

                            if (parentDailyActualId.HasValue)
                            {
                                AddOrUpdateDailyRollupEntry(rolledupValue, parentDailyActualId,
                                    rollupInfo);
                            }
                        }
                        startDate = startDate.AddDays(1);
                    }
                }
                else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    while (startDate.Month <= actualEntryEndDate.Month)
                    {
                        rollupInfo.ActualEntry.Date = startDate;
                        rolledupValue = CalculateRollupValue(rollupInfo);

                        parentMonthlyActualId = monthlyActualRepository.GetAll().Where(x =>
                           x.TargetId == rollupInfo.ParentTargetId &&
                           x.Month == startDate.Month).Select
                           (x => new { Id = x.Id }).FirstOrDefault()?.Id;

                        if (parentMonthlyActualId.HasValue)
                        {
                            AddOrUpdateMonthlyRollupEntry(rolledupValue, parentMonthlyActualId,
                                rollupInfo);
                            startDate = startDate.AddMonths(1);
                        }
                    }
                }

                // Change the parent target and scorecard id's to the next parent 
                // and call recursively                
                rollupInfo.ParentTargetId = currentRollupTarget.ParentTarget?.Id;
                rollupInfo.ParentScorecardId = currentRollupTarget.ParentTarget?.ScorecardId;
                rollupInfo.RollupMethodId = currentRollupTarget.ParentTarget?.RollUpMethodId;
                RollupRecursivelyForSucceedingDays(rollupInfo, actualEntryStartDate, actualEntryEndDate);
            }
        }

        /// <summary>
        /// Rollup target recursively
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        private void RollupTargetRecursively(RollupInfo rollupInfo, DateTime selectedDate, bool updateStatus)
        {
            while (rollupInfo.ParentTargetId != null)
            {
                // If parent targets are calculated, just roll them up
                int? parentDailyTargetId = null;
                int? parentMonthlyTargetId = null;

                DateTime rollupStartDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                DateTime rollupEndDate = new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month));

                decimal? rolledupValue;

                var rollupTargets = new List<RollupTargetItem>();

                var currentRollupTarget = targetRepository.Get(rollupInfo.ParentTargetId.Value);

                if(currentRollupTarget.EffectiveStartDate > rollupStartDate)
                {
                    rollupStartDate = currentRollupTarget.EffectiveStartDate;
                }

                if(currentRollupTarget.EffectiveEndDate < rollupEndDate)
                {
                    rollupEndDate = currentRollupTarget.EffectiveEndDate;
                }

                var parentMonthlyTarget = currentRollupTarget?.MonthlyTargets.FirstOrDefault(x => x.Month == selectedDate.Month);

                // Depending upon the tracking method, retrieve the targets
                // of the parent for which we need to roll up
                if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    IList<DailyTargetItem> dailyTargets = new List<DailyTargetItem>();
                    if (parentMonthlyTarget != null)
                    {
                        dailyTargets = DistributeDailyTarget(currentRollupTarget, parentMonthlyTarget, selectedDate.Year);
                    }

                    while (rollupStartDate <= rollupEndDate)
                    {
                        rollupInfo.TargetEntryDate = rollupStartDate;
                        rolledupValue = CalculateRollupTargetValue(rollupInfo);                        

                        parentDailyTargetId = parentMonthlyTarget?.DailyTargets.FirstOrDefault(x => x.Day == rollupStartDate.Day)?.Id;
                        var dailyTarget = dailyTargets?.FirstOrDefault(x => x.Day == rollupStartDate.Day && !x.IsOutofRange);
                        var rollupTargetItem = new RollupTargetItem
                        {
                            TargetId = parentDailyTargetId,
                            TargetEntryDate = rollupStartDate,
                            RollUpValue = rolledupValue,
                            GoalValue = dailyTarget?.GoalValue,
                            IsHoliday = dailyTarget?.IsHoliday ?? false
                        };
                        rollupTargets.Add(rollupTargetItem);
                        rollupStartDate = rollupStartDate.AddDays(1);
                    }
                }
                else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    rollupInfo.TargetEntryDate = rollupStartDate;
                    rolledupValue = CalculateRollupTargetValue(rollupInfo);
                    parentMonthlyTargetId = parentMonthlyTarget?.Id;

                    var rollupTargetItem = new RollupTargetItem
                    {
                        TargetId = parentMonthlyTargetId,
                        TargetEntryDate = rollupStartDate,
                        RollUpValue = rolledupValue
                    };
                    rollupTargets.Add(rollupTargetItem);
                }

                AddOrUpdateTargetRollup(rollupInfo, rollupTargets);

                if (currentRollupTarget.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets || updateStatus)
                {
                    UpdateActualStatus(rollupInfo, rollupStartDate, rollupTargets, ref updateStatus);
                }

                rollupInfo.ParentTargetId = currentRollupTarget.ParentTarget?.Id;
                rollupInfo.ParentScorecardId = currentRollupTarget.ParentTarget?.ScorecardId;
                rollupInfo.RollupMethodId = currentRollupTarget.ParentTarget?.RollUpMethodId;
                RollupTargetRecursively(rollupInfo, selectedDate, updateStatus);
            }
        }       

        /// <summary>
        /// Rollup target recursively
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="updateStatus">if set to <c>true</c> [update status].</param>
        private void RollupTargetRecursively(RollupInfo rollupInfo, bool updateStatus)
        {
            while (rollupInfo.ParentTargetId != null)
            {
                // If parent targets are calculated, just roll them up
                int? parentDailyTargetId = null;
                int? parentMonthlyTargetId = null;
                int startMonth;

                decimal? rolledupValue;
                var rollupTargets = new List<RollupTargetItem>();

                var currentRollupTarget = targetRepository.Get(rollupInfo.ParentTargetId.Value);

                DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                DateTime previousMonthDate = currentDate.AddMonths(-1);
                DateTime targetStartDate = currentRollupTarget.EffectiveStartDate;
                DateTime targetEndDate = currentRollupTarget.EffectiveEndDate;
                startMonth = previousMonthDate.Month;
                if (previousMonthDate < targetStartDate)
                {
                    startMonth = targetStartDate.Month;
                }

                int endMonth = currentRollupTarget.EffectiveEndDate.Month;
                int targetYear = targetStartDate.Year;

                while (startMonth <= endMonth)
                {
                    var parentMonthlyTarget = currentRollupTarget?.MonthlyTargets.FirstOrDefault(x => x.Month == startMonth);
                    DateTime monthStartDate = new DateTime(targetYear, startMonth, 1);
                    if (monthStartDate < targetStartDate)
                    {
                        monthStartDate = targetStartDate;
                    }

                    // Depending upon the tracking method, retrieve the targets
                    // of the parent for which we need to roll up
                    if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                    {
                        DateTime monthEndDate = new DateTime(targetYear, startMonth, DateTime.DaysInMonth(targetYear, startMonth));
                        IList<DailyTargetItem> dailyTargets = new List<DailyTargetItem>();

                        if (monthEndDate > targetEndDate)
                        {
                            monthEndDate = targetEndDate;
                        }

                        if (parentMonthlyTarget != null)
                        {
                            dailyTargets = DistributeDailyTarget(currentRollupTarget, parentMonthlyTarget, targetYear);
                        }

                        while (monthStartDate <= monthEndDate)
                        {
                            rollupInfo.TargetEntryDate = monthStartDate;
                            rolledupValue = CalculateRollupTargetValue(rollupInfo);
                            parentDailyTargetId = parentMonthlyTarget?.DailyTargets.FirstOrDefault(x => x.Day == monthStartDate.Day)?.Id;
                            
                            var dailyTarget = dailyTargets?.FirstOrDefault(x => x.Day == monthStartDate.Day && !x.IsOutofRange);

                            var rollupTargetItem = new RollupTargetItem
                            {
                                TargetId = parentDailyTargetId,
                                TargetEntryDate = monthStartDate,
                                RollUpValue = rolledupValue,
                                GoalValue = dailyTarget?.GoalValue,
                                IsHoliday = dailyTarget?.IsHoliday ?? false
                            };
                            rollupTargets.Add(rollupTargetItem);
                            monthStartDate = monthStartDate.AddDays(1);
                        }
                    }
                    else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                    {
                        rollupInfo.TargetEntryDate = monthStartDate;
                        rolledupValue = CalculateRollupTargetValue(rollupInfo);
                        parentMonthlyTargetId = parentMonthlyTarget?.Id;
                        
                        var rollupTargetItem = new RollupTargetItem
                        {
                            TargetId = parentMonthlyTargetId,
                            TargetEntryDate = monthStartDate,
                            RollUpValue = rolledupValue
                        };
                        rollupTargets.Add(rollupTargetItem);
                    }
                    startMonth++;
                }

                AddOrUpdateTargetRollup(rollupInfo, rollupTargets);

                if (currentRollupTarget.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets || updateStatus)
                {
                    // update previous month status
                    UpdateActualStatus(rollupInfo, previousMonthDate, rollupTargets, ref updateStatus);
                    // update current month status
                    UpdateActualStatus(rollupInfo, currentDate, rollupTargets, ref updateStatus);
                }

                rollupInfo.ParentTargetId = currentRollupTarget.ParentTarget?.Id;
                rollupInfo.ParentScorecardId = currentRollupTarget.ParentTarget?.ScorecardId;
                rollupInfo.RollupMethodId = currentRollupTarget.ParentTarget?.RollUpMethodId;
                RollupTargetRecursively(rollupInfo, updateStatus);
            }
        }

        private IList<DailyTargetItem> DistributeDailyTarget(Target currentRollupTarget, MonthlyTarget monthlyTarget, int year)
        {
            bool isDailyTargetExists = (monthlyTarget?.DailyTargets.Count(x => x.MaxGoalValue.HasValue) ?? 0) > 0;
            bool isTargetOnHoliday = targetModifier.CheckIfTargetExistsForHolidays(monthlyTarget, currentRollupTarget.ScorecardId, year);

            if (!isDailyTargetExists || isTargetOnHoliday)
            {
                List<DailyTargetItem> dailyTargetItemList = targetModifier.DistributeExistingDailyTarget(
                                                new GenerateDailyTargetsRequest
                                                {
                                                    ScorecardId = currentRollupTarget.ScorecardId,
                                                    YearId = currentRollupTarget.CalendarYearId,
                                                    MonthId = monthlyTarget.Month,
                                                    MetricId = currentRollupTarget.MetricId,
                                                    EffectiveStartDate = currentRollupTarget.EffectiveStartDate,
                                                    TargetEntryMethodId = currentRollupTarget.TargetEntryMethodId.Value,
                                                    EffectiveEndDate = currentRollupTarget.EffectiveEndDate,
                                                    MonthlyGoalValue = monthlyTarget.MaxGoalValue,
                                                    DailyRateValue = monthlyTarget.DailyRate,
                                                }, year).ToList();
                return dailyTargetItemList;
            }

            return null;
        }

        /// <summary>
        /// Calculates the roll up value based on the rollup method set
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <returns>Calculated rollup value</returns>
        private decimal? CalculateRollupValue(RollupInfo rollupInfo)
        {
            decimal? rolledupValue = null;
            // Retrieve all child targets
            var childTargetIds = targetRepository.GetAll().Where(x =>
                x.ParentTargetId == rollupInfo.ParentTargetId
                && x.IsActive).Select(y => y.Id).ToList();

            switch (rollupInfo.RollupMethodId)
            {
                // "Sum of Children"
                case Constants.RollupMethodSumOfChildren:
                    {
                        IRollupStrategy sumOfChildrenRollup = CreateSumOfChildrenRollupStrategy();
                        rolledupValue = sumOfChildrenRollup.CalculateRollup(rollupInfo,
                            childTargetIds);
                        break;
                    }

                // "Average of Children"
                case Constants.RollupMethodAverageOfChildren:
                    {
                        IRollupStrategy avgOfChildrentRollup =
                            CreateAverageOfChildrenRollupStrategy();
                        rolledupValue = avgOfChildrentRollup.CalculateRollup(rollupInfo,
                            childTargetIds);
                        break;
                    }

                // "Same as Child"
                case Constants.RollupMethodSameAsChild:
                    {
                        IRollupStrategy sameAsChildRollup = CreateSameAsChildRollupStrategy();
                        rolledupValue = sameAsChildRollup.CalculateRollup(rollupInfo,
                            childTargetIds);
                        break;
                    }
            }

            // Round the rolled up value to two decimal places
            if (rolledupValue.HasValue)
            {
                var rollupValue = decimal.Round(rolledupValue.Value, 2, MidpointRounding.AwayFromZero);
                if (rollupInfo.DataTypeId == Constants.DataTypeWholeNumber)
                {
                    rollupValue = decimal.Round(rolledupValue.Value, 0, MidpointRounding.AwayFromZero);
                }

                return rollupValue;
            }

            return rolledupValue;
        }
        /// <summary>
        /// Calculates the roll up value based on the rollup method set
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <returns>Calculated rollup value</returns>
        private decimal? CalculateRollupTargetValue(RollupInfo rollupInfo)
        {
            decimal? rolledupValue = null;
            // Retrieve all child targets
            var childTargetIds = targetRepository.GetAll().Where(x =>
                x.ParentTargetId == rollupInfo.ParentTargetId
                && x.IsActive).Select(y => y.Id).ToList();

            switch (rollupInfo.RollupMethodId)
            {
                // "Sum of Children"
                case Constants.RollupMethodSumOfChildren:
                    {
                        IRollupStrategy sumOfChildrenRollup = CreateSumOfChildrenRollupStrategy();
                        rolledupValue = sumOfChildrenRollup.CalculateRollupTarget(rollupInfo,
                            childTargetIds);
                        break;
                    }

                // "Average of Children"
                case Constants.RollupMethodAverageOfChildren:
                    {
                        IRollupStrategy avgOfChildrentRollup =
                            CreateAverageOfChildrenRollupStrategy();
                        rolledupValue = avgOfChildrentRollup.CalculateRollupTarget(rollupInfo,
                            childTargetIds);
                        break;
                    }

                // "Same as Child"
                case Constants.RollupMethodSameAsChild:
                    {
                        IRollupStrategy sameAsChildRollup = CreateSameAsChildRollupStrategy();
                        rolledupValue = sameAsChildRollup.CalculateRollupTarget(rollupInfo,
                            childTargetIds);
                        break;
                    }
            }

            // Round the rolled up value to two decimal places
            if (rolledupValue.HasValue)
            {
                var rollupValue = decimal.Round(rolledupValue.Value, 2, MidpointRounding.AwayFromZero);
                if (rollupInfo.DataTypeId == Constants.DataTypeWholeNumber)
                {
                    rollupValue = decimal.Round(rolledupValue.Value, 0, MidpointRounding.AwayFromZero);
                }

                return rollupValue;
            }

            return rolledupValue;
        }

        /// <summary>
        /// Add/update a daily roll up entry
        /// </summary>
        /// <param name="parentActual">Parent's actual(rolled up value)</param>
        /// <param name="parentDailyActualId">Id of the parent actual entry</param>
        /// <param name="rollupInfo">Rollup information</param>
        private void AddOrUpdateDailyRollupEntry(decimal? parentActual, int? parentDailyActualId,
            RollupInfo rollupInfo)
        {
            ActualItem rollupEntry = new ActualItem();
            // Assign target and scorecard id
            rollupEntry.TargetId = rollupInfo.ParentTargetId.Value;
            rollupEntry.ScorecardId = rollupInfo.ParentScorecardId;
            rollupEntry.ActualValue = parentActual;
            // Retrieves the daily goal for the parent
            rollupEntry.GoalValue = goalCalculator.GetDailyGoal(rollupInfo.ParentTargetId.Value,
                rollupInfo.ActualEntry.Date.Month, rollupInfo.ActualEntry.Date.Day);
            rollupEntry.Date = rollupInfo.ActualEntry.Date;

            if (parentDailyActualId.HasValue)
            {
                rollupEntry.Id = parentDailyActualId.Value;
                actualsModifier.UpdateDailyActual(rollupEntry, rollupInfo.GoalTypeId,
                     rollupInfo.DataTypeId, rollupInfo.Username);
            }
            else
            {
                actualsModifier.AddDailyActual(rollupEntry, rollupInfo.GoalTypeId,
                     rollupInfo.DataTypeId, rollupInfo.Username);
            }
        }

        /// <summary>
        /// Add/update a monthly rollup entry
        /// </summary>
        /// <param name="parentActual">Parent's actual(rolled up value)</param>
        /// <param name="parentMonthlyActualId">Id of the parent actual entry</param>
        /// <param name="rollupInfo">Rollup information</param>
        private void AddOrUpdateMonthlyRollupEntry(decimal? parentActual,
            int? parentMonthlyActualId, RollupInfo rollupInfo)
        {
            ActualItem rollupEntry = new ActualItem();
            rollupEntry.ActualValue = parentActual;
            // Assign target and scorecard id
            rollupEntry.TargetId = rollupInfo.ParentTargetId.Value;
            rollupEntry.ScorecardId = rollupInfo.ParentScorecardId;

            // Retrieves the monthly goal for the parent
            rollupEntry.GoalValue = goalCalculator.GetMonthlyGoal(
                rollupInfo.ParentTargetId.Value, rollupInfo.ActualEntry.Date.Month);
            rollupEntry.Date = rollupInfo.ActualEntry.Date;

            if (parentMonthlyActualId.HasValue)
            {
                rollupEntry.Id = parentMonthlyActualId.Value;
                actualsModifier.UpdateMonthlyActual(rollupEntry, rollupInfo.GoalTypeId,
                     rollupInfo.Username);
            }
            else
            {
                actualsModifier.AddMonthlyActual(rollupEntry, rollupInfo.GoalTypeId,
                    rollupInfo.Username);
            }
        }

        /// <summary>
        /// Add/update a target rollup
        /// </summary>
        /// <param name="parentActual">Parent's actual(rolled up value)</param>
        /// <param name="parentMonthlyActualId">Id of the parent actual entry</param>
        /// <param name="rollupInfo">Rollup information</param>
        private void AddOrUpdateTargetRollup(RollupInfo rollupInfo, IList<RollupTargetItem> rollupTargets)
        {
            if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
            {

                targetModifier.AddOrUpdateDailyTargets(rollupInfo.ParentTargetId.Value, rollupTargets, rollupInfo.Username);
            }
            else
            {
                targetModifier.AddOrUpdateMonthlyTargets(rollupInfo.ParentTargetId.Value, rollupTargets, rollupInfo.Username);
            }
        }

        /// <summary>
        /// Updates the daily actual status.
        /// </summary>
        /// <param name="rollupInfo">The rollup information.</param>
        /// <param name="rollupTargets">The rollup targets.</param>
        private void UpdateActualStatus(RollupInfo rollupInfo, DateTime selectedDate, IList<RollupTargetItem> rollupTargets, ref bool updateStatus)
        {
            if (!updateStatus)
            {
                // if rollup targets and actual entry for the given child is missing, check whether roll-up actual from other children exists for the parent target
                updateStatus = (dailyActualRepository.GetAll().Where(x => x.TargetId == rollupInfo.ParentTargetId && x.Date.Month == selectedDate.Month)?.Count() ?? 0) > 0;
            }

            if (rollupTargets.Any(x => x.TargetEntryDate.Month == selectedDate.Month) && updateStatus)
            {
                if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                {

                    actualsModifier.UpdateDailyActualStatusAndGoalForMonth(rollupInfo.ParentTargetId.Value, selectedDate, rollupInfo.Username);
                }
                else
                {
                    actualsModifier.UpdateMonthlyActualStatusAndGoalForMonth(rollupInfo.ParentTargetId.Value, selectedDate, rollupInfo.Username);
                }
            }
        }

        #endregion

        #region Protected Method(s)
        /// <summary>
        /// Creates an instance of "Sum Of Children" rollup and returns
        /// </summary>
        /// <returns></returns>
        protected virtual IRollupStrategy CreateSumOfChildrenRollupStrategy()
        {
            if (sumOfChildrenRollup == null)
            {
                sumOfChildrenRollup = new SumOfChildrenRollup(
                    dailyActualRepository, monthlyActualRepository, targetRepository);
            }
            return sumOfChildrenRollup;
        }

        /// <summary>
        /// Creates an instance of "Average Of Children" rollup and returns
        /// </summary>
        /// <returns></returns>
        protected virtual IRollupStrategy CreateAverageOfChildrenRollupStrategy()
        {
            if (avgOfChildrenRollup == null)
            {
                avgOfChildrenRollup = new AverageOfChildrenRollup(dailyActualRepository,
                    monthlyActualRepository, targetRepository);
            }
            return avgOfChildrenRollup;
        }

        /// <summary>
        /// Creates an instance of "Same As Child" rollup and returns
        /// </summary>
        /// <returns></returns>
        protected virtual IRollupStrategy CreateSameAsChildRollupStrategy()
        {
            if (sameAsChildRollup == null)
            {
                sameAsChildRollup = new SameAsChildRollup(dailyActualRepository,
                    monthlyActualRepository, targetRepository);
            }
            return sameAsChildRollup;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="actualsModifier">Actuals modifier</param>
        /// <param name="targetRepository">Target repository</param>
        /// <param name="dailyActualRepository">Daily actual repository</param>
        /// <param name="monthlyActualRepository">Monthly actual repository</param>
        /// <param name="goalCalculator">Scorecard goal calculator</param>
        public RollupManager(ActualsModifier actualsModifier,
                             TargetModifier targetModifier,
                             IBaseRepository<Target> targetRepository,
                             IBaseRepository<DailyActual> dailyActualRepository,
                             IBaseRepository<MonthlyActual> monthlyActualRepository,
                             ScorecardGoalCalculator goalCalculator,
                             ScorecardRecordablesCalculator recordableCalculator)
        {
            this.actualsModifier = actualsModifier;
            this.targetModifier = targetModifier;
            this.targetRepository = targetRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.goalCalculator = goalCalculator;
            this.recordablesCalculator = recordableCalculator;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Performs roll up for an actual entry
        /// </summary>
        /// <param name="actualEntry">Actual entry to rollup</param>
        /// <param name="username">Logged in user name</param>
        public virtual void PerformRollup(ActualItem actualEntry, bool updateRecordable, string username)
        {
            // Find target with the given id
            var target = targetRepository.Get(actualEntry.TargetId);
            if (target.ParentTargetId != null)
            {
                // Fill the parameters needed for rollup operation
                RollupInfo rollupInfo = new RollupInfo();
                rollupInfo.ActualEntry = actualEntry;
                rollupInfo.Username = username;
                rollupInfo.ParentTargetId = target.ParentTargetId;
                rollupInfo.ParentScorecardId = target.ParentTarget.ScorecardId;
                rollupInfo.RollupMethodId = target.ParentTarget.RollUpMethodId;
                rollupInfo.DataTypeId = target.Metric.DataTypeId;
                rollupInfo.GoalTypeId = target.Metric.GoalTypeId;
                rollupInfo.TrackingMethodId = target.TrackingMethodId;
                rollupInfo.UpdateRecordable = updateRecordable;

                // Invoke the recursive rollup operation
                RollupRecursively(rollupInfo);
            }
        }

        /// <summary>
        /// Performs the rollup on succeeding days on previous day update.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="actualEntryStartDate">The actual entry start date.</param>
        /// <param name="actualEntryEndDate">The actual entry end date.</param>
        /// <param name="username">The username.</param>
        public virtual void PerformRollupOnSucceedingDays(Target target, DateTime actualEntryStartDate, DateTime actualEntryEndDate, string username)
        {
            if (target.ParentTargetId != null)
            {
                // Fill the parameters needed for rollup operation
                RollupInfo rollupInfo = new RollupInfo();
                rollupInfo.ActualEntry = new ActualItem();
                rollupInfo.Username = username;
                rollupInfo.ParentTargetId = target.ParentTargetId;
                rollupInfo.ParentScorecardId = target.ParentTarget.ScorecardId;
                rollupInfo.RollupMethodId = target.ParentTarget.RollUpMethodId;
                rollupInfo.DataTypeId = target.Metric.DataTypeId;
                rollupInfo.GoalTypeId = target.Metric.GoalTypeId;
                rollupInfo.TrackingMethodId = target.TrackingMethodId;

                // Invoke the recursive rollup operation
                RollupRecursivelyForSucceedingDays(rollupInfo, actualEntryStartDate, actualEntryEndDate);
            }
        }

        /// <summary>
        /// Performs roll up for targets
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="targetEntryDate">The target entry date.</param>
        /// <param name="username">Logged in user name</param>
        public virtual void PerformTargetRollup(Target target, string username, bool updateStatus)
        {
            if (target.ParentTargetId != null)
            {
                // Fill the parameters needed for rollup operation
                RollupInfo rollupInfo = new RollupInfo();
                rollupInfo.Username = username;
                rollupInfo.ParentTargetId = target.ParentTargetId;
                rollupInfo.ParentScorecardId = target.ParentTarget.ScorecardId;
                rollupInfo.RollupMethodId = target.ParentTarget.RollUpMethodId;
                rollupInfo.DataTypeId = target.Metric.DataTypeId;
                rollupInfo.GoalTypeId = target.Metric.GoalTypeId;
                rollupInfo.TrackingMethodId = target.TrackingMethodId;
                // Invoke the recursive rollup operation
                RollupTargetRecursively(rollupInfo, updateStatus);
            }
        }

        /// <summary>
        /// Performs roll up for targets for a specific Month
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="username">Logged in user name</param>
        /// <param name="updateStatus">if set to <c>true</c> [update status].</param>
        public virtual void PerformTargetRollup(Target target, DateTime selectedDate, string username, bool updateStatus)
        {
            if (target.ParentTargetId != null)
            {
                // Fill the parameters needed for rollup operation
                RollupInfo rollupInfo = new RollupInfo();
                rollupInfo.Username = username;
                rollupInfo.ParentTargetId = target.ParentTargetId;
                rollupInfo.ParentScorecardId = target.ParentTarget.ScorecardId;
                rollupInfo.RollupMethodId = target.ParentTarget.RollUpMethodId;
                rollupInfo.DataTypeId = target.Metric.DataTypeId;
                rollupInfo.GoalTypeId = target.Metric.GoalTypeId;
                rollupInfo.TrackingMethodId = target.TrackingMethodId;
                // Invoke the recursive rollup operation
                RollupTargetRecursively(rollupInfo, selectedDate, updateStatus);
            }
        }

        #endregion
    }
}
