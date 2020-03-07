using NDMS.Business.Common;
using NDMS.Business.Converters;
using NDMS.Business.Interfaces;
using NDMS.Business.Rollup;
using NDMS.Business.Validators;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Manager class to add/update actuals
    /// </summary>
    public class ActualsManager : IActualsManager
    {
        #region Field(s)
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Recordable Repository
        /// </summary>
        private IBaseRepository<Recordable> recordableRepository;

        /// <summary>
        /// The scorecard workday tracker repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository;

        /// <summary>
        /// Monthly Target repository
        /// </summary>
        private IBaseRepository<MonthlyTarget> monthlyTargetRepository;

        /// <summary>
        /// Daily target repository
        /// </summary>
        private IBaseRepository<DailyTarget> dailyTargetRepository;

        /// <summary>
        /// Daily target repository
        /// </summary>
        private IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;
        /// <summary>
        /// Metric Repository
        /// </summary>
        private IBaseRepository<Metric> metricRepository;

        /// <summary>
        /// Reference to actuals modifier
        /// </summary>
        private ActualsModifier actualsModifier;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Rollup manager instance
        /// </summary>
        private RollupManager rollupManager;

        /// <summary>
        /// Reference to Recordable Calculator
        /// </summary>
        private ScorecardRecordablesCalculator recordableCalculator;

        /// <summary>
        /// The holiday calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        /// <summary>
        /// The target modifier
        /// </summary>
        private TargetModifier targetModifier;

        /// <summary>
        /// Scorecard Repository
        /// </summary>
        private IBaseRepository<Scorecard> scorecardRepository;

        /// <summary>
        /// Reference to target validator
        /// </summary>
        private TargetValidator targetValidator;

        #endregion

        #region Private Method(s)
        /// <summary>
        /// Validate Monthly Actuals Entry
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="date">given date</param>
        private void ValidateMonthlyActuals(Target target, DateTime date)
        {
            if (target.IsCascaded)
            {
                throw new NDMSBusinessException(Constants.CascadedMetricActualEntryErrorMessage);
            }
            var targetMonths = CalendarUtility.GetMonthsBetweenDates(target.EffectiveStartDate,
                target.EffectiveEndDate);
            if (!targetMonths.Any(x => x.Id == date.Month))
            {
                throw new NDMSBusinessException(Constants.ActualEntryTargetMonthsErrorMessage);
            }
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            //check whether the month is current or previous month
            if (!(date.Month == currentDate.Month || date.Month == currentDate.AddMonths(-1).Month))
            {
                throw new NDMSBusinessException(Constants.ActualEntryMonthErrorMessage);
            }
            if (date.Month == currentDate.Month)
            {
                if (target.EffectiveStartDate > currentDate)
                {
                    throw new NDMSBusinessException(Constants.MonthlyActualEntryFutureDateErrorMessage);
                }
            }
        }

        /// <summary>
        /// Validate Daily Actuals Entry
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="date">given date</param>
        private void ValidateDailyActuals(Target target, DateTime date)
        {
            if (target.IsCascaded)
            {
                throw new NDMSBusinessException(Constants.CascadedMetricActualEntryErrorMessage);
            }

            ValidateActualEntryDates(target, date);
        }

        /// <summary>
        /// Validate the date in which actual is entered
        /// </summary>
        /// <param name="target"></param>
        /// <param name="date"></param>
        private void ValidateActualEntryDates(Target target, DateTime date)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            if (!(date >= target.EffectiveStartDate && date <= target.EffectiveEndDate))
            {
                throw new NDMSBusinessException(Constants.ActualEntryTargetDatesErrorMessage);
            }

            //check whether the given date is a future date
            if (date.Date > currentDate)
            {
                throw new NDMSBusinessException(Constants.ActualEntryFutureDateErrorMessage);
            }
            //check whether the month is current or previous month
            if (!(date.Month == currentDate.Month || date.Month == currentDate.AddMonths(-1).Month))
            {
                throw new NDMSBusinessException(Constants.ActualEntryMonthErrorMessage);
            }

            //check if given date is a holiday
            if (holidayCalculator.CheckIfDateIsaHoliday(target.ScorecardId, date))
            {
                throw new NDMSBusinessException(Constants.ActualEntryOnHolidayErrorMessage);
            }
        }

        /// <summary>
        /// Validate the date in which holiday is marked
        /// </summary>
        /// <param name="target">Corresponding target</param>
        /// <param name="date">Date on which holiday is marked</param>
        private void ValidateHolidayEntryDates(Target target, DateTime date,bool isWorkday)
        {
          
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            if (!(date >= target.EffectiveStartDate && date <= target.EffectiveEndDate))
            {
                throw new NDMSBusinessException(Constants.HolidayEntryTargetDatesErrorMessage);
            }

            //check whether the date is less than previous month date
            var previousMonthDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);
            if (date.Date <= previousMonthDate)
            {
                throw new NDMSBusinessException(Constants.HolidayEntryMonthErrorMessage);
            }

            //check whether the date has any actual entry
            int actualCount = dailyActualRepository.GetAll().
                Where(x => x.TargetId == target.Id && x.Date == date && x.ActualValue != null).Count();
            if (actualCount > 0)
            {
                throw new NDMSBusinessException(Constants.HolidayEntryActualExistsErrorMessage);

            }

            // Get all secondary metrics
            var scorecardTargets = targetRepository.GetAll().Where(x => x.ScorecardId == target.ScorecardId
                                                            && x.Id != target.Id
                                                            && x.CalendarYearId == target.CalendarYearId
                                                            && date >= x.EffectiveStartDate && date <= x.EffectiveEndDate
                                                            && x.IsActive
                                                            && x.TrackingMethodId == Constants.TrackingMethodDaily).Select(x => x.Id);

            //check whether the date has any actual entry for any secondary metrics
            int metricActualCount = dailyActualRepository.GetAll().
                Where(x => scorecardTargets.Any(id => id == x.TargetId) && x.Date == date && x.ActualValue != null).Count();

            if (metricActualCount > 0 && !isWorkday)
            {
                throw new NDMSBusinessException(Constants.HolidayEntryMetricActualExistsErrorMessage);

            }
        }

        /// <summary>
        /// Check if actual/target needs to be rolled up for this target entry
        /// </summary>
        /// <param name="target">Target to check</param>
        /// <returns>True if actuals needs to be rolled up</returns>
        private bool CheckIfActualOrTargetRollupRequired(Target target)
        {
            bool rollUpRequired = false;
            if (target.ParentTargetId.HasValue)
            {
                rollUpRequired = true;
            }
            return rollUpRequired;
        }

        /// <summary>
        /// Checks if actual exists.
        /// </summary>
        /// <param name="selectedDate">The selected date.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool CheckIfActualExists(DateTime selectedDate, Target target)
        {
            int? dailyActualCount = dailyActualRepository.GetAll().Where(x => x.TargetId == target.Id && x.Date.Month == selectedDate.Month)?.Count();
            if (dailyActualCount.HasValue)
            {
                return dailyActualCount.Value > 0;
            }

            return false;
        }

        /// <summary>
        /// Checks if update required for existing actual in case editing a previous date.
        /// </summary>
        /// <param name="actualRequestDate">The actual request date.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool CheckIfUpdateRequiredForExistingActual(DateTime actualRequestDate, Target target)
        {
            if (target.GraphPlottingMethodId != Constants.GraphPlottingMethodCumulative)
            {
                return false;
            }

            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                return CheckIfUpdateRequiredForDailyActual(actualRequestDate, target);
            }
            else if (target.TrackingMethodId == Constants.TrackingMethodMonthly)
            {
                return CheckIfUpdateRequiredForMonthlyActual(actualRequestDate, target);
            }

            return false;
        }

        /// <summary>
        /// Checks if update required for daily actual in case editing a previous date.
        /// </summary>
        /// <param name="actualRequestDate">The actual request date.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool CheckIfUpdateRequiredForDailyActual(DateTime actualRequestDate, Target target)
        {
            bool isPastDateUpdate;
            if (target.ParentTargetId.HasValue)
            {
                isPastDateUpdate = actualRequestDate < TimeZoneUtility.GetCurrentTimestamp().Date;
            }
            else
            {
                isPastDateUpdate = dailyActualRepository.GetAll().Any(x => x.TargetId == target.Id && x.Date > actualRequestDate && x.ActualValue != null);
            }

            return isPastDateUpdate;
        }

        /// <summary>
        /// Checks if update required for monthly actual in case editing a previous date.
        /// </summary>
        /// <param name="actualRequestDate">The actual request date.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool CheckIfUpdateRequiredForMonthlyActual(DateTime actualRequestDate, Target target)
        {
            bool isPastMonthUpdate;
            if (target.ParentTargetId.HasValue)
            {
                isPastMonthUpdate = actualRequestDate.Month < TimeZoneUtility.GetCurrentTimestamp().Date.Month;
            }
            else
            {
                isPastMonthUpdate = dailyActualRepository.GetAll().Any(x => x.TargetId == target.Id && x.Date.Month > actualRequestDate.Month);
            }

            return isPastMonthUpdate;
        }

        /// <summary>
        /// Checks if update recordable entry required.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        private bool CheckIfUpdateRecordableRequired(Target target)
        {
            bool isNumberOfDaysWithOutRecordablesEnabled = Convert.ToBoolean(
                ConfigurationManager.AppSettings[AppSettingsKeys.
                        EnableNumberOfDaysWithoutRecordables]);

            if (isNumberOfDaysWithOutRecordablesEnabled)
            {
                int recordableKPIId = Convert.ToInt32(
                ConfigurationManager.AppSettings[AppSettingsKeys.RecordableKPIId]);

                int recordableMetricId = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.RecordableMetricId]);

                if (target.KPIId == recordableKPIId && target.MetricId == recordableMetricId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Performs roll up operation for an actual entry
        /// </summary>
        /// <param name="target">Target against which actual entry is made</param>
        /// <param name="actualEntry">Actual entry which needs to be rolled up</param>
        /// <param name="username">Actual entry which needs to be rolled up</param>
        private void PerformRollup(Target target, ActualItem actualEntry, bool updateRecordable, string username)
        {
            RollupManager rollupMgr = CreateRollupManager();
            rollupMgr.PerformRollup(actualEntry, updateRecordable, username);
        }

        /// <summary>
        /// Updates the existing actual status and per day goal.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="actualEntry">The actual entry.</param>
        /// <param name="isMarkUnmarkWorkday">if set to <c>true</c> [is mark unmark workday].</param>
        /// <param name="username">The username.</param>
        private void UpdateExistingActual(Target target, ActualItem actualEntry, bool isMarkUnmarkWorkday, string username)
        {
            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                UpdateExistingDailyActual(target, actualEntry, isMarkUnmarkWorkday, username);
            }
            else
            {
                UpdateExistingMonthlyActual(target, actualEntry, username);
            }
        }

        /// <summary>
        /// Update the daily rate.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="IsMarkWorkday">if set to <c>true</c> [is mark workday].</param>
        /// <param name="date">The date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void UpdateDailyRate(int targetId, bool IsMarkWorkday, DateTime date, int loggedInUserId)
        {

            actualsModifier.UpdateDailyRateValue(targetId, IsMarkWorkday, date, loggedInUserId);


        }
        /// <summary>
        /// Updates the existing daily targets.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="IsMarkWorkday">if set to <c>true</c> [is mark workday].</param>
        /// <param name="date">The date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void UpdateExistingDailyTargets(Target target, bool IsMarkWorkday, DateTime date, int loggedInUserId)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            RecalculateDailyTargets(target, date, loggedInUserId);
          
        }
        /// <summary>
        /// Distribute the existing targets.
        /// </summary>
        /// <param name="request">The request to generate daily targets.</param>
        private IEnumerable<DailyTargetItem> DistributeExistingDailyTarget(GenerateDailyTargetsRequest request)
        {
            var calendarYear = yearRepository.Get(request.YearId);
            //gets the year
            int targetYear = CalendarUtility.GetYearOfTheMonth(calendarYear, request.MonthId);
            //gets the day list of the required month
            var dayList = CalendarUtility.GetAllDaysInMonth(targetYear, request.MonthId).ToList();

            var dailyTargetList = new List<DailyTargetItem>();

            dayList.ForEach(day =>
            {
                dailyTargetList.Add(new DailyTargetItem
                {
                    Day = day,
                    IsHoliday = holidayCalculator.CheckIfDateIsaHoliday(request.ScorecardId, day, request.MonthId, targetYear),
                    IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(request.EffectiveStartDate, request.EffectiveEndDate, new DateTime(targetYear, request.MonthId, day))
                });
            });

            var metric = metricRepository.Get(request.MetricId); // distribute monthly goal evenly

            if (request.MonthlyGoalValue.HasValue)
            {
                dailyTargetList = goalCalculator.DistributeMonthlyGoalValueAmongWorkdaysEvenly(dailyTargetList, metric.DataTypeId, request.MonthlyGoalValue.Value);
            }

            return dailyTargetList;
        }

        /// <summary>
        /// Recalculate the daily targets.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="date">The date.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void RecalculateDailyTargets(Target target, DateTime date, int loggedInUserId)
        {
            var monthTarget = target.MonthlyTargets.Where(x => x.Month == date.Month).FirstOrDefault();
            if (monthTarget != null)
            {
                List<DailyTargetItem> dailyTargetItemList = DistributeExistingDailyTarget(
                                            new GenerateDailyTargetsRequest
                                            {
                                                ScorecardId = target.ScorecardId,
                                                YearId = target.CalendarYearId,
                                                MonthId = date.Month,
                                                MetricId = target.MetricId,
                                                EffectiveStartDate = target.EffectiveStartDate,
                                                TargetEntryMethodId = target.TargetEntryMethodId.Value,
                                                EffectiveEndDate = target.EffectiveEndDate,
                                                MonthlyGoalValue = monthTarget.MaxGoalValue,
                                                DailyRateValue = null,
                                            }).ToList();
                var added = dailyTargetItemList.Where(x => !monthTarget.DailyTargets.Any(y => y.Day == x.Day) && x.GoalValue != null).ToList();
                var existing = dailyTargetItemList.Where(x => monthTarget.DailyTargets.Any(y => y.Day == x.Day)).ToList();
                var deleted = monthTarget.DailyTargets.Where(x => !dailyTargetItemList.Any(y => y.Day == x.Day)).ToList();
                //New Targets in New list are added 
                added.ForEach(addedtarget =>
                {
                    monthTarget.DailyTargets.Add(TargetConverters.ConvertDailyTargetItemToDailyTargetEntity(addedtarget, monthTarget, loggedInUserId));
                });
                //Existing daily target details are updated with the values in new list
                existing.ForEach(existingtarget =>
                {
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == existingtarget.Day).MaxGoalValue = existingtarget.GoalValue;
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == existingtarget.Day).LastModifiedBy = loggedInUserId;
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == existingtarget.Day).LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                });
                //Daily Targets which present in Existing daily target list and not in New list are deleted.
                deleted.ForEach(deletedTarget =>
                {
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == deletedTarget.Day).MaxGoalValue = null;
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == deletedTarget.Day).LastModifiedBy = loggedInUserId;
                    monthTarget.DailyTargets.FirstOrDefault(x => x.Day == deletedTarget.Day).LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                });
                monthlyTargetRepository.AddOrUpdate(monthTarget);
                monthlyTargetRepository.Save();
            }

        }

        /// <summary>
        /// insert the daily targets.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="monthTargets">The month targets.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        private void AddDailyTargets(Target target, IEnumerable<MonthlyTarget> monthTargets, int loggedInUserId)
        {
            foreach (MonthlyTarget monthTarget in monthTargets)
            {
                List<DailyTargetItem> dailyTargetItemList = DistributeExistingDailyTarget(
                                            new GenerateDailyTargetsRequest
                                            {
                                                ScorecardId = target.ScorecardId,
                                                YearId = target.CalendarYearId,
                                                MonthId = monthTarget.Month,
                                                MetricId = target.MetricId,
                                                EffectiveStartDate = target.EffectiveStartDate,
                                                TargetEntryMethodId = target.TargetEntryMethodId.Value,
                                                EffectiveEndDate = target.EffectiveEndDate,
                                                MonthlyGoalValue = monthTarget.MaxGoalValue,
                                                DailyRateValue = monthTarget.DailyRate,
                                            }).ToList();
                var added = dailyTargetItemList.Where(x => x.GoalValue != null).ToList();
                var existingDailyTargets = dailyTargetRepository.GetAll().Where(x => x.MonthlyTargetId == monthTarget.Id).ToList();
                //New Targets in New list are added 
                added.ForEach(addedtarget =>
                {
                    if (!existingDailyTargets.Any(x=>x.Day == addedtarget.Day))
                    {
                        monthTarget.DailyTargets.Add(TargetConverters.ConvertDailyTargetItemToDailyTargetEntity(addedtarget, monthTarget, loggedInUserId));
                    }
                });
                monthlyTargetRepository.AddOrUpdate(monthTarget);
            }
            monthlyTargetRepository.Save();
        }
        ///<summary>
        ///Updates existing daily actual values for succeeding dates if user updates actual for a previous date
        ///</summary>
        private void UpdateExistingDailyActual(Target target, ActualItem actualEntry, bool isMarkUnmarkWorkday, string username)
        {
            DateTime actualEntryEndDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            DateTime actualEntryStartDate = actualEntry.Date.AddDays(1);

            if (actualEntry.Date.Month == actualEntryEndDate.AddMonths(-1).Month)
            {
                actualEntryEndDate = actualEntryEndDate.AddDays(-(actualEntryEndDate.Day));
            }

            // Flag to perform Roll up. 
            bool isRollupRequired = CheckIfActualOrTargetRollupRequired(target) && !isMarkUnmarkWorkday;

            if (isRollupRequired)
            {
                PerformRollupOnSucceedingDays(target, actualEntryStartDate, actualEntryEndDate, username);
            }

            if (isMarkUnmarkWorkday)
            {
                actualsModifier.UpdateDailyActualStatusAndGoalForMonth(target.Id, actualEntry.Date, username);
            }
            else
            {
                var rollupRequiredActuals = dailyActualRepository.GetAll()
                                           .Where(x => x.TargetId == target.Id && x.Date >= actualEntryStartDate.Date && x.ActualValue != null).ToList()
                                           .Select(x => new ActualItem
                                           {
                                               ActualValue = x.ActualValue,
                                               Date = x.Date,
                                               Id = x.Id,
                                               GoalValue = x.GoalValue,
                                               TargetId = x.TargetId,
                                               ScorecardId = x.Target.ScorecardId
                                           }).ToList();

                while (actualEntryStartDate <= actualEntryEndDate)
                {
                    var existingActual = rollupRequiredActuals.FirstOrDefault(x => x.Date == actualEntryStartDate);
                    if (existingActual != null)
                    {
                        existingActual.GoalValue = goalCalculator.GetDailyGoal(existingActual.TargetId, actualEntryStartDate.Month, actualEntryStartDate.Day);
                        actualsModifier.UpdateDailyActual(existingActual, target.Metric.GoalTypeId, target.Metric.DataTypeId, username);
                    }
                    actualEntryStartDate = actualEntryStartDate.AddDays(1);
                }
            }

        }

        ///<summary>
        ///Updates existing monthly actual values for succeeding dates if user updates actual for a previous date
        ///</summary>
        private void UpdateExistingMonthlyActual(Target target, ActualItem actualEntry, string username)
        {
            DateTime actualEntryEndDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            DateTime actualEntryStartDate = actualEntry.Date.AddDays(1);
            bool isRollupRequired = CheckIfActualOrTargetRollupRequired(target);

            if (isRollupRequired)
            {
                PerformRollupOnSucceedingDays(target, actualEntryStartDate, actualEntryEndDate, username);
            }

            var rollupRequiredActuals = dailyActualRepository.GetAll()
                                          .Where(x => x.TargetId == target.Id && x.Date.Month > actualEntry.Date.Month && x.ActualValue != null).ToList()
                                          .Select(x => new ActualItem
                                          {
                                              ActualValue = x.ActualValue,
                                              Date = x.Date,
                                              Id = x.Id,
                                              GoalValue = x.GoalValue,
                                              TargetId = x.TargetId,
                                              ScorecardId = x.Target.ScorecardId
                                          }).ToList();
            while (actualEntryStartDate.Month <= actualEntryEndDate.Month)
            {
                var existingActual = rollupRequiredActuals.FirstOrDefault(x => x.Date.Month == actualEntryStartDate.Month);
                if (existingActual != null)
                {
                    actualsModifier.UpdateMonthlyActual(existingActual, target.Metric.GoalTypeId, username);
                }

                actualEntryStartDate = actualEntryStartDate.AddMonths(1);
            }
        }

        /// <summary>
        /// Performs the rollup on succeeding days in case of a previous date update.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="actualEntryStartDate">The actual entry start date.</param>
        /// <param name="actualEntryEndDate">The actual entry end date.</param>
        /// <param name="calculateGoal">if set to <c>true</c> [calculate goal].</param>
        /// <param name="username">The username.</param>
        private void PerformRollupOnSucceedingDays(Target target, DateTime actualEntryStartDate, DateTime actualEntryEndDate, string username)
        {
            RollupManager rollupMgr = CreateRollupManager();
            rollupMgr.PerformRollupOnSucceedingDays(target, actualEntryStartDate, actualEntryEndDate, username);
        }

        /// <summary>
        /// Performs the rollup on target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="actualEntryDate">The actual entry date.</param>
        /// <param name="userName">Name of the user.</param>
        private void PerformRollupTarget(Target target, DateTime actualEntryDate, string userName, bool updateStatus, bool isFullTargetRollUp = false)
        {
            RollupManager rollupMgr = CreateRollupManager();
            if (isFullTargetRollUp)
            {
                rollupManager.PerformTargetRollup(target, userName, updateStatus);
            }
            else
            {
                rollupMgr.PerformTargetRollup(target, actualEntryDate, userName, updateStatus);
            }
        }

        /// <summary>
        /// Method to validate duplicate entry for daily actual entry
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="selectedDate">selected date</param>
        private void ValidateDuplicateDailyActual(int targetId, DateTime selectedDate)
        {
            bool duplicateDailyActualExists = dailyActualRepository.GetAll()
                  .Any(x => x.TargetId == targetId && x.Date == selectedDate);
            if (duplicateDailyActualExists)
            {
                throw new NDMSBusinessException(Constants.DuplicateDailyActualEntryErrorMessage);
            }
        }

        /// <summary>
        /// Method to validate duplicate entry for monthly actual entry
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="selectedDate">selected date</param>
        private void ValidateDuplicateMonthlyActual(int targetId, DateTime selectedDate)
        {
            bool duplicateMonthlyActualExists = monthlyActualRepository.GetAll()
                  .Any(x => x.TargetId == targetId && x.Month == selectedDate.Month);
            if (duplicateMonthlyActualExists)
            {
                throw new NDMSBusinessException(Constants.DuplicateMonthlyActualEntryErrorMessage);
            }
        }

        /// <summary>
        /// Updates the holiday entry.
        /// </summary>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <param name="existingActual">The existing actual.</param>
        /// <returns></returns>
        private int UpdateHoliday(int loggedInUserId, DailyActual existingActual)
        {
            existingActual.ActualValue = null;
            existingActual.GoalValue = null;
            existingActual.Status = ActualStatus.Holiday;
            existingActual.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingActual.LastModifiedBy = loggedInUserId;
            //add history of daily actual
            existingActual.DailyActualHistory.Add(
                ActualConverters.ConvertDailyActualToDailyActualHistory(existingActual));
            // Saves the data
            dailyActualRepository.Save();

            return existingActual.Id;
        }

        /// <summary>
        /// Add new holiday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="target">The target.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        private int AddHoliday(DateTime date, Target target, int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var dailyActual = new DailyActual()
            {
                Date = date,
                Status = ActualStatus.Holiday,
                TargetId = target.Id,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId
            };
            dailyActual.DailyActualHistory = new List<DailyActualHistory>()
                    {
                        ActualConverters.ConvertDailyActualToDailyActualHistory(dailyActual)
                    };
            dailyActualRepository.AddOrUpdate(dailyActual);
            // Saves the data
            dailyActualRepository.Save();

            return dailyActual.Id;
        }

        /// <summary>
        /// set the existing marked workday or holiday inactive
        /// </summary>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="existingScorecardDayTracker">The existing scorecard day tracker.</param>
        /// <returns></returns>
        private int ClearMarkedDaysForDate(int loggedInUserId, DateTime date, ScorecardWorkdayTracker existingScorecardDayTracker)
        {
            //update daily actual with new changes
            existingScorecardDayTracker.IsActive = false;
            existingScorecardDayTracker.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingScorecardDayTracker.LastModifiedBy = loggedInUserId;
            // Saves the data
            scorecardWorkdayTrackerRepository.Save();
            return existingScorecardDayTracker.Id;
        }

        /// <summary>
        /// Adds the workday.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="target">The target.</param>
        /// <param name="loggedInUserId">The logged in user identifier.</param>
        /// <returns></returns>
        private int AddWorkdayMarking(DateTime date, int scorecardId, bool isWorkday, int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var workdayTracker = new ScorecardWorkdayTracker()
            {
                Date = date,
                ScorecardId = scorecardId,
                IsWorkDay = isWorkday,
                IsActive = true,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId
            };

            scorecardWorkdayTrackerRepository.AddOrUpdate(workdayTracker);
            // Saves the data
            scorecardWorkdayTrackerRepository.Save();
            return workdayTracker.Id;
        }        

        #endregion

        #region Protected Method(s)
        /// <summary>
        /// Creates an instance of "Average Of Children" rollup and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ActualsModifier CreateActualsModifier(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<MonthlyTarget> monthlyTargetRepository,
            IBaseRepository<DailyTarget> dailyTargetRepository,
            IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository

            )
        {
            if (actualsModifier == null)
            {
                actualsModifier = new ActualsModifier(targetRepository, dailyActualRepository,
                    monthlyActualRepository, userRepository, scorecardWorkdayPatternRepository,
                    scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository, monthlyTargetRepository, dailyTargetRepository, dailyTargetHistoryRepository);
            }
            return actualsModifier;
        }

        /// <summary>
        /// Creates an instance of ScorecardGoalCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardGoalCalculator CreateScorecardGoalCalculator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (goalCalculator == null)
            {
                goalCalculator = new ScorecardGoalCalculator(targetRepository, dailyActualRepository,
                    scorecardWorkdayPatternRepository, scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
            }
            return goalCalculator;
        }

        /// <summary>
        /// Create an instance of RollupManager and returns
        /// </summary>
        /// <returns></returns>
        protected virtual RollupManager CreateRollupManager()
        {
            if (rollupManager == null)
            {
                rollupManager = new RollupManager(actualsModifier, targetModifier, targetRepository,
                     dailyActualRepository, monthlyActualRepository, goalCalculator, recordableCalculator);
            }
            return rollupManager;
        }

        /// <summary>
        /// Creates an instance of RecordablesCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardRecordablesCalculator CreateRecordablesCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<Recordable> recordableRepository, IBaseRepository<User> userRepository)
        {
            if (recordableCalculator == null)
            {
                recordableCalculator = new ScorecardRecordablesCalculator(targetRepository, recordableRepository, userRepository);
            }
            return recordableCalculator;
        }


        /// <summary>
        /// Creates an instance of HolidayCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual HolidayCalculator CreateHolidayCalculator(IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository)
        {
            if (holidayCalculator == null)
            {
                holidayCalculator = new HolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository, scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
            }
            return holidayCalculator;
        }

        /// <summary>
        /// Creates the target modifier.
        /// </summary>
        /// <param name="dailyTargetRepository">The daily target repository.</param>
        /// <param name="monthlyTargetRepository">The monthly target repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <returns>The target Modifier</returns>
        protected virtual TargetModifier CreateTargetModifier(IBaseRepository<DailyTarget> dailyTargetRepository, IBaseRepository<MonthlyTarget> monthlyTargetRepository,
           IBaseRepository<User> userRepository, IBaseRepository<Metric> metricRepository, ScorecardGoalCalculator goalCalculator, HolidayCalculator holidayCalculator)
        {
            if (targetModifier == null)
            {
                targetModifier = new TargetModifier(dailyTargetRepository, monthlyTargetRepository, userRepository, metricRepository, goalCalculator, holidayCalculator);
            }

            return targetModifier;
        }


        /// <summary>
        /// Creates an instance of Target Validator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual TargetValidator CreateTargetValidator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<Scorecard> scorecardRepository,
            IBaseRepository<Metric> metricRepository)
        {
            if (targetValidator == null)
            {
                targetValidator = new TargetValidator(targetRepository, dailyActualRepository, monthlyActualRepository,
                    yearRepository, scorecardRepository, metricRepository);
            }
            return targetValidator;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target Repository</param>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        /// <param name="monthlyActualRepository">Monthly Actual Repository</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="recordableRepository">The recordable repository.</param>
        /// <param name="scorecardHolidayPatternRepository">The scorecard holiday pattern repository.</param>
        /// <param name="scorecardWorkdayPatternRepository">The scorecard workday pattern repository.</param>
        /// <param name="scorecardWorkdayTrackerRepository">The scorecard workday tracker repository.</param>
        public ActualsManager(IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<MonthlyTarget> monthlyTargetRepository,
            IBaseRepository<DailyTarget> dailyTargetRepository,
            IBaseRepository<Recordable> recordableRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<Metric> metricRepository,
            IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository,
            IBaseRepository<Scorecard> scorecardRepository)
        {
            if (targetRepository == null || dailyActualRepository == null ||
                monthlyActualRepository == null || scorecardHolidayPatternRepository == null ||
                scorecardWorkdayPatternRepository == null || scorecardWorkdayTrackerRepository == null ||
                userRepository == null || monthlyTargetRepository == null || recordableRepository == null || dailyTargetRepository == null
                || yearRepository == null || metricRepository == null || dailyTargetHistoryRepository == null || scorecardRepository== null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.userRepository = userRepository;
            this.recordableRepository = recordableRepository;
            this.monthlyTargetRepository = monthlyTargetRepository;
            this.dailyTargetRepository = dailyTargetRepository;
            this.yearRepository = yearRepository;
            this.metricRepository = metricRepository;
            this.dailyTargetHistoryRepository = dailyTargetHistoryRepository;
            this.scorecardWorkdayTrackerRepository = scorecardWorkdayTrackerRepository;
            this.scorecardRepository = scorecardRepository;
            this.actualsModifier = CreateActualsModifier(targetRepository, dailyActualRepository,
               monthlyActualRepository, userRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository, monthlyTargetRepository, dailyTargetRepository, dailyTargetHistoryRepository);
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
            this.recordableCalculator = CreateRecordablesCalculator(targetRepository,
                recordableRepository, userRepository);
            this.holidayCalculator = CreateHolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayTrackerRepository, scorecardWorkdayPatternRepository);
            this.targetModifier = CreateTargetModifier(dailyTargetRepository, monthlyTargetRepository, userRepository, metricRepository, goalCalculator, holidayCalculator);
            this.targetValidator = CreateTargetValidator(targetRepository, dailyActualRepository, monthlyActualRepository,
             yearRepository, scorecardRepository, metricRepository);
           
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Add daily or monthly actuals
        /// </summary>
        /// <param name="actualRequest">actual request</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>New actual entry's Id</returns>
        public int AddActual(ActualItem actualRequest, string userName)
        {
            // Round the actual and goal value to two decimal places
            ActualUtility.RoundActualItem(actualRequest);

            // Find target with the given id
            var target = targetRepository.Get(actualRequest.TargetId);
            if (target == null || !target.IsActive)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }

            int newActualId;
            bool updateRecordable = CheckIfUpdateRecordableRequired(target);

            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                ValidateDuplicateDailyActual(actualRequest.TargetId, actualRequest.Date);
                ValidateDailyActuals(target, actualRequest.Date);
                newActualId = actualsModifier.AddDailyActual(actualRequest,
                    target.Metric.GoalTypeId, target.Metric.DataTypeId, userName);
            }
            else
            {
                ValidateDuplicateMonthlyActual(actualRequest.TargetId, actualRequest.Date);
                ValidateMonthlyActuals(target, actualRequest.Date);
                newActualId = actualsModifier.AddMonthlyActual(actualRequest,
                    target.Metric.GoalTypeId, userName);
            }

            // Check if roll up required
            if (CheckIfActualOrTargetRollupRequired(target))
            {
                PerformRollup(target, actualRequest, updateRecordable, userName);
            }

            if (CheckIfUpdateRequiredForExistingActual(actualRequest.Date, target))
            {
                UpdateExistingActual(target, actualRequest, false, userName);
            }

            if (updateRecordable)
            {
                recordableCalculator.TrackRecordables(target.Id, actualRequest, userName);
            }

            return newActualId;
        }

        /// <summary>
        /// Update daily or monthly actuals
        /// </summary>
        /// <param name="actualRequest">actual request</param>
        /// <param name="userName">logged in user name</param>
        public void UpdateActual(ActualItem actualRequest, string userName)
        {
            // Round the actual and goal value to two decimal places
            ActualUtility.RoundActualItem(actualRequest);

            // Find target with the given id
            var target = targetRepository.Get(actualRequest.TargetId);

            bool updateRecordable = CheckIfUpdateRecordableRequired(target);

            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                ValidateDailyActuals(target, actualRequest.Date);
                actualsModifier.UpdateDailyActual(actualRequest, target.Metric.GoalTypeId,
                    target.Metric.DataTypeId, userName);
            }
            else
            {
                ValidateMonthlyActuals(target, actualRequest.Date);
                actualsModifier.UpdateMonthlyActual(actualRequest, target.Metric.GoalTypeId,
                    userName);
            }

            // Check if roll up required
            if (CheckIfActualOrTargetRollupRequired(target))
            {
                PerformRollup(target, actualRequest, updateRecordable, userName);
            }

            if (CheckIfUpdateRequiredForExistingActual(actualRequest.Date, target))
            {
                UpdateExistingActual(target, actualRequest, false, userName);
            }

            if (updateRecordable)
            {
                recordableCalculator.TrackRecordables(target.Id, actualRequest, userName);
            }
        }

        /// <summary>
        /// Marks a day as holiday or workday in the system
        /// </summary>
        /// <param name="targetId">Target Id corresponding</param>
        /// <param name="date">Date which needs to be marked as holiday</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>New actual entry's Id</returns>
        public int MarkHolidayOrWorkday(int targetId, DateTime date, bool isWorkday, string userName)
        {
            int trackerId = 0;
            // Find target with the given id
            var target = targetRepository.Get(targetId);
            ValidateHolidayEntryDates(target, date, isWorkday);

            //get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
            x => x.AccountName == userName).Id;

            var existingScorecardDayTracker = scorecardWorkdayTrackerRepository.GetAll()
                                               .Where(x => x.ScorecardId == target.ScorecardId && x.Date == date &&
                                               x.IsActive).FirstOrDefault();

            // if entry for same date exists set it to inactive
            if (existingScorecardDayTracker != null)
            {
                ClearMarkedDaysForDate(loggedInUserId, date, existingScorecardDayTracker);
            }

            AddWorkdayMarking(date, target.ScorecardId, isWorkday, loggedInUserId);

            // Find all non-cascaded daily targets mapped to the scorecard
            // && !x.IsCascaded  Checking Removed
            var scorecardTargets = targetRepository.GetAll().Where(x => x.ScorecardId == target.ScorecardId
                                                            && x.CalendarYearId == target.CalendarYearId
                                                            && date >= x.EffectiveStartDate && date <= x.EffectiveEndDate
                                                            && x.IsActive
                                                            && x.TrackingMethodId == Constants.TrackingMethodDaily).ToList();
            foreach (var scorecardTarget in scorecardTargets)
            {
               
                bool hasMonthlyAndDailyTargets = targetValidator.CheckIfMetricHasMonthlyAndDailyTargetValues(scorecardTarget);
                if (!hasMonthlyAndDailyTargets)
                {
                    if (scorecardTarget.TargetEntryMethodId == Constants.TargetEntryMethodDaily)
                    {
                        //Update the targets for daily target entry method, ie, Daily Rate
                        UpdateDailyRate(scorecardTarget.Id, isWorkday, date, loggedInUserId);
                    }
                    else
                    {
                        //Update the targets for monthly target entry method
                        UpdateExistingDailyTargets(scorecardTarget, isWorkday, date, loggedInUserId);

                    }
                }
                else
                {
                    var monthlyTarget = monthlyTargetRepository.GetAll().FirstOrDefault(x => x.TargetId == targetId && x.Month == date.Month);
                    if (monthlyTarget != null)
                    {
                        if (monthlyTarget.MaxGoalValue.HasValue)
                        {
                            //Update the targets for monthly target entry method
                            UpdateExistingDailyTargets(scorecardTarget, isWorkday, date, loggedInUserId);
                        }
                        else if (monthlyTarget.DailyRate.HasValue)
                        {
                            //Update the targets for daily target entry method, ie, Daily Rate
                            UpdateDailyRate(scorecardTarget.Id, isWorkday, date, loggedInUserId);
                        }
                    }

                }

                // if cascaded child roll up the targets
                if (CheckIfActualOrTargetRollupRequired(scorecardTarget))
                {
                    PerformRollupTarget(scorecardTarget, date, userName, true);
                }

                // update actual after updating the targets
                if (CheckIfActualExists(date, scorecardTarget))
                {
                    var actualRequest = new ActualItem()
                    {
                        Date = date,
                        TargetId = scorecardTarget.Id,
                        ActualValue = null,
                        GoalValue = null
                    };

                    UpdateExistingActual(scorecardTarget, actualRequest, true, userName);
                }

                
            }

            return trackerId;
        }

        /// <summary>
        /// Populates the daily target if not already exist or target found on holiday.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="userName">Name of the user.</param>
        public void PopulateDailyTargets(Target target, int month, string userName)
        {
            bool isTargetUpdated = false;
            bool isFullTargetRollUp = false;
            int yearId = target.CalendarYearId;
            Year year = yearRepository.Get(yearId);
            int targetYear = year.EndDate.Year;

            DateTime previousMonthDate = TimeZoneUtility.GetCurrentTimestamp().AddMonths(-1);
            DateTime previousMonthStartDate = new DateTime(previousMonthDate.Year, previousMonthDate.Month, 1);
            DateTime requestedDate = new DateTime(targetYear, month, 1);
            // only previous, current and future month targets can be distributed
            bool isTargetUpdateAllowed = requestedDate.Date >= previousMonthStartDate.Date;

            // get all target update months without daily targets
            var monthlyTargetsForUpdate = target?.MonthlyTargets?.Where(x => x.Month >= previousMonthDate.Month && ((x.DailyTargets?.Count ?? 0) <= 0));

            var monthlyTarget = target?.MonthlyTargets?.FirstOrDefault(x => x.Month == month);
            if ((monthlyTarget == null  && monthlyTargetsForUpdate == null) || !isTargetUpdateAllowed)
            {
                return;
            }            
            
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
            x => x.AccountName == userName)?.Id ?? 0;
            

            // if daily target not exists during actual entry,
            // populate the targets
            if (monthlyTargetsForUpdate != null && monthlyTargetsForUpdate.Any())
            {
                AddDailyTargets(target, monthlyTargetsForUpdate, loggedInUserId);
                isTargetUpdated = true;
                isFullTargetRollUp = true;
            }
            else if (monthlyTarget != null && targetModifier.CheckIfTargetExistsForHolidays(monthlyTarget, target.ScorecardId, targetYear))
            {
                // if monthly target entry is used and target exists for holidays recalculate the goal 
                if (monthlyTarget.MaxGoalValue.HasValue)
                {
                    RecalculateDailyTargets(target, requestedDate, loggedInUserId);
                }
                // if daily target entry is used and target exists for holidays remove the entries
                else if (monthlyTarget.DailyRate.HasValue)
                {
                    var holidayTargets = monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue.HasValue && 
                                         holidayCalculator.CheckIfDateIsaHoliday(target.ScorecardId, new DateTime(targetYear, monthlyTarget.Month, x.Day))).ToList();
                    foreach(var holidayTarget in holidayTargets)
                    {
                        DateTime selectedDate = new DateTime(targetYear, monthlyTarget.Month, holidayTarget.Day);
                        UpdateDailyRate(target.Id, false, selectedDate, loggedInUserId);
                    }
                }
                isTargetUpdated = true;
            }

            bool isActualExists = CheckIfActualExists(requestedDate, target);
            if (isActualExists && isTargetUpdated)
            {
                actualsModifier.UpdateDailyActualStatusAndGoalForMonth(target.Id, requestedDate, userName);
                if(isFullTargetRollUp)
                {
                    // update status for previous month
                    actualsModifier.UpdateDailyActualStatusAndGoalForMonth(target.Id, previousMonthDate, userName);
                }
            }

            if (CheckIfActualOrTargetRollupRequired(target) && isTargetUpdated)
            {
                PerformRollupTarget(target, requestedDate, userName, isActualExists, isFullTargetRollUp);
            }                     
        }       

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (dailyActualRepository != null)
                    {
                        dailyActualRepository.Dispose();
                    }
                    if (monthlyActualRepository != null)
                    {
                        monthlyActualRepository.Dispose();
                    }
                    if (targetRepository != null)
                    {
                        targetRepository.Dispose();
                    }
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    if (monthlyTargetRepository == null)
                    {
                        monthlyTargetRepository.Dispose();
                    }
                    if (dailyTargetRepository == null)
                    {
                        dailyTargetRepository.Dispose();
                    }
                    if (yearRepository == null)
                    {
                        yearRepository.Dispose();
                    }
                    if (metricRepository == null)
                    {
                        metricRepository.Dispose();
                    }
                    if (scorecardWorkdayTrackerRepository != null)
                    {
                        scorecardWorkdayTrackerRepository.Dispose();
                    }
                    if (dailyTargetHistoryRepository == null)
                    {
                        dailyTargetHistoryRepository.Dispose();
                    }
                    if (scorecardRepository != null)
                    {
                        scorecardRepository.Dispose();
                    }
                    // Assign references to null
                    dailyActualRepository = null;
                    monthlyActualRepository = null;
                    targetRepository = null;
                    userRepository = null;
                    monthlyTargetRepository = null;
                    dailyTargetRepository = null;
                    yearRepository = null;
                    metricRepository = null;
                    scorecardWorkdayTrackerRepository = null;
                    dailyTargetHistoryRepository = null;
                    scorecardRepository = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
