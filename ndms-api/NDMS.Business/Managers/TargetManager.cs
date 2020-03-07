using NDMS.Business.Common;
using NDMS.Business.Converters;
using NDMS.Business.Interfaces;
using NDMS.Business.Rollup;
using NDMS.Business.Validators;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Managers
{
    public class TargetManager : ITargetManager
    {
        #region Field(s)
        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Scorecard Repository
        /// </summary>
        private IBaseRepository<Scorecard> scorecardRepository;

        /// <summary>
        /// Metric Mapping Repository
        /// </summary>
        private IBaseRepository<MetricMapping> metricMappingRepository;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;

        /// <summary>
        /// Roll-up Method Repository
        /// </summary>
        private IBaseRepository<RollupMethod> rollupMethodRepository;

        /// <summary>
        /// Tracking Method Repository
        /// </summary>
        private IBaseRepository<TrackingMethod> trackingMethodRepository;

        /// <summary>
        /// Target Entry Method Repository
        /// </summary>
        private IBaseRepository<TargetEntryMethod> targetEntryMethodRepository;

        /// <summary>
        /// Graph Plotting Method Repository
        /// </summary>
        private IBaseRepository<GraphPlottingMethod> graphPlottingMethodRepository;

        /// <summary>
        /// Metric Repository
        /// </summary>
        private IBaseRepository<Metric> metricRepository;

        /// <summary>
        /// Actuals Repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly Actuals Repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Monthly Target Repository
        /// </summary>
        private IBaseRepository<MonthlyTarget> monthlyTargetRepository;

        /// <summary>
        /// The daily target repository
        /// </summary>
        private IBaseRepository<DailyTarget> dailyTargetRepository;

        /// <summary>
        /// The daily target history repository
        /// </summary>
        private IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository;

        /// <summary>
        /// The monthly target history repository
        /// </summary>
        private IBaseRepository<MonthlyTargetHistory> monthlyTargetHistoryRepository;

        /// <summary>
        /// User Repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Reference to target validator
        /// </summary>
        private TargetValidator targetValidator;

        /// <summary>
        /// Reference to holiday calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        /// <summary>
        /// The target modifier
        /// </summary>
        private TargetModifier targetModifier;

        /// <summary>
        /// The goal calculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Rollup manager instance
        /// </summary>
        private RollupManager rollupManager;

        /// <summary>
        /// Reference to actuals modifier
        /// </summary>
        private ActualsModifier actualsModifier;

        /// <summary>
        /// Reference to Recordable Calculator
        /// </summary>
        private ScorecardRecordablesCalculator recordableCalculator;

        ///<summary>
        /// Reference to workday Calculator
        /// </summary>
        private WorkdayCalculator workdayPatternCalculator;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Retrieves the maximum allowed monthly goal for a new/existing target which is 
        /// going to be/being cascaded
        /// </summary>
        /// <param name="parentTargetId">Parent target</param>
        /// <param name="childTargetId">Child target Id(if child target is already created)</param>
        /// <param name="selectedRollUpMethodId">Selected rollup method id</param>
        /// <returns>Maximum allowed monthly goals</returns>
        private List<MonthAndTarget> GetMaximumAllowedMonthlyGoals(Target parentTarget,
            int? childTargetId, int? selectedRollUpMethodId, int? selectedTargetEntryMethodId)
        {
            int metricDataTypeId = parentTarget.Metric.DataTypeId;
            Year calendarYear = yearRepository.Get(parentTarget.CalendarYearId);
            var monthItems = CalendarUtility.GetMonthsBetweenDates(calendarYear.StartDate,
                calendarYear.EndDate).ToList();
            var currentMonth = TimeZoneUtility.GetCurrentTimestamp().Month;
            var childMonthlyTargets = new List<MonthlyTarget>();
            MonthlyTarget childMonthlyTarget = null;

            if (childTargetId.HasValue)
            {
                childMonthlyTargets = targetRepository.Get(childTargetId.Value)?.MonthlyTargets.ToList();
            }

            var maximumAllowedGoalValues = new List<MonthAndTarget>();

            //Initially assign parent's monthly target in unallocatedTargets list
            foreach (var monthlyTarget in parentTarget.MonthlyTargets)
            {
                int? currentMonthTargetEntryMethodId = selectedTargetEntryMethodId;
                if (monthlyTarget.Month <= currentMonth)
                {
                    childMonthlyTarget = childMonthlyTargets.FirstOrDefault(x => x.Month == monthlyTarget.Month && (x.DailyRate.HasValue || x.MaxGoalValue.HasValue));
                    if (childMonthlyTarget != null)
                    {
                        // if child has max goal value then the child's target entry is monthly for the selected month
                        if (childMonthlyTarget.MaxGoalValue.HasValue)
                        {
                            currentMonthTargetEntryMethodId = Constants.TargetEntryMethodMonthly;
                        }
                        // if child has daily rate value then the child's target entry is daily for the selected month
                        else if (childMonthlyTarget.DailyRate.HasValue)
                        {
                            currentMonthTargetEntryMethodId = Constants.TargetEntryMethodDaily;
                        }
                    }
                }

                maximumAllowedGoalValues.Add(new MonthAndTarget()
                {
                    Month = monthItems.FirstOrDefault(x => x.Id == monthlyTarget.Month),
                    Value = GetGoalValueForMonthByTargetEntryMethod(parentTarget, monthlyTarget, currentMonthTargetEntryMethodId),
                    TargetEntryMethodId = currentMonthTargetEntryMethodId
                });
            }

            // if the roll up method is Same As Child or Avg of Children
            // no need to consider sibling targets. In such cases, maximum allowed is same as
            // parent
            if (selectedRollUpMethodId != Constants.RollupMethodSameAsChild &&
                selectedRollUpMethodId != Constants.RollupMethodAverageOfChildren)
            {
                // Get child scorecards having cascade from parent enabled excluding 
                // the selected child scorecard
                var siblingTargets = targetRepository.GetAll().Where(x =>
                    x.ParentTargetId == parentTarget.Id && x.CascadeFromParent && x.IsActive &&
                    (childTargetId.HasValue ? x.Id != childTargetId.Value : x.Id == x.Id)).
                        ToList();

                // Iterating through all sibling targets 
                foreach (var siblingTarget in siblingTargets)
                {
                    //iterating through child scorecard's monthly targets
                    foreach (var monthlyTarget in siblingTarget.MonthlyTargets)
                    {
                        var unallocatedTarget = maximumAllowedGoalValues.Where(x =>
                            x.Month.Id == monthlyTarget.Month).FirstOrDefault();
                        if (unallocatedTarget != null && (monthlyTarget.MaxGoalValue.HasValue || monthlyTarget.DailyRate.HasValue))
                        {
                            //subtract the child goal value from unallocated Target month list
                            unallocatedTarget.Value -= GetGoalValueForMonthByTargetEntryMethod(siblingTarget, monthlyTarget, unallocatedTarget.TargetEntryMethodId);
                        }
                    }
                }
            }

            // Fill the remaining months with empty goal values
            foreach (var monthItem in monthItems)
            {
                if (!maximumAllowedGoalValues.Any(x => x.Month.Id == monthItem.Id))
                {
                    maximumAllowedGoalValues.Add(new MonthAndTarget()
                    {
                        Month = monthItem
                    });
                }
            }
            return maximumAllowedGoalValues.OrderBy(x => x.Month.Year).
                ThenBy(y => y.Month.Id).ToList();
        }

        private decimal? GetGoalValueForMonthByTargetEntryMethod(Target target, MonthlyTarget monthlyTarget, int? targetEntryMethod)
        {
            int year = target.CalendarYear.StartDate.Year;
            DateTime monthStartDate = new DateTime(year, monthlyTarget.Month, 1);
            DateTime monthEndDate = new DateTime(year, monthlyTarget.Month, DateTime.DaysInMonth(year, monthlyTarget.Month));
            if (monthStartDate < target.EffectiveStartDate)
            {
                monthStartDate = target.EffectiveStartDate;
            }

            if (monthEndDate > target.EffectiveEndDate)
            {
                monthEndDate = target.EffectiveEndDate;
            }

            int totalDaysInMonth = (int)(monthEndDate - monthStartDate).TotalDays + 1; ;

            int numberOfHolidaysInMonth = holidayCalculator.CountHolidaysBetweenDaysOfMonth(target.Id, target.ScorecardId, monthStartDate, monthEndDate);

            int totalWorkdaysInmonth = totalDaysInMonth - numberOfHolidaysInMonth;

            switch (targetEntryMethod)
            {
                case Constants.TargetEntryMethodDaily:
                    // get daily rate from monthly goal
                    if (monthlyTarget.MaxGoalValue.HasValue)
                    {
                        decimal? dailyRate = null;
                        if (totalWorkdaysInmonth > 0)
                        {
                            dailyRate = Math.Round((monthlyTarget.MaxGoalValue.Value / totalWorkdaysInmonth), 2, MidpointRounding.AwayFromZero);
                        }
                        return dailyRate;
                    }
                    else
                    {
                        return monthlyTarget.DailyRate;
                    }
                case Constants.TargetEntryMethodMonthly:
                    // get monthly goal from daily rate
                    if (monthlyTarget.DailyRate.HasValue)
                    {
                        decimal? totalMonthlyGoal = null;
                        if (monthlyTarget.DailyTargets != null)
                        {
                            totalMonthlyGoal = monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue != null).Sum(x => x.MaxGoalValue);
                        }
                        else
                        {
                            totalMonthlyGoal = monthlyTarget.DailyRate.Value * totalWorkdaysInmonth;
                        }

                        if (totalMonthlyGoal.HasValue)
                        {
                            totalMonthlyGoal = Math.Round(totalMonthlyGoal.Value, 2, MidpointRounding.AwayFromZero);
                        }
                        return totalMonthlyGoal;

                    }
                    else
                    {
                        return monthlyTarget.MaxGoalValue;
                    }
                default:
                    return monthlyTarget.MaxGoalValue.HasValue ?
                        monthlyTarget.MaxGoalValue :
                        monthlyTarget.DailyRate;
            }
        }

        /// <summary>
        /// Method to retrieve all active roll up methods
        /// </summary>
        /// <returns>list of roll up methods</returns>
        private List<RollupMethodItem> GetRollupMethods()
        {
            return rollupMethodRepository.GetAll().Where(r => r.IsActive)
                                                .Select(r => new RollupMethodItem()
                                                {
                                                    Id = r.Id,
                                                    Name = r.Name
                                                }).ToList();
        }

        /// <summary>
        /// Method to retrieve all tracking methods
        /// </summary>
        /// <returns>List of tracking methods</returns>
        private List<TrackingMethodItem> GetTrackingMethods()
        {
            return trackingMethodRepository.GetAll().Select(r => new TrackingMethodItem()
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }

        /// <summary>
        /// Method to retrieve all Target entry methods
        /// </summary>
        /// <returns>List of target entry methods</returns>
        private List<TargetEntryMethodItem> GetTargetEntryMethods()
        {
            return targetEntryMethodRepository.GetAll().Select(r => new TargetEntryMethodItem()
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }

        /// <summary>
        /// Method to retrieve all graph plotting methods
        /// </summary>
        /// <returns>List of graph plotting methods</returns>
        private List<GraphPlottingMethodItem> GetGraphPlottingMethods()
        {
            return graphPlottingMethodRepository.GetAll().Select(r => new GraphPlottingMethodItem()
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }

        private List<MtdPerformanceTrackingMethodItem> GetMtdPerformanceTrackingMethods()
        {
            var mtdTrackingMethods = Enum.GetValues(typeof(MTDPerformanceTrackingMethod)).Cast<MTDPerformanceTrackingMethod>().ToList();
            var mtdTrackingMethodItems = new List<MtdPerformanceTrackingMethodItem>();
            mtdTrackingMethods.ForEach(x => mtdTrackingMethodItems.Add(new MtdPerformanceTrackingMethodItem()
            {
                Id = (int)x,
                Name = x.ToString()
            }));
            return mtdTrackingMethodItems;
        }

        private List<CascadedMetricsTrackingMethodItem> GetCascadedMetricsTrackingMethods()
        {
            var cascadedMetricsTrackingMethods = Enum.GetValues(typeof(CascadedMetricsTrackingMethod)).Cast<CascadedMetricsTrackingMethod>().ToList();
            var cascadedMetricsTrackingMethodItems = new List<CascadedMetricsTrackingMethodItem>();
            cascadedMetricsTrackingMethods.ForEach(x => cascadedMetricsTrackingMethodItems.Add(new CascadedMetricsTrackingMethodItem()
            {
                Id = (int)x,
                Name = EnumUtility.GetDescription(x)
            }));
            return cascadedMetricsTrackingMethodItems;
        }

        /// <summary>
        /// Method to retrieve years for which target can be set or managed
        /// </summary>
        /// <returns>list of years</returns>
        private List<YearItem> GetTargetYears()
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            return yearRepository.GetAll().ToList().Where(y =>
                y.StartDate.Year >= currentDate.Year - 1
                && y.StartDate.Year <= currentDate.Year + 1)
                                      .Select(y => new YearItem()
                                      {
                                          Id = y.Id,
                                          Name = y.Name,
                                          StartDate = y.StartDate,
                                          EndDate = y.EndDate
                                      }).ToList();
        }

        /// <summary>
        /// Get existing dailyTargets of the monthly target and fill other days with relevent info
        /// </summary>
        /// <param name="monthlyTargetId"></param>
        /// <returns></returns>
        private List<DailyTargetItem> GetFullMonthDailyTargets(int monthlyTargetId)
        {
            //get monthly target item
            var monthlyTarget = monthlyTargetRepository.Get(monthlyTargetId);

            if (monthlyTarget != null)
            {

                // Convert all daily targets to DTO's
                var dailyTargetItems = monthlyTarget.DailyTargets.Select
                    (d => TargetConverters.ConvertDailyTargetToDailyTargetItemDTO(d)).ToList();

                // Retrieve all days in a month
                var calendarYear = yearRepository.Get(monthlyTarget.Target.CalendarYearId);
                int targetYear = CalendarUtility.GetYearOfTheMonth(calendarYear,
                    monthlyTarget.Month);

                // Retrieve all days in a month
                var daysList = CalendarUtility.GetAllDaysInMonth(targetYear, monthlyTarget.Month);

                // Fill the remaining days with empty values
                foreach (var day in daysList)
                {

                    var existingday = dailyTargetItems.FirstOrDefault(x => x.Day == day);
                    if (existingday == null)
                    {
                        dailyTargetItems.Add(new DailyTargetItem()
                        {
                            Day = day,
                            IsHoliday = holidayCalculator.CheckIfDateIsaHoliday(monthlyTarget.Target.ScorecardId, day, monthlyTarget.Month, targetYear),
                            IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(monthlyTarget.Target.EffectiveStartDate, monthlyTarget.Target.EffectiveEndDate,
                            new DateTime(targetYear, monthlyTarget.Month, day))
                        });
                    }
                    else
                    {
                        existingday.IsHoliday = holidayCalculator.CheckIfDateIsaHoliday(monthlyTarget.Target.ScorecardId, day, monthlyTarget.Month, targetYear);
                        existingday.IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(monthlyTarget.Target.EffectiveStartDate, monthlyTarget.Target.EffectiveEndDate,
                            new DateTime(targetYear, monthlyTarget.Month, day));

                    }

                }
                return dailyTargetItems.OrderBy(x => x.Day).ToList();
            }

            return null;
        }

        /// <summary>
        /// Update parent target's Is Cascaded and Roll-up method while managing targets 
        /// for child scorecards
        /// </summary>
        /// <param name="targetItem">target id</param>
        /// <param name="loggedInUserId">logged in id</param>
        /// <returns>Id of Parent Target</returns>
        private int? UpdateParentTarget(TargetItem targetItem, int loggedInUserId)
        {
            int? parentTargetId = null;
            // If metric is going to be cascaded from parent, we need to set IsCascaded true 
            // in parent record
            int? parentScorecardId = scorecardRepository.Get(targetItem.ScorecardId).
                ParentScorecardId;
            if (parentScorecardId != null)
            {
                var parentTarget = targetRepository.GetAll().Where(x =>
                    x.ScorecardId == parentScorecardId &&
                    x.KPIId == targetItem.KPIId &&
                    x.MetricId == targetItem.MetricId &&
                    x.CalendarYearId == targetItem.CalendarYearId &&
                    x.EffectiveStartDate <= targetItem.EffectiveStartDate &&
                    x.EffectiveStartDate <= targetItem.EffectiveEndDate &&
                    x.EffectiveEndDate >= targetItem.EffectiveEndDate &&
                    x.EffectiveEndDate >= targetItem.EffectiveStartDate &&
                    x.IsActive).FirstOrDefault();
                parentTargetId = parentTarget.Id;
                parentTarget.IsCascaded = true;
                parentTarget.RollUpMethodId = targetItem.RollupMethodId;
                if (!parentTarget.CascadedMetricsTrackingMethodId.HasValue)
                {   // set the default value
                    parentTarget.CascadedMetricsTrackingMethodId = (int)CascadedMetricsTrackingMethod.RolledUpTargets;
                    var currentMonth = TimeZoneUtility.GetCurrentTimestamp().Month;
                    parentTarget.MonthlyTargets.Where(x => x.Month >= currentMonth).ToList().ForEach(x =>
                    {
                        x.IsRolledUpGoal = true;
                    });
                }

                parentTarget.LastModifiedBy = loggedInUserId;
                parentTarget.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                parentTarget.TargetHistory.Add(TargetConverters.
                    ConvertTargetToTargetHistory(parentTarget));
            }
            return parentTargetId;
        }

        /// <summary>
        /// Assign system generated targets to all possible combination of Tracking and Target Entry Method
        /// on adding new targets
        /// </summary>
        /// <param name="targetItem"></param>
        private void GenerateMonthlyAndDailyTargets(TargetItem targetItem)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var existingTarget = targetItem.Id.HasValue ? targetRepository.Get(targetItem.Id.Value) : null;

            if (targetItem.TrackingMethodId == Constants.TrackingMethodMonthly)
            {
                //Only Monthly Targets is set, no dailyTarget for monthly tracking monthly target entry
                targetItem.MonthlyTargets.ForEach(monthlyTarget =>
                {

                    if (!monthlyTarget.Id.HasValue && monthlyTarget.GoalValue != null)
                    {
                        monthlyTarget.DailyRateValue = null;
                        monthlyTarget.DailyTargets = null;
                    }
                });
            }
            else if (targetItem.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                var generateDailyTargetsRequestItem = new GenerateDailyTargetsRequest
                {
                    ScorecardId = targetItem.ScorecardId,
                    YearId = targetItem.CalendarYearId.Value,
                    MetricId = targetItem.MetricId.Value,
                    EffectiveStartDate = targetItem.EffectiveStartDate,
                    EffectiveEndDate = targetItem.EffectiveEndDate
                };
                // if target entry method is changing 
                if (targetItem.Id.HasValue && existingTarget.TargetEntryMethodId.Value != targetItem.TargetEntryMethodId.Value)
                {
                    generateDailyTargetsRequestItem.TargetEntryMethodId = existingTarget.TargetEntryMethodId.Value;
                    var currentAndPrevMonthItems = targetItem.MonthlyTargets.Where(x => x.Month.Id == currentDate.Month || x.Month.Id == currentDate.Month - 1).ToList();
                    currentAndPrevMonthItems.ForEach(monthlyTarget =>
                    {
                        generateDailyTargetsRequestItem.MonthId = monthlyTarget.Month.Id;
                        generateDailyTargetsRequestItem.MonthlyGoalValue = existingTarget.TargetEntryMethodId.Value == Constants.TargetEntryMethodMonthly ? monthlyTarget.GoalValue : null;
                        generateDailyTargetsRequestItem.DailyRateValue = existingTarget.TargetEntryMethodId.Value == Constants.TargetEntryMethodDaily ? monthlyTarget.DailyRateValue : null;

                        if (monthlyTarget.DailyTargets == null || monthlyTarget.DailyTargets.Count() == 0)
                        {
                            var existingMonthTarget = existingTarget.MonthlyTargets?.FirstOrDefault(x => x.Month == monthlyTarget.Month.Id);

                            if (existingMonthTarget != null)
                            {
                                if (existingMonthTarget.DailyTargets?.Count() > 0)
                                {
                                    monthlyTarget.DailyTargets = GetFullMonthDailyTargets(existingMonthTarget.Id);

                                }
                                else
                                {
                                    monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem);
                                }
                            }
                            else
                            {
                                monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem);
                            }
                            monthlyTarget.DailyTargets.Where(x => x.GoalValue != null).ToList().ForEach(x =>
                            {
                                x.IsManual = true;
                            });

                        }
                    });

                }

                generateDailyTargetsRequestItem.TargetEntryMethodId = targetItem.TargetEntryMethodId.Value;
                targetItem.MonthlyTargets.ForEach(monthlyTarget =>
                {
                    if (monthlyTarget.Month.Year == currentDate.Year && monthlyTarget.Month.Id >= currentDate.Month - 1)
                    {
                        generateDailyTargetsRequestItem.MonthId = monthlyTarget.Month.Id;
                        generateDailyTargetsRequestItem.DailyRateValue = targetItem.TargetEntryMethodId.Value == Constants.TargetEntryMethodDaily ? monthlyTarget.DailyRateValue : null;
                        generateDailyTargetsRequestItem.MonthlyGoalValue = targetItem.TargetEntryMethodId.Value == Constants.TargetEntryMethodMonthly ? monthlyTarget.GoalValue : null;

                        if (monthlyTarget.Id.HasValue)
                        {
                            //not altered if updated target item contains any daily target
                            if (monthlyTarget.DailyTargets?.Count() == 0 || monthlyTarget.DailyTargets == null)
                            {
                                if (targetModifier.CheckIfSelectedMonthNeedsUpdation(targetItem, existingTarget, monthlyTarget))
                                {
                                    monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem).ToList();
                                }
                                else
                                {
                                    var existingMonthlyTarget = monthlyTargetRepository.GetAll().FirstOrDefault(x => x.Id == monthlyTarget.Id.Value);
                                    // check if already existing monthly target contains any daily target
                                    if (existingMonthlyTarget.DailyTargets?.Count() == 0 || existingMonthlyTarget.DailyTargets == null)
                                    {
                                        monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem).ToList();
                                    }
                                    else if ((existingMonthlyTarget.DailyRate != monthlyTarget.DailyRateValue || (existingMonthlyTarget.MaxGoalValue != monthlyTarget.GoalValue) && monthlyTarget.IsManualTarget))
                                    {
                                        monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem);
                                    }
                                }
                            }
                            else
                            {
                                monthlyTarget.DailyTargets.ToList().ForEach(x =>
                                {
                                    x.IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(targetItem.EffectiveStartDate,
                                        targetItem.EffectiveEndDate, new DateTime(monthlyTarget.Month.Year, monthlyTarget.Month.Id, x.Day));
                                    if (x.IsManual && targetItem.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative && !x.GoalValue.HasValue)
                                    {
                                        x.GoalValue = 0;
                                    }
                                });
                            }

                        }
                        //Daily Rate as daily targets for daily target entry
                        //where daily targets are not assigned explicitly
                        else if (monthlyTarget.DailyTargets?.Count() == 0 || monthlyTarget.DailyTargets == null)
                        {
                            monthlyTarget.DailyTargets = GenerateDailyTargetsList(generateDailyTargetsRequestItem).ToList();
                        }
                    }
                });
            }

        }

        /// <summary>
        /// Updates monthly and daily targets for a target
        /// </summary>
        /// <param name="targetItem">Target values to update</param>
        /// <param name="targetToUpdate">Existing target entity to update</param>
        /// <param name="loggedInUserId">Logged in user id</param>
        /// <param name="metricType">Metric type</param>
        private void UpdateMonthlyAndDailyTargets(TargetItem targetItem, Target targetToUpdate,
            int loggedInUserId, MetricType metricType, ref int updatedTargetCount)
        {
            //looping through monthly targets for add/edit monthly targets
            foreach (var monthlyTargetItem in targetItem.MonthlyTargets)
            {
                //updating an already existing monthly target
                if (monthlyTargetItem.Id.HasValue)
                {
                    //fetching existing monthly target object
                    var existingMonthlyTarget = targetToUpdate.MonthlyTargets.Where(x =>
                            x.Id == monthlyTargetItem.Id.Value).FirstOrDefault();

                    //check if any changes to goal/stretch value, then only update is done
                    if (existingMonthlyTarget.MaxGoalValue != monthlyTargetItem.GoalValue ||
                        existingMonthlyTarget.StretchGoalValue != monthlyTargetItem.StretchGoalValue ||
                        existingMonthlyTarget.DailyRate != monthlyTargetItem.DailyRateValue)
                    {
                        existingMonthlyTarget.MaxGoalValue = monthlyTargetItem.GoalValue;
                        existingMonthlyTarget.DailyRate = monthlyTargetItem.DailyRateValue;
                        existingMonthlyTarget.StretchGoalValue = monthlyTargetItem.StretchGoalValue;
                        existingMonthlyTarget.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                        existingMonthlyTarget.LastModifiedBy = loggedInUserId;

                        var monthlyTargetHistory = TargetConverters.
                                 ConvertMonthlyTargetToMonthlyTargetHistory(existingMonthlyTarget);
                        monthlyTargetHistory.TargetId = targetItem.Id.Value;
                        existingMonthlyTarget.MonthlyTargetHistory.Add(monthlyTargetHistory);
                        updatedTargetCount++;
                    }
                    //Clear Obsoleted monthlyTargets

                    if (monthlyTargetItem.Month.Id < targetItem.EffectiveStartDate.Month || monthlyTargetItem.Month.Id > targetItem.EffectiveEndDate.Month)
                    {
                        if (existingMonthlyTarget != null)
                        {
                            updatedTargetCount++;
                            ClearObseletedMonthlyTargets(existingMonthlyTarget, loggedInUserId);
                        }
                    }
                    if (monthlyTargetItem.DailyRateValue == null && monthlyTargetItem.GoalValue == null)
                    {
                        if (existingMonthlyTarget != null)
                        {
                            updatedTargetCount++;
                            ClearObseletedMonthlyTargets(existingMonthlyTarget, loggedInUserId);
                        }
                    }
                    if (targetItem.TrackingMethodId == Constants.TrackingMethodMonthly)
                    {
                        if (existingMonthlyTarget?.DailyTargets != null)
                        {
                            updatedTargetCount++;
                            ClearObseleteDailyTargetofMonthlyTargets(existingMonthlyTarget, loggedInUserId);
                        }
                    }

                    if (monthlyTargetItem.DailyTargets != null)
                    {
                        UpdateDailyTargets(monthlyTargetItem, existingMonthlyTarget,
                            loggedInUserId, ref updatedTargetCount);
                    }
                }
                //adding new monthly and daily targets
                else
                {
                    if (monthlyTargetItem.GoalValue != null || monthlyTargetItem.DailyRateValue != null)
                    {
                        MonthlyTarget mthlyTarget = TargetConverters.
                        ConvertMonthlyTargetItemToMonthlyTargetEntity(monthlyTargetItem,
                        loggedInUserId);

                        var monthlyTargetHistory = TargetConverters.
                                        ConvertMonthlyTargetToMonthlyTargetHistory(mthlyTarget);
                        monthlyTargetHistory.TargetId = targetItem.Id.Value;

                        mthlyTarget.MonthlyTargetHistory = new List<MonthlyTargetHistory>(){
                            monthlyTargetHistory
                        };

                        updatedTargetCount++;

                        if (monthlyTargetItem.DailyTargets != null)
                        {
                            mthlyTarget.DailyTargets = new List<DailyTarget>();
                            foreach (var dailyTarget in monthlyTargetItem.DailyTargets)
                            {
                                if (dailyTarget.GoalValue != null)
                                {
                                    DailyTarget daily = TargetConverters.
                                        ConvertDailyTargetItemToDailyTargetEntity(dailyTarget,
                                        mthlyTarget, loggedInUserId);
                                    mthlyTarget.DailyTargets.Add(daily);
                                    updatedTargetCount++;
                                }
                            }
                        }
                        targetToUpdate.MonthlyTargets.Add(mthlyTarget);
                    }
                }
            }
        }

        /// <summary>
        /// Updates daily targets of a monthly target
        /// </summary>
        /// <param name="monthlyTargetItem">Monthly target item which contains 
        /// daily targets to update</param>
        /// <param name="monthlyTargetToUpdate">MonthlyTarget entity to update</param>
        /// <param name="loggedInUserId">Logged in user ID</param>
        private void UpdateDailyTargets(MonthlyTargetItem monthlyTargetItem,
            MonthlyTarget monthlyTargetToUpdate, int loggedInUserId, ref int updatedTargetCount)
        {
            //looping through daily targets for add/edit daily targets
            foreach (var dailyTarget in monthlyTargetItem.DailyTargets)
            {
                //updating an already existing daily target
                if (dailyTarget.Id.HasValue)
                {
                    //fetching existing daily target object
                    var existingDailyTarget = monthlyTargetToUpdate.DailyTargets?
                       .FirstOrDefault(x => x.Id == dailyTarget.Id.Value);

                    if (existingDailyTarget != null)
                    {
                        if (existingDailyTarget.MaxGoalValue != dailyTarget.GoalValue ||
                        existingDailyTarget.IsManual != dailyTarget.IsManual)
                        {
                            existingDailyTarget.MaxGoalValue = dailyTarget.GoalValue;
                            existingDailyTarget.IsManual = dailyTarget.IsManual;
                            existingDailyTarget.LastModifiedOn = TimeZoneUtility.
                                GetCurrentTimestamp();
                            existingDailyTarget.LastModifiedBy = loggedInUserId;
                            existingDailyTarget.DailyTargetHistory.Add(TargetConverters.
                                ConvertDailyTargetToDailyTargetHistory(existingDailyTarget));
                            updatedTargetCount++;
                        }
                    }
                    //check if any changes to goal value that needs to be updated

                }
                //adding new daily target 
                else if (monthlyTargetToUpdate.DailyTargets?.Count(target => target.Day == dailyTarget.Day) > 0)
                {
                    var existingDailyTarget = monthlyTargetToUpdate.DailyTargets?
                        .FirstOrDefault(x => x.Day == dailyTarget.Day);
                    if (existingDailyTarget != null)
                    {
                        if (existingDailyTarget?.MaxGoalValue != dailyTarget.GoalValue ||
                        existingDailyTarget?.IsManual != dailyTarget.IsManual)
                        {
                            existingDailyTarget.MaxGoalValue = dailyTarget.GoalValue;
                            existingDailyTarget.IsManual = dailyTarget.IsManual;
                            existingDailyTarget.LastModifiedOn = TimeZoneUtility.
                                GetCurrentTimestamp();
                            existingDailyTarget.LastModifiedBy = loggedInUserId;
                            existingDailyTarget.DailyTargetHistory.Add(TargetConverters.
                                ConvertDailyTargetToDailyTargetHistory(existingDailyTarget));
                            updatedTargetCount++;
                        }
                    }

                }
                else if (dailyTarget.GoalValue != null)
                {
                    DailyTarget daily = TargetConverters.
                    ConvertDailyTargetItemToDailyTargetEntity(dailyTarget,
                      monthlyTargetToUpdate, loggedInUserId);
                    monthlyTargetToUpdate.DailyTargets.Add(daily);
                    updatedTargetCount++;
                }
                if (dailyTarget.IsOutofRange || dailyTarget.IsHoliday)
                {
                    var existingDailyTarget = monthlyTargetToUpdate.DailyTargets
                        ?.FirstOrDefault(x => x.Day == dailyTarget.Day);

                    if (existingDailyTarget != null)
                    {
                        updatedTargetCount++;
                        ClearObseleteDailyTarget(existingDailyTarget, loggedInUserId);
                    }

                }
            }
        }

        ///<summary>
        /// Clear Obsoleted Monthly Target and DailyTargets associated the target
        ///</summary>
        ///<param name="monthlyTargetId"></param>
        private void ClearObseletedMonthlyTargets(MonthlyTarget deletedMonth, int loggedInUserId)
        {
            if (deletedMonth.RolledUpGoalValue.HasValue)
            {
                UnsetObseletedRolledUpMonthlyTarget(deletedMonth, loggedInUserId);
            }
            else
            {
                var dailyTargetHistoryList = dailyTargetHistoryRepository.GetAll().Where(x => x.MonthlyTargetId == deletedMonth.Id).ToList();
                dailyTargetHistoryList.ForEach(x =>
                {
                    dailyTargetHistoryRepository.Remove(x);
                });

                var monthlyTargetHistoryList = deletedMonth.MonthlyTargetHistory.ToList();
                monthlyTargetHistoryList.ForEach(x =>
                {
                    monthlyTargetHistoryRepository.Remove(x);
                });
                monthlyTargetRepository.Remove(deletedMonth);
            }

        }

        /// <summary>
        ///  Clear Obsoleted DailyTargets associated the monthlyTarget
        /// </summary>
        /// <param name="monthlyTarget"></param>
        private void ClearObseleteDailyTargetofMonthlyTargets(MonthlyTarget existingMonthlyTarget, int loggedInUserId)
        {
            existingMonthlyTarget.DailyTargets?.ToList().ForEach(x =>
        {
            ClearObseleteDailyTarget(x, loggedInUserId);
        });
        }

        ///<summary>
        /// Unset Monthly Target Details in case of rolled up value present
        /// </summary>
        private void UnsetObseletedRolledUpMonthlyTarget(MonthlyTarget rolledUpMonth, int loggedInUserId)
        {
            rolledUpMonth.MaxGoalValue = null;
            rolledUpMonth.StretchGoalValue = null;
            rolledUpMonth.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            rolledUpMonth.LastModifiedBy = loggedInUserId;
            rolledUpMonth.DailyTargets?.ToList().ForEach(x =>
                {
                    ClearObseleteDailyTarget(x, loggedInUserId);
                });
            var monthlyTargetHistory = TargetConverters.ConvertMonthlyTargetToMonthlyTargetHistory(rolledUpMonth);
            rolledUpMonth.MonthlyTargetHistory.Add(monthlyTargetHistory);
        }

        ///<summary>
        ///Clear Obsoleted DailyTargets associated with the target
        ///</summary>
        ///<param name="dailyTargetId"></param>
        private void ClearObseleteDailyTarget(DailyTarget deletedDayTarget, int loggedInUserId)
        {
            if (deletedDayTarget != null)
            {
                if (deletedDayTarget.RolledUpGoalValue.HasValue)
                {
                    UnsetObseletedRolledUpDailyTarget(deletedDayTarget, loggedInUserId);
                }
                else
                {
                    var dailyTargetHistoryList = deletedDayTarget.DailyTargetHistory.ToList();

                    dailyTargetHistoryList.ForEach(x =>
                    {
                        dailyTargetHistoryRepository.Remove(x);
                    });
                    dailyTargetRepository.Remove(deletedDayTarget);
                }

            }

        }


        /// <summary>
        /// Unset DailyTargetDetails in case of rolled up value present
        /// </summary>
        /// <param name="rolledUpDay"></param>
        /// <param name="loggedInUserId"></param>
        private void UnsetObseletedRolledUpDailyTarget(DailyTarget rolledUpDay, int loggedInUserId)
        {
            rolledUpDay.MaxGoalValue = null;
            rolledUpDay.IsManual = false;
            rolledUpDay.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            rolledUpDay.LastModifiedBy = loggedInUserId;
            var dailyTargetHistory = TargetConverters.ConvertDailyTargetToDailyTargetHistory(rolledUpDay);
            rolledUpDay.DailyTargetHistory.Add(dailyTargetHistory);
        }

        /// <summary>
        /// Delete the target if actuals are not entered yet
        /// </summary>
        /// <param name="target"></param>
        /// <param name="loggedInUserId"></param>
        private bool DeleteExistingMetricTarget(Target target, int loggedInUserId)
        {
            var parentTargetId = target.ParentTargetId;
            bool isTargetRolledUp = false;
            targetValidator.ValidateMetricTargetDeleteRequest(target);

            //Target deactivate(soft delete)
            target.IsActive = false;
            target.LastModifiedBy = loggedInUserId;
            target.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            targetRepository.AddOrUpdate(target);
            // clear monthly targets of deleted target
            target.MonthlyTargets?.ToList().ForEach(x => ClearObseletedMonthlyTargets(x, loggedInUserId));

            target.TargetHistory.Add(TargetConverters.ConvertTargetToTargetHistory(target));

            if (target.CascadeFromParent)
            {
                var parentTarget = targetRepository.Get(parentTargetId.Value);
                var siblingTargetsCount = targetRepository.GetAll().Where(x =>
                                                             x.ParentTargetId == parentTarget.Id
                                                             && x.Id != target.Id && x.IsActive)
                                                             ?.Count();
                if (siblingTargetsCount.HasValue && siblingTargetsCount.Value == 0)
                {
                    parentTarget.IsCascaded = false;
                    parentTarget.CascadedMetricsTrackingMethodId = null;
                    var targetMonth = target.EffectiveStartDate.Month;
                    parentTarget.MonthlyTargets.Where(x => x.Month >= targetMonth).ToList().ForEach(x =>
                    {
                        x.RolledUpGoalValue = null;
                        x.IsRolledUpGoal = false;
                    });
                    parentTarget.LastModifiedBy = loggedInUserId;
                    parentTarget.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    targetRepository.AddOrUpdate(parentTarget);
                    isTargetRolledUp = true;
                }
            }
            targetRepository.Save();
            return isTargetRolledUp;
        }
        /// <summary>
        /// Clear parent target cascading details if the parent target is not cascaded 
        /// to other children
        /// </summary>
        /// <param name="targetItem">Target Item to be updated</param>
        /// <param name="loggedInUserId">Logged In user</param>
        private void ClearParentTargetCascadedInfo(TargetItem targetItem, int loggedInUserId)
        {
            // If metric is going to be cascaded from parent, we need to set IsCascaded true 
            // in parent record
            int? parentScorecardId = scorecardRepository.Get(targetItem.ScorecardId).
                ParentScorecardId;
            if (parentScorecardId != null)
            {
                var parentTarget = targetRepository.GetAll().Where(x =>
                    x.ScorecardId == parentScorecardId &&
                    x.KPIId == targetItem.KPIId &&
                    x.MetricId == targetItem.MetricId &&
                    x.MetricType == targetItem.MetricType &&
                    x.CalendarYearId == targetItem.CalendarYearId && x.IsActive).FirstOrDefault();
                if (parentTarget != null)
                {
                    //check if parent target is cascaded to other children
                    bool siblingTargetExists = targetRepository.GetAll()
                        .Any(x => x.ParentTargetId == parentTarget.Id
                        && x.Id != targetItem.Id && x.IsActive);
                    //if no other children are cascaded, remove cascaded info from parent
                    if (!siblingTargetExists)
                    {
                        parentTarget.IsCascaded = false;
                        parentTarget.RollUpMethodId = null;
                        int currentMonth = TimeZoneUtility.GetCurrentTimestamp().Month;

                        parentTarget.MonthlyTargets.Where(x => x.Month >= currentMonth).ToList().ForEach(x =>
                        {
                            x.RolledUpGoalValue = null;
                            x.IsRolledUpGoal = false;
                            x.DailyTargets.ToList().ForEach(y => y.RolledUpGoalValue = null);
                        });
                        parentTarget.LastModifiedBy = loggedInUserId;
                        parentTarget.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                        parentTarget.TargetHistory.Add(TargetConverters.
                            ConvertTargetToTargetHistory(parentTarget));
                    }
                }
            }
        }

        /// <summary>
        /// Method to clear cascaded details from children, when parent has changed metric/metric type
        /// </summary>
        /// <param name="targetItem">Target Item to be updated</param>
        /// <param name="loggedInUserId">Logged In user</param>
        private void ClearTargetCascadedDetails(TargetItem targetItem, int loggedInUserId)
        {
            //get all child targets of selected parent target
            var childTargets = targetRepository.GetAll()
                .Where(x => x.ParentTargetId == targetItem.Id && x.IsActive).ToList();
            // Clear all cascaded child targets
            foreach (Target childTarget in childTargets)
            {
                childTarget.CascadeFromParent = false;
                childTarget.ParentTargetId = null;

                // Add history of modification
                childTarget.TargetHistory.Add(TargetConverters.ConvertTargetToTargetHistory
                    (childTarget));
            }
        }

        /// <summary>
        /// Gets the next year id from current year id.
        /// </summary>
        /// <param name="currentYearId">The current year id.</param>
        /// <returns>The next year id.</returns>
        private int GetNextYearId(int currentYearId)
        {
            var nextYear = yearRepository.GetAll().Where(x => x.Id > currentYearId)
                               ?.OrderBy(x => x.Id).FirstOrDefault();

            return nextYear.Id;
        }

        /// <summary>
        /// Retrieves all targets for a scorecard and KPI for copy
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>KPI Item</returns>
        private CopiedKPIItem GetCopyTargetsForScorecardAndKPI(int scorecardId, KPI kpi,
            int yearId)
        {
            CopiedKPIItem kpiItem = new CopiedKPIItem();
            bool isCopyMetricByDate = Convert.ToBoolean(ConfigurationManager
                                     .AppSettings[AppSettingsKeys.CopyMetricByDate]);
            List<Target> targets = new List<Target>();
            Year currentYear = yearRepository.Get(yearId);
            int nextYearId = GetNextYearId(yearId);
            kpiItem.CalendarYearId = nextYearId;
            kpiItem.KpiName = kpi.Name;
            kpiItem.ErrorMessage = SetErrorMessage(scorecardId, kpi.Id, currentYear, nextYearId, isCopyMetricByDate);
            kpiItem.Targets = new List<CopiedTargetItem>();
            DateTime effectiveEndDate = currentYear.EndDate.Date;
            DateTime effectiveStartDate = new DateTime(effectiveEndDate.Year, effectiveEndDate.Month, 1);

            if (kpiItem.ErrorMessage.Length <= 0)
            {
                if (isCopyMetricByDate)
                {
                    // get all active targets on 31st December current year
                    targets = targetRepository.GetAll()
                                 .Where(x => x.ScorecardId == scorecardId &&
                                 x.KPIId == kpi.Id && x.CalendarYearId == yearId &&
                                 x.IsActive &&
                                 x.EffectiveEndDate == effectiveEndDate).ToList();
                }
                else
                {
                    // get all active targets in December
                    var activeTargetsInDecember = targetRepository.GetAll()
                                 .Where(x => x.ScorecardId == scorecardId &&
                                 x.KPIId == kpi.Id && x.CalendarYearId == yearId &&
                                 x.IsActive &&
                                 x.EffectiveEndDate >= effectiveStartDate &&
                                 x.EffectiveEndDate <= effectiveEndDate).ToList();

                    // if same metric is found more than once then select 
                    // the one with highest effective end date
                    var uniqueTargets = from target in activeTargetsInDecember
                                        group target by target.MetricId
                                  into targetGroups
                                        select targetGroups.OrderByDescending(t => t.EffectiveEndDate).First();

                    // only 1 primary and 4 secondary metrics should be there. 
                    // select the targets based on highest effective end date                    

                    // check if primary metric is present. select the one with highest effective end date
                    if (uniqueTargets.Count(x => x.MetricType == MetricType.Primary) >= 1)
                    {
                        targets.Add(uniqueTargets.Where(x => x.MetricType == MetricType.Primary)
                                  .OrderByDescending(t => t.EffectiveEndDate).First());
                    }

                    // check if secondary metrics are present. select the first four metrics with highest effective end date
                    if (uniqueTargets.Count(x => x.MetricType == MetricType.Secondary) >= 1)
                    {
                        targets.AddRange(uniqueTargets.Where(x => x.MetricType == MetricType.Secondary)
                                  .OrderByDescending(t => t.EffectiveEndDate).Take(4));
                    }
                }

                if (targets != null && targets.Count() > 0)
                {
                    targets.ForEach(x => kpiItem.Targets.Add(TargetConverters.
                        ConvertTargetToCopyTargetItemDTO(x)));

                    kpiItem.Targets = kpiItem.Targets.OrderBy(x => x.MetricType).ThenBy(x => x.MetricName).ToList();
                }
            }


            return kpiItem;
        }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="currentYearId">The current year identifier.</param>
        /// <param name="nextYearId">The next year identifier.</param>
        /// <returns></returns>
        private string SetErrorMessage(int scorecardId, int kpiId,
            Year currentYear, int nextYearId, bool isCopyMetricByDate)
        {

            int targetCount = targetRepository.GetAll()
                          .Where(x => x.ScorecardId == scorecardId &&
                                 x.KPIId == kpiId && x.IsActive &&
                                 x.CalendarYearId == nextYearId)?.Count() ?? 0;
            if (targetCount > 0)
            {
                string metricExistsErrorMessage = Constants.MetricExistsForKPIErrorMessage;
                return metricExistsErrorMessage;
            }

            DateTime effectiveEndDate = currentYear.EndDate.Date;
            DateTime effectiveStartDate = new DateTime(effectiveEndDate.Year, effectiveEndDate.Month, 1);

            // get count of targets on Dec 31st current year
            if (isCopyMetricByDate)
            {
                targetCount = targetRepository.GetAll()
                              .Where(x => x.ScorecardId == scorecardId &&
                                     x.KPIId == kpiId &&
                                     x.CalendarYearId == currentYear.Id &&
                                     x.IsActive &&
                                     x.EffectiveEndDate == effectiveEndDate)?.Count() ?? 0;
            }
            // get count of targets in the month of Dec current year
            else
            {
                targetCount = targetRepository.GetAll()
                              .Where(x => x.ScorecardId == scorecardId &&
                                     x.KPIId == kpiId &&
                                     x.CalendarYearId == currentYear.Id &&
                                     x.IsActive &&
                                     x.EffectiveEndDate >= effectiveStartDate &&
                                     x.EffectiveEndDate <= effectiveEndDate)?.Count() ?? 0;
            }
            if (targetCount <= 0)
            {
                string metricNotExists = Constants.MetricNotExistsForKPIErrorMessage;
                return metricNotExists;
            }

            return string.Empty;
        }

        /// <summary>
        /// Check if target needs to be rolled up for this target entry
        /// </summary>
        /// <param name="target">Target to check</param>
        /// <returns>True if target needs to be rolled up</returns>
        private bool CheckIfTargetRollupRequired(Target target)
        {
            bool rollUpRequired = false;
            if (target.ParentTargetId.HasValue)
            {
                rollUpRequired = true;
            }
            return rollUpRequired;
        }

        /// <summary>
        /// Check if actual status needs to be updated for this target for current month
        /// </summary>
        /// <param name="target">Target to check</param>
        /// <returns>True if target needs to be rolled up</returns>
        private bool CheckIfActualStatusUpdateRequired(Target target)
        {
            bool statusUpdateRequired = false;
            int currentMonth = TimeZoneUtility.GetCurrentTimestamp().Month;
            if (target.CascadedMetricsTrackingMethodId != (int)CascadedMetricsTrackingMethod.RolledUpTargets)
            {
                int actualCount = 0;
                if (target.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    actualCount = dailyActualRepository.GetAll().Where(x => x.ActualValue != null && x.TargetId == target.Id && x.Date.Month == currentMonth)?.Count() ?? 0;
                }
                else if (target.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    actualCount = monthlyActualRepository.GetAll().Where(x => x.ActualValue != null && x.TargetId == target.Id && x.Month == currentMonth)?.Count() ?? 0;
                }

                statusUpdateRequired = actualCount > 0;
            }
            return statusUpdateRequired;
        }

        /// <summary>
        /// Performs roll up operation for target
        /// </summary>
        /// <param name="target">Target against which actual entry is made</param>
        /// <param name="username">The logged in user name</param>
        private void PerformTargetRollup(Target target, string username, bool updateStatus = false)
        {
            RollupManager rollupMgr = CreateRollupManager();
            rollupMgr.PerformTargetRollup(target, username, updateStatus);
        }

        /// <summary>
        /// Updates the actual status and goal.
        /// </summary>
        /// <param name="existingTarget">The existing target.</param>
        /// <param name="userName">Name of the user.</param>
        private void UpdateActualStatusAndGoal(Target existingTarget, string userName, bool isUpdatePreviousMonth)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            if (existingTarget.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                actualsModifier.UpdateDailyActualStatusAndGoalForMonth(existingTarget.Id, currentDate, userName);
                if (isUpdatePreviousMonth)
                {
                    var previousMonthDate = currentDate.AddMonths(-1);
                    actualsModifier.UpdateDailyActualStatusAndGoalForMonth(existingTarget.Id, previousMonthDate, userName);
                }
            }
            else if (existingTarget.TrackingMethodId == Constants.TrackingMethodMonthly)
            {
                if (isUpdatePreviousMonth)
                {
                    var previousMonthDate = currentDate.AddMonths(-1);
                    actualsModifier.UpdateMonthlyActualStatusAndGoalForMonth(existingTarget.Id, previousMonthDate, userName);
                }
                actualsModifier.UpdateMonthlyActualStatusAndGoalForMonth(existingTarget.Id, currentDate, userName);
            }
        }

        /// <summary>
        /// Adjusts the rollup targets based on target entry method.
        /// </summary>
        /// <param name="targetItem">The target item.</param>
        private void AdjustRollupTargets(TargetItem targetItem, int targetEntryMethodId, int mtdPerformanceTrackingId)
        {
            if (targetItem.MonthlyTargets != null && targetItem.MonthlyTargets.Any())
            {
                Year year = yearRepository.Get(targetItem.CalendarYearId.Value);
                foreach (var monthlyTarget in targetItem.MonthlyTargets)
                {
                    // check the target entry method for month. If monthly goal is present then
                    // no change to roll up value. If monthly goal is missing go by daily rate or
                    // target entry method
                    if (!monthlyTarget.GoalValue.HasValue && CheckIfMonthInTargetDates(targetItem, monthlyTarget.Month.Id) && monthlyTarget.RolledupGoalValue.HasValue)
                    {
                        int targetYear = CalendarUtility.GetYearOfTheMonth(year, monthlyTarget.Month.Id);

                        DateTime monthStartDate = new DateTime(targetYear, monthlyTarget.Month.Id, 1);
                        DateTime monthEndDate = new DateTime(targetYear, monthlyTarget.Month.Id, DateTime.DaysInMonth(targetYear, monthlyTarget.Month.Id));
                        if (monthStartDate < targetItem.EffectiveStartDate)
                        {
                            monthStartDate = targetItem.EffectiveStartDate;
                        }

                        if (monthEndDate > targetItem.EffectiveEndDate)
                        {
                            monthEndDate = targetItem.EffectiveEndDate;
                        }

                        if (monthlyTarget.DailyRateValue.HasValue || targetEntryMethodId == Constants.TargetEntryMethodDaily)
                        {
                            int totalDaysInMonth = (int)(monthEndDate - monthStartDate).TotalDays + 1; ;

                            int numberOfHolidaysInMonth = holidayCalculator.CountHolidaysBetweenDaysOfMonth(targetItem.Id.Value, targetItem.ScorecardId, monthStartDate, monthEndDate);

                            int totalWorkdaysInmonth = totalDaysInMonth - numberOfHolidaysInMonth;

                            if (mtdPerformanceTrackingId == (int)MTDPerformanceTrackingMethod.Latest)
                            {
                                var dailyTargets = monthlyTargetRepository.Get(monthlyTarget.Id.Value)?.DailyTargets;
                                if (dailyTargets != null && dailyTargets.Any(x => x.RolledUpGoalValue.HasValue))
                                {
                                    monthlyTarget.RolledupGoalValue = dailyTargets.OrderByDescending(x => x.Day).FirstOrDefault(x => x.RolledUpGoalValue.HasValue)?.RolledUpGoalValue;
                                }
                            }
                            else if (totalWorkdaysInmonth > 0)
                            {
                                monthlyTarget.RolledupGoalValue = Math.Round((monthlyTarget.RolledupGoalValue.Value / totalWorkdaysInmonth), 2, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to check whether the selected month is within the target effective dates
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        private bool CheckIfMonthInTargetDates(TargetItem target, int month)
        {
            if (month >= target.EffectiveStartDate.Month &&
                 month <= target.EffectiveEndDate.Month)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Protected Method(s)
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

        /// <summary>
        /// Creates an instance of HolidayCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual HolidayCalculator CreateHolidayCalculator(
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (holidayCalculator == null)
            {
                holidayCalculator = new HolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository,
                    scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
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
            IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository)
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
        /// Creates the workday pattern calculator.
        /// </summary>
        /// <param name="scorecardWorkdayPatternRepository">The scorecard workday pattern repository.</param>
        /// <returns>WorkdayCalculator.</returns>
        protected WorkdayCalculator CreateWorkdayPatternCalculator(IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository)
        {
            if (workdayPatternCalculator == null)
            {
                workdayPatternCalculator = new WorkdayCalculator(scorecardWorkdayPatternRepository);
            }
            return workdayPatternCalculator;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target Repository</param>
        public TargetManager(IBaseRepository<Target> targetRepository,
                            IBaseRepository<MonthlyTarget> monthlyTargetRepository,
                            IBaseRepository<DailyTarget> dailyTargetRepository,
                            IBaseRepository<Scorecard> scorecardRepository,
                            IBaseRepository<MetricMapping> metricMappingRepository,
                            IBaseRepository<Year> yearRepository,
                            IBaseRepository<RollupMethod> rollupMethodRepository,
                            IBaseRepository<TrackingMethod> trackingMethodRepository,
                            IBaseRepository<TargetEntryMethod> targetEntryMethodRepository,
                            IBaseRepository<GraphPlottingMethod> graphPlottingMethodRepository,
                            IBaseRepository<Metric> metricRepository,
                            IBaseRepository<DailyActual> dailyActualRepository,
                            IBaseRepository<MonthlyActual> monthlyActualRepository,
                            IBaseRepository<User> userRepository,
                            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
                            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
                            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
                            IBaseRepository<Recordable> recordableRepository,
                            IBaseRepository<DailyTargetHistory> dailyTargetHistoryRepository,
                            IBaseRepository<MonthlyTargetHistory> monthlyTargetHistoryRepository)
        {
            if (targetRepository == null || scorecardRepository == null
                || metricMappingRepository == null || yearRepository == null
                || rollupMethodRepository == null || metricRepository == null
                || dailyActualRepository == null || monthlyActualRepository == null
                || monthlyTargetRepository == null || userRepository == null
                || dailyTargetRepository == null || dailyTargetHistoryRepository == null
                || monthlyTargetHistoryRepository == null)
            {
                throw new ArgumentNullException("Repository", "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.scorecardRepository = scorecardRepository;
            this.metricMappingRepository = metricMappingRepository;
            this.yearRepository = yearRepository;
            this.rollupMethodRepository = rollupMethodRepository;
            this.trackingMethodRepository = trackingMethodRepository;
            this.targetEntryMethodRepository = targetEntryMethodRepository;
            this.graphPlottingMethodRepository = graphPlottingMethodRepository;
            this.metricRepository = metricRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.monthlyTargetRepository = monthlyTargetRepository;
            this.dailyTargetRepository = dailyTargetRepository;
            this.userRepository = userRepository;
            this.dailyTargetHistoryRepository = dailyTargetHistoryRepository;
            this.monthlyTargetHistoryRepository = monthlyTargetHistoryRepository;
            this.targetValidator = CreateTargetValidator(targetRepository, dailyActualRepository, monthlyActualRepository,
                yearRepository, scorecardRepository, metricRepository);
            this.holidayCalculator = CreateHolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
            this.recordableCalculator = CreateRecordablesCalculator(targetRepository,
                recordableRepository, userRepository);
            this.targetModifier = CreateTargetModifier(dailyTargetRepository, monthlyTargetRepository, userRepository, metricRepository, goalCalculator, holidayCalculator);
            this.actualsModifier = CreateActualsModifier(targetRepository, dailyActualRepository,
                monthlyActualRepository, userRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository, monthlyTargetRepository, dailyTargetHistoryRepository);
            this.workdayPatternCalculator = CreateWorkdayPatternCalculator(scorecardWorkdayPatternRepository);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Method to check whether parent scorecard has the same metric set for the KPI selected 
        /// by child scorecard
        /// </summary>
        /// <param name="request">Object containing scorecard id and the selected metric id,
        /// kpi id and metric type of child scorecard  
        /// </param>
        /// <returns>
        /// Cascading parent target details if parent has the same metric set else returns null
        /// which indicates the parent cannot cascade the targets to children
        /// </returns>
        public CascadedParentTargetItem GetCascadedMetricDetails(MetricCascadeRequest request)
        {
            var scorecard = scorecardRepository.Get(request.ScorecardId);
            if (scorecard != null)
            {
                // getting parent target having the same metric set selected for child
                var parentTarget = targetRepository.GetAll()
                                        .Where(x => x.ScorecardId == scorecard.ParentScorecardId
                                        && x.KPIId == request.KPIId
                                        && x.MetricId == request.MetricId
                                        && x.CalendarYearId == request.CalendarYearId
                                        && x.IsActive
                                        && x.EffectiveStartDate <= request.EffectiveStartDate
                                        && x.EffectiveStartDate <= request.EffectiveEndDate
                                        && x.EffectiveEndDate >= request.EffectiveStartDate
                                        && x.EffectiveEndDate >= request.EffectiveEndDate)
                                        .FirstOrDefault();
                // if parent target is present returns target details
                if (parentTarget != null)
                {
                    return new CascadedParentTargetItem()
                    {
                        ParentTargetId = parentTarget.Id,
                        EffectiveStartDate = parentTarget.EffectiveStartDate,
                        EffectiveEndDate = parentTarget.EffectiveEndDate,
                        IsStretchGoalEnabled = parentTarget.IsStretchGoalEnabled,
                        RollupMethodId = parentTarget.RollUpMethodId,
                        TrackingMethodId = parentTarget.TrackingMethodId,
                        CascadeFromParent = parentTarget.CascadeFromParent,
                        TargetEntryMethodId = parentTarget.TargetEntryMethodId,
                        GraphPlottingMethodId = parentTarget.GraphPlottingMethodId,
                        MTDPerformanceTrackingMethodId = parentTarget.MTDPerformanceTrackingMethodId,
                        MaximumAllowedMonthlyGoals = GetMaximumAllowedMonthlyGoals(parentTarget, request.TargetId, request.RollUpMethodId, request.TargetEntryMethodId)
                    };
                }
            }
            //return null if parent target is not found
            return null;
        }

        /// <summary>
        /// Retrieves all targets for a scorecard and KPI belongs to an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>List of target entries</returns>
        public IEnumerable<TargetItem> GetTargetsForScorecardAndKPI(int scorecardId, int kpiId,
            int yearId)
        {
            List<TargetItem> targetItems = new List<TargetItem>();
            bool scorecardExists = scorecardRepository.GetAll().Any(x => x.Id == scorecardId);
            if (scorecardExists)
            {
                var targets = targetRepository.GetAll().Where(x => x.ScorecardId == scorecardId
                                                            && x.KPIId == kpiId
                                                            && x.CalendarYearId == yearId
                                                            && x.IsActive).ToList();
                if (targets != null && targets.Count > 0)
                {
                    Year calendarYear = yearRepository.Get(yearId);
                    var monthItems = CalendarUtility.GetMonthsBetweenDates(calendarYear.StartDate,
                        calendarYear.EndDate).ToList();
                    targets.ForEach(x => targetItems.Add(TargetConverters.
                        ConvertTargetToTargetItemDTO(x, monthItems)));

                    foreach (TargetItem targetItem in targetItems)
                    {
                        // If the target is a cascaded one, we need to give the unallocated portions as well
                        if (targetItem.CascadeFromParent)
                        {
                            var scorecard = scorecardRepository.Get(scorecardId);
                            if (scorecard != null)
                            {
                                // Getting parent target having the same metric set selected for child
                                var parentTarget = targetRepository.GetAll().Where(
                                    x => x.ScorecardId == scorecard.ParentScorecardId &&
                                    x.KPIId == targetItem.KPIId &&
                                    x.MetricId == targetItem.MetricId &&
                                    x.CalendarYearId == targetItem.CalendarYearId &&
                                    x.EffectiveStartDate <= targetItem.EffectiveEndDate &&
                                    x.EffectiveEndDate >= targetItem.EffectiveStartDate &&
                                    x.IsActive).FirstOrDefault();
                                if (parentTarget != null)
                                {
                                    // Setting the Id and rollup method of parent
                                    targetItem.ParentTargetId = parentTarget.Id;
                                    targetItem.RollupMethodId = parentTarget.RollUpMethodId;
                                    targetItem.MaximumAllowedMonthlyGoals = GetMaximumAllowedMonthlyGoals
                                        (parentTarget, targetItem.Id, parentTarget.RollUpMethodId, targetItem.TargetEntryMethodId);
                                }
                            }
                        }
                        targetItem.CanDelete = targetValidator.CheckIfTargetCanbeDeleted(targetItem);
                        targetItem.HasMonthlyAndDailyTargets = targetValidator.CheckIfMetricHasMonthlyAndDailyTargets(targetItem);

                        if (targetItem.IsCascaded && targetItem.MTDPerformanceTrackingMethodId.HasValue)
                        {
                            AdjustRollupTargets(targetItem, targetItem.TargetEntryMethodId.Value, targetItem.MTDPerformanceTrackingMethodId.Value);
                        }
                    }
                    return targetItems;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the rolledup goals.
        /// </summary>
        /// <param name="targetItem">The target.</param>
        /// <returns></returns>
        public IEnumerable<MonthAndTarget> GetRolledupGoals(int targetId, int targetEntryMethodId, int mtdPerformanceTrackingId)
        {
            List<MonthAndTarget> rollupTargets = new List<MonthAndTarget>();
            var target = targetRepository.Get(targetId);
            Year year = yearRepository.Get(target.CalendarYearId);
            var monthItems = CalendarUtility.GetMonthsBetweenDates(year.StartDate,
                year.EndDate).ToList();
            var targetItem = TargetConverters.ConvertTargetToTargetItemDTO(target, monthItems);
            bool isPastMonth = false;

            // if target entry method changed clear all future month data
            if (targetEntryMethodId != targetItem.TargetEntryMethodId)
            {
                foreach (var monthlyTarget in targetItem.MonthlyTargets)
                {
                    isPastMonth = monthlyTarget.IsPastMonth || (monthlyTarget.Month.Id <= TimeZoneUtility.GetCurrentTimestamp().Month
                                  && year.StartDate.Year == TimeZoneUtility.GetCurrentTimestamp().Year);
                    if (!isPastMonth)
                    {
                        monthlyTarget.GoalValue = null;
                        monthlyTarget.DailyRateValue = null;
                    }
                }
            }

            AdjustRollupTargets(targetItem, targetEntryMethodId, mtdPerformanceTrackingId);
            targetItem.MonthlyTargets.ForEach(x =>
            {
                rollupTargets.Add(new MonthAndTarget
                {
                    Month = monthItems.FirstOrDefault(m => m.Id == x.Month.Id),
                    Value = x.RolledupGoalValue
                });
            });

            return rollupTargets.OrderBy(x => x.Month.Year).ThenBy(x => x.Month.Id).ToList();
        }

        /// <summary>
        /// Retrieves all daily targets for a scorecard, KPI and metric belongs to an year
        /// </summary>
        /// <param name="monthlyTargetId">Monthly Target Id</param>
        /// <returns>Daily target entries</returns>
        public IEnumerable<DailyTargetItem> GetDailyTargets(int monthlyTargetId)
        {
            return GetFullMonthDailyTargets(monthlyTargetId);
        }



        /// <summary>
        /// Gets the list of available workdays of the month with monthly goal distributed evenly if applicable
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <param name="yearId"></param>
        /// <param name="monthId"></param>
        /// <param name="goalValue"></param>
        /// <returns>List of system generated daily Target </returns>
        public IEnumerable<DailyTargetItem> GenerateDailyTargetsList(GenerateDailyTargetsRequest request)
            {
            var calendarYear = yearRepository.Get(request.YearId);
            //gets the year
            int targetYear = CalendarUtility.GetYearOfTheMonth(calendarYear, request.MonthId);
            //gets the day list of the required month
            var dayList = CalendarUtility.GetAllDaysInMonth(targetYear, request.MonthId).ToList();
            var existingMonthlyTarget = monthlyTargetRepository.GetAll().FirstOrDefault(x => x.Id == request.ExistingMonthlyTargetId);
            var dailyTargetList = new List<DailyTargetItem>();

            dayList.ForEach(day =>
            {
                dailyTargetList.Add(new DailyTargetItem
                {
                    Day = day,
                    IsHoliday = holidayCalculator.CheckIfDateIsaHoliday(request.ScorecardId, day, request.MonthId, targetYear),
                    IsOutofRange = goalCalculator.CheckIfSelectedDateIsOutOfDateRange(request.EffectiveStartDate, request.EffectiveEndDate, new DateTime(targetYear, request.MonthId, day)),
                    RolledUpGoalValue = existingMonthlyTarget?.DailyTargets?.FirstOrDefault(x => x.Day == day)?.RolledUpGoalValue
                });
            });

            var metric = metricRepository.Get(request.MetricId);
            // distribute monthly goal evenly
            if (request.TargetEntryMethodId == Constants.TargetEntryMethodMonthly && request.MonthlyGoalValue.HasValue)
            {
                dailyTargetList = goalCalculator.DistributeMonthlyGoalValueAmongWorkdaysEvenly(dailyTargetList, metric.DataTypeId, request.MonthlyGoalValue.Value);
            }
            //distribute daily rate evenly
            else if (request.TargetEntryMethodId == Constants.TargetEntryMethodDaily && request.DailyRateValue.HasValue)
            {
                dailyTargetList = goalCalculator.DistributeDailyRateAmongWorkDays(dailyTargetList, metric.DataTypeId, request.DailyRateValue.Value);
            }

            return dailyTargetList;
        }

        /// <summary>
        /// Method to retrieve all metrics associated with selected KPI 
        /// having organization data same as that of selected scorecard
        /// </summary>
        /// <param name="kpiId">selected kpi id</param>
        /// <param name="scorecardId">selected scorecard id</param>
        /// <returns>metric list</returns>
        public List<MetricItem> GetMetrics(int kpiId, int scorecardId)
        {
            ValidateScorecardWorkdayPattern(scorecardId);
            Scorecard scorecard = scorecardRepository.Get(scorecardId);

            if (scorecard != null)
            {
                List<int> businessSegmentIds = scorecard.BusinessSegments.Select(b => b.Id).ToList();
                List<int> divisionIds = scorecard.Divisions.Select(d => d.Id).ToList();
                List<int> facilityIds = scorecard.Facilities.Select(f => f.Id).ToList();
                List<int> departmentIds = scorecard.Departments.Select(d => d.Id).ToList();
                List<int> processIds = scorecard.Processes.Select(p => p.Id).ToList();
                List<int> productLineIds = scorecard.ProductLines.Select(p => p.Id).ToList();

                var metricMapping = metricMappingRepository.GetAll().Where(x => x.KPIId == kpiId
                    && ((x.BusinessSegmentId == 0) || businessSegmentIds.Contains(x.BusinessSegmentId) || businessSegmentIds.Contains(0))
                    && ((x.DivisionId == 0) || divisionIds.Contains(x.DivisionId) || divisionIds.Contains(0))
                    && ((x.FacilityId == 0) || facilityIds.Contains(x.FacilityId) || facilityIds.Contains(0))
                    && ((x.ProductLineId == 0) || productLineIds.Contains(x.ProductLineId) || productLineIds.Contains(0))
                    && ((x.DepartmentId == 0) || departmentIds.Contains(x.DepartmentId) || departmentIds.Contains(0))
                    && ((x.ProcessId == 0) || processIds.Contains(x.ProcessId) || processIds.Contains(0))
                    && x.IsActive && x.Metric.IsActive).Distinct().ToList();

                var metrics = metricMapping.Select(mm =>
                    TargetConverters.ConvertMetricToMetricItemDTO(mm.Metric));

                return metrics.ToList();
            }
            return null;
        }



        /// <summary>
        /// Adds a new target for a primary/secondary metric which belongs to a scorecard and KPI
        /// </summary>
        /// <param name="targetItem">New primary metric target entry</param>
        /// <param name="userName">logged in user name</param>
        public void AddMetricTarget(TargetItem targetItem, string userName)
        {
            // Round the target values to two decimal places
            TargetUtility.RoundTargetItem(targetItem);

            // Trim the time part of the date time
            targetItem.EffectiveStartDate = targetItem.EffectiveStartDate.Date;
            targetItem.EffectiveEndDate = targetItem.EffectiveEndDate.Date;
            // assign default monthly and DailyTargetValues
            GenerateMonthlyAndDailyTargets(targetItem);
            // Validate primary/secondary metric add request
            targetValidator.ValidateMetricAddTargetRequest(targetItem);
            // Get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                    x.AccountName == userName).Id;
            int? parentTargetId = null;
            if (targetItem.CascadeFromParent)
            {
                parentTargetId = UpdateParentTarget(targetItem, loggedInUserId);
                // If the current child target have siblings we need to clear the metric
                // cascading in case the roll up method is "Same As Child"
                if (parentTargetId != null &&
                    targetItem.RollupMethodId == Constants.RollupMethodSameAsChild)
                {
                    ClearSiblingTargetsMetricCascading(parentTargetId.Value, targetItem.Id);
                }
            }

            // Create the Target entity from Target Item DTO
            Target target = TargetConverters.ConvertTargetItemDTOToTarget(targetItem,
                parentTargetId, loggedInUserId);

            // for cascaded parent set the cascaded metric tracking method for previous, current and future months
            if (target.IsCascaded)
            {
                var previousMonth = TimeZoneUtility.GetCurrentTimestamp().Month - 1;
                target.MonthlyTargets.Where(x => x.Month >= previousMonth).ToList().ForEach(x =>
                {
                    x.IsRolledUpGoal = target.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets;
                });
            }

            target.Metric = metricRepository.Get(target.MetricId);
            targetRepository.AddOrUpdate(target);
            targetRepository.Save();

            if (CheckIfTargetRollupRequired(target))
            {
                PerformTargetRollup(target, userName);
            }
        }



        /// <summary>
        /// Clears sibling targets cascading to make sure that roll up is removed for them 
        /// </summary>
        /// <param name="parentTargetId">Parent target Id</param>
        /// <param name="currentTargetId">Current child target Id</param>
        private void ClearSiblingTargetsMetricCascading(int parentTargetId, int? currentTargetId)
        {
            var siblingTargets = targetRepository.GetAll().Where(x =>
                                                    x.ParentTargetId == parentTargetId
                                                    && x.CascadeFromParent && x.IsActive &&
                                                    (currentTargetId != null ? x.Id != currentTargetId : x.Id == x.Id)).ToList();

            // Clear all cascaded sibling targets
            foreach (Target siblingTarget in siblingTargets)
            {
                siblingTarget.CascadeFromParent = false;
                siblingTarget.ParentTargetId = null;
                // Add history of modification
                siblingTarget.TargetHistory.Add(TargetConverters.ConvertTargetToTargetHistory
                    (siblingTarget));
            }
        }

        /// <summary>
        /// Method to retrieve targets initial data (Years and roll up Methods)
        /// and bool to check whether bowling chart is applicable or not
        /// </summary>
        /// <param name="scorecardId">identifier of scorecard</param>
        /// <returns>
        /// object with year and roll up method list
        /// </returns>
        public TargetTemplateData GetTargetsInitialData(int scorecardId)
        {
            var scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard != null)
            {
                var metricTypes = Enum.GetValues(typeof(MetricType)).Cast<MetricType>().ToList();
                var currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                // Returns a metric type item with id and name
                List<MetricTypeItem> metricTypeItems = new List<MetricTypeItem>();
                metricTypes.ForEach(x => metricTypeItems.Add(new MetricTypeItem()
                {
                    Id = (int)x,
                    Name = x.ToString()
                }));
                return new TargetTemplateData()
                {
                    ScorecardName = scorecard.Name,
                    KPIOwners = scorecard.KPIOwners.Where(x => x.IsActive).
                        Select(x => x.User.FirstName + " " + x.User.LastName).ToList(),
                    MetricTypes = metricTypeItems,
                    Years = GetTargetYears(),
                    RollupMethods = GetRollupMethods(),
                    GraphPlottingMethods = GetGraphPlottingMethods(),
                    MtdTrackingMethods = GetMtdPerformanceTrackingMethods(),
                    CascadedMetricsTrackingMethods = GetCascadedMetricsTrackingMethods(),
                    TrackingMethods = GetTrackingMethods(),
                    TargetEntryMethods = GetTargetEntryMethods(),
                    IsBowlingChartApplicable = scorecard.IsBowlingChartApplicable,
                    CurrentDate = currentDate,
                    CurrentMonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                    DefaultRollupMethodId = Constants.RollupMethodSumOfChildren
                };
            }
            return null;
        }

        /// <summary>
        /// Returns month's list for a calendar year in ascending order
        /// </summary>
        /// <param name="yearId">Calendar year ID</param>
        /// <returns>List of months in ascending order</returns>
        public IEnumerable<MonthItem> GetMonthsListForCalendarYear(int yearId)
        {
            var calendarYear = yearRepository.Get(yearId);
            if (calendarYear != null)
            {
                var monthItems = CalendarUtility.GetMonthsBetweenDates(calendarYear.StartDate,
                    calendarYear.EndDate).ToList();
                return monthItems;
            }
            return null;
        }

        /// <summary>
        /// Update target for a primary metric which belongs to a scorecard and
        /// KPI
        /// </summary>
        /// <param name="targetItem">Target entry to be updated</param>
        /// <param name="userName">Logged in user name</param>
        public void EditMetricTarget(TargetItem targetItem, string userName)
        {
            int updatedTargetCount = 0;
            bool isCascadedMetricTrackingChanged = false;

            // Round the target values to two decimal places
            TargetUtility.RoundTargetItem(targetItem);

            // Trim the time part of the date time
            targetItem.EffectiveStartDate = targetItem.EffectiveStartDate.Date;
            targetItem.EffectiveEndDate = targetItem.EffectiveEndDate.Date;

            //fetching existing target object
            var existingTarget = targetRepository.Get(targetItem.Id.Value);

            GenerateMonthlyAndDailyTargets(targetItem);

            //validating edit target request
            targetValidator.ValidateMetricTargetEditRequest(targetItem, existingTarget);

            //get logged in user id
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            int? parentTargetId = null;
            if (targetItem.CascadeFromParent)
            {
                parentTargetId = UpdateParentTarget(targetItem, loggedInUserId);
                // If the current child target have siblings we need to clear the metric
                // cascading in case the roll up method is "Same As Child"
                if (parentTargetId != null &&
                    targetItem.RollupMethodId == Constants.RollupMethodSameAsChild)
                {
                    ClearSiblingTargetsMetricCascading(parentTargetId.Value, targetItem.Id);
                }
            }
            //clear parent target cascaded details when selected child target 
            //has selected not to cascade from parent
            if (existingTarget.CascadeFromParent && !targetItem.CascadeFromParent)
            {
                ClearParentTargetCascadedInfo(targetItem, loggedInUserId);
            }
            //if metric is changed, clear cascading relations between parent and children
            if (existingTarget.MetricId != targetItem.MetricId && existingTarget.IsCascaded)
            {
                // clear parent target cascaded info
                existingTarget.IsCascaded = false;
                existingTarget.RollUpMethodId = null;
                ClearTargetCascadedDetails(targetItem, loggedInUserId);
            }

            if (existingTarget.CascadedMetricsTrackingMethodId != targetItem.CascadedMetricsTrackingMethodId && existingTarget.IsCascaded)
            {
                isCascadedMetricTrackingChanged = true;
                existingTarget.CascadedMetricsTrackingMethodId = targetItem.CascadedMetricsTrackingMethodId;

                // for cascaded parent set the cascaded metric tracking method for previous, current and future months

                var previousMonth = TimeZoneUtility.GetCurrentTimestamp().Month - 1;
                existingTarget.MonthlyTargets.Where(x => x.Month >= previousMonth).ToList().ForEach(x =>
                {
                    x.IsRolledUpGoal = existingTarget.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets;
                });
            }

            // Update the properties
            existingTarget.MetricType = targetItem.MetricType;
            existingTarget.MetricId = targetItem.MetricId.Value;
            existingTarget.CascadeFromParent = targetItem.CascadeFromParent;
            existingTarget.ParentTargetId = parentTargetId;
            existingTarget.EffectiveStartDate = targetItem.EffectiveStartDate;
            existingTarget.EffectiveEndDate = targetItem.EffectiveEndDate;
            existingTarget.AnnualTarget = targetItem.AnnualTarget;
            existingTarget.IsStretchGoalEnabled = targetItem.IsStretchGoalEnabled ?? false;
            existingTarget.GraphPlottingMethodId = targetItem.GraphPlottingMethodId;
            existingTarget.TargetEntryMethodId = targetItem.TargetEntryMethodId;
            existingTarget.MTDPerformanceTrackingMethodId = targetItem.MTDPerformanceTrackingMethodId;
            existingTarget.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingTarget.LastModifiedBy = loggedInUserId;

            // Update the monthly targets
            UpdateMonthlyAndDailyTargets(targetItem, existingTarget, loggedInUserId,
                targetItem.MetricType, ref updatedTargetCount);

            // If the update request contains changing tracking method from daily to monthly, 
            // we need to empty the daily targets set so far
            if (targetItem.TrackingMethodId != null)
            {
                if (targetItem.TrackingMethodId == Constants.TrackingMethodMonthly &&
                targetItem.TrackingMethodId != existingTarget.TrackingMethodId)
                {
                    if (existingTarget.MonthlyTargets != null)
                    {
                        foreach (var monthlyTarget in existingTarget.MonthlyTargets)
                        {
                            if (monthlyTarget.DailyTargets != null)
                            {
                                foreach (DailyTarget dailyTarget in monthlyTarget.DailyTargets)
                                {
                                    if (dailyTarget.MaxGoalValue != null)
                                    {
                                        dailyTarget.MaxGoalValue = null;
                                    }
                                    dailyTarget.DailyTargetHistory.Add(TargetConverters.
                                            ConvertDailyTargetToDailyTargetHistory(dailyTarget));
                                }
                            }
                        }
                    }
                }
                existingTarget.TrackingMethodId = targetItem.TrackingMethodId.Value;
            }
            // Add history of modification
            existingTarget.TargetHistory.Add(TargetConverters.ConvertTargetToTargetHistory
                (existingTarget));
            targetRepository.Save();

            bool isUpdateStatus = CheckIfActualStatusUpdateRequired(existingTarget) && (updatedTargetCount > 0);
            if (isUpdateStatus || isCascadedMetricTrackingChanged)
            {
                UpdateActualStatusAndGoal(existingTarget, userName, true);
            }

            if (updatedTargetCount > 0)
            {
                if (CheckIfTargetRollupRequired(existingTarget))
                {
                    PerformTargetRollup(existingTarget, userName, isUpdateStatus);
                }
            }
        }

        /// <summary>
        /// Method to check whether the targets for cascaded metric set on a score card is cascaded 
        /// completely to child score cards
        /// </summary>
        /// <param name="scorecardId">scorecard id</param>
        /// <param name="kpiId">kpi id</param>
        /// <param name="metricId">metric id</param>
        /// <param name="calendarYearId">calendar year id</param>
        /// <returns>flag which says whether the target of scorecard is fully allocated or not
        /// </returns>
        public bool IsMetricTargetsCascadedCompletely(int scorecardId, int kpiId,
                                                      int metricId, int calendarYearId)
        {
            var target = targetRepository.GetAll()
                                        .Where(x => x.ScorecardId == scorecardId
                                        && x.KPIId == kpiId
                                        && x.MetricId == metricId
                                        && x.CalendarYearId == calendarYearId
                                        && x.IsActive).FirstOrDefault();
            var unallocatedTargets = GetMaximumAllowedMonthlyGoals(target, null, null, null);
            return unallocatedTargets.Any(x => x.Value != null);
        }

        /// <summary>
        /// Retrieves all selected year targets for a scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>
        /// List of target entries
        /// </returns>
        public IEnumerable<CopiedKPIItem> GetSelectedYearTargetsForScorecard(int scorecardId, int yearId)
        {
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard == null)
                return null;

            List<CopiedKPIItem> kpiItems = new List<CopiedKPIItem>();

            foreach (KPI kpi in scorecard.KPIs)
            {
                kpiItems.Add(GetCopyTargetsForScorecardAndKPI(scorecardId, kpi, yearId));
            }

            return kpiItems;
        }

        /// <summary>
        /// Retrieves the maximum allowed monthly goal for a new/existing target which is 
        /// going to be/being cascaded
        /// </summary>
        /// <param name="parentTargetId">Parent target Id</param>
        /// <param name="childTargetId">Child target Id(if child target is already created)</param>
        /// <param name="selectedRollUpMethodId">Selected rollup method id</param>
        /// <returns>Maximum allowed monthly goals</returns>
        public IEnumerable<MonthAndTarget> GetMaximumAllowedMonthlyGoals(int parentTargetId,
            int? childTargetId, int? selectedRollUpMethodId, int? selectedTargetEntryMethodId)
        {
            var parentTarget = targetRepository.GetAll().Where(x => x.Id == parentTargetId
                                                             && x.IsActive).FirstOrDefault();
            if (parentTarget == null)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }
            return GetMaximumAllowedMonthlyGoals(parentTarget, childTargetId,
                selectedRollUpMethodId, selectedTargetEntryMethodId);
        }

        /// <summary>
        /// Deletes a specific Metric target if the metric does not have any actuals associated.
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <returns>Status of the delete operation</returns>
        public bool DeleteMetricTarget(int targetId, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                                x => x.AccountName == userName).Id;
            var target = targetRepository.GetAll().FirstOrDefault(x => x.Id == targetId);

            bool isTargetRolledUp = DeleteExistingMetricTarget(target, loggedInUserId);

            if (CheckIfTargetRollupRequired(target) && !isTargetRolledUp)
            {
                PerformTargetRollup(target, userName, true);
            }
            return true;
        }


        /// <summary>
        /// Copies the targets in a scorecard.
        /// </summary>
        /// <param name="scorecardTarget">The targets for copy.</param>
        /// <param name="userName">Name of the user.</param>
        public void CopyTargets(CopiedScorecardItem scorecardTarget, string userName)
        {
            Year calendarYear = yearRepository.Get(scorecardTarget.CalendarYearId);
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            DateTime effectiveStartDate = calendarYear.StartDate;
            DateTime effectiveEndDate = calendarYear.EndDate;
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(x =>
                    x.AccountName == userName).Id;

            // if target is copied to current year after January, set effective start date as first of current month
            if (currentDate.Year == effectiveStartDate.Year && currentDate.Month > effectiveStartDate.Month)
            {
                effectiveStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            }
            var selectedTargetIds = scorecardTarget.Targets.Select(x => x.Id).ToList();
            var selectedTargets = targetRepository.GetAll().Where(x =>
                                                selectedTargetIds.Contains(x.Id)).ToList();

            selectedTargets.ForEach(target =>
            {
                Target parentTarget = null;
                if (target.CascadeFromParent)
                {
                    int? parentTargetId = GetCascadedMetricDetails(new MetricCascadeRequest
                    {
                        ScorecardId = target.ScorecardId,
                        KPIId = target.KPIId,
                        MetricId = target.MetricId,
                        CalendarYearId = calendarYear.Id,
                        MetricType = target.MetricType,
                        TargetId = target.Id

                    })?.ParentTargetId;

                    if (parentTargetId != null)
                    {
                        parentTarget = targetRepository.Get(parentTargetId.Value);
                        parentTarget.IsCascaded = true;
                        targetRepository.AddOrUpdate(parentTarget);
                    }
                }

                var updatedTarget = TargetConverters.ConvertCurrentYearTargetToNextYearTarget(target, loggedInUserId, parentTarget);

                // reset values before copy
                updatedTarget.EffectiveStartDate = parentTarget?.EffectiveStartDate ?? effectiveStartDate;
                updatedTarget.EffectiveEndDate = parentTarget?.EffectiveEndDate ?? effectiveEndDate;
                updatedTarget.CalendarYearId = calendarYear.Id;
                targetRepository.AddOrUpdate(updatedTarget);
            });

            targetRepository.Save();
        }

        /// <summary>
        /// validate if workday pattern is available for scorecard or throw business exception
        /// </summary>
        /// <param name="scorecard">The scorecard.</param>
        private void ValidateScorecardWorkdayPattern(int scorecardId)
        {
            bool isWorkdayPatternAvailable = workdayPatternCalculator.CheckIfWorkdayPatternAvailableForScorecard(scorecardId);
            if (!isWorkdayPatternAvailable)
                throw new NDMSBusinessException(Constants.ScorecardWorkdayPatternNotExists);
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
                    if (targetRepository != null)
                    {
                        targetRepository.Dispose();
                    }
                    if (monthlyTargetRepository != null)
                    {
                        monthlyTargetRepository.Dispose();
                    }
                    if (scorecardRepository != null)
                    {
                        scorecardRepository.Dispose();
                    }
                    if (metricMappingRepository != null)
                    {
                        metricMappingRepository.Dispose();
                    }
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    if (rollupMethodRepository != null)
                    {
                        rollupMethodRepository.Dispose();
                    }
                    if (trackingMethodRepository != null)
                    {
                        trackingMethodRepository.Dispose();
                    }
                    if (graphPlottingMethodRepository != null)
                    {
                        graphPlottingMethodRepository.Dispose();
                    }
                    if (metricRepository != null)
                    {
                        metricRepository.Dispose();
                    }
                    if (dailyActualRepository != null)
                    {
                        dailyActualRepository.Dispose();
                    }
                    if (monthlyActualRepository != null)
                    {
                        monthlyActualRepository.Dispose();
                    }
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    if (monthlyTargetHistoryRepository != null)
                    {
                        monthlyTargetHistoryRepository.Dispose();
                    }

                    // Assign references to null
                    targetRepository = null;
                    monthlyTargetRepository = null;
                    scorecardRepository = null;
                    metricMappingRepository = null;
                    yearRepository = null;
                    rollupMethodRepository = null;
                    trackingMethodRepository = null;
                    graphPlottingMethodRepository = null;
                    metricRepository = null;
                    dailyActualRepository = null;
                    userRepository = null;
                    monthlyActualRepository = null;
                    monthlyTargetHistoryRepository = null;
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

