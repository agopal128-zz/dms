using NDMS.Business.Common;
using NDMS.Business.Interfaces;
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
    /// <summary>
    /// Implements IScorecardManager interface to retrieve/update score card data
    /// </summary>
    public class ScorecardManager : IScorecardManager
    {
        #region Field(s)
        /// <summary>
        /// Scorecard Repository
        /// </summary>
        private IScorecardRepository scorecardRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;

        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Monthly target repository
        /// </summary>
        private IBaseRepository<MonthlyTarget> monthlyTargetRepository;

        /// <summary>
        /// ICounterMeasureManager reference
        /// </summary>
        private ICounterMeasureManager counterMeasureManager;        

        /// <summary>
        /// Recordable Repository
        /// </summary>
        private IBaseRepository<Recordable> recordableRepository;

        /// <summary>
        /// The scorecard workday tracker repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository;

        /// <summary>
        /// User Repository
        /// </summary>
        private INDMSUserRepository userRepository;

        /// <summary>
        /// Metric Repository
        /// </summary>
        private IBaseRepository<Metric> metricRepository;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        ///<summary>
        /// Reference to Holiday Calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        ///<summary>
        /// Reference to MTD Calculator
        /// </summary>
        private MTDPerformanceCalculator mtdCalculator;

        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardRecordablesCalculator recordablesCalculator;

        ///<summary>
        /// Reference to workday Calculator
        /// </summary>
        private WorkdayCalculator workdayPatternCalculator;

        /// <summary>
        /// Reference to target validator
        /// </summary>
        private TargetValidator targetValidator;

        /// <summary>
        /// Reference to actual manager
        /// </summary>
        private IActualsManager actualManager;

        #endregion

        #region Private Method(s)
        /// <summary>
        /// Retrieve the daily actuals for a month
        /// </summary>
        /// <param name="target">Corresponding target</param>
        /// <param name="year">Year for which we need the actuals</param>
        /// <param name="month">Month for which we need the actuals</param>
        /// <returns>Daily actuals for a month</returns>
        private List<DailyActualItem> GetDailyActuals(Target target, int year, int month)
        {
            if (!holidayCalculator.CheckIfPatternIsSetForScorecard(target.ScorecardId, month, year))
            {
                return null;
            }

            var actuals = dailyActualRepository.GetAll().Where(x => x.TargetId == target.Id &&
            x.Date.Month == month && x.Date.Year == year).Select(x => new DailyActualItem()
            {
                Id = x.Id,
                Day = x.Date.Day,
                Value = x.ActualValue,
                Status = x.Status
            }).ToList();

            // update the goal missing status
            foreach(var actual in actuals.Where(x=>x.Status == 0))
            {
                if (actual.Value.HasValue)
                {
                    actual.Status = ActualStatus.NoTarget;
                }
            }

            //set start date and end date for the month as the start and end date can be 
            //any date
            DateTime monthStartDate = new DateTime(year, month, 1);
            DateTime monthEndDate = new DateTime(year, month,
                DateTime.DaysInMonth(year, month));
            if (target.EffectiveStartDate > monthStartDate)
            {
                monthStartDate = target.EffectiveStartDate;
            }
            if (target.EffectiveEndDate < monthEndDate)
            {
                monthEndDate = target.EffectiveEndDate;
            }

            // get list of marked workdays and non-workdays
            var markedDays = scorecardWorkdayTrackerRepository.GetAll()
                            .Where(x => x.ScorecardId == target.ScorecardId &&
                            x.Date >= monthStartDate && x.Date <= monthEndDate && x.IsActive).ToList();

            foreach (var markedDay in markedDays)
            {
                if (!actuals.Any(x => x.Day == markedDay.Date.Day))
                {
                    actuals.Add(new DailyActualItem()
                    {
                        Day = markedDay.Date.Day,
                        Status = markedDay.IsWorkDay ?
                                 ActualStatus.NotEntered :
                                 ActualStatus.Holiday
                    });
                }
            }

            // Retrieves the holidays between effective days in a month from Scorecard Holiday Pattern info table 
            var holidayList = holidayCalculator.GetHolidayListForScorecardInaMonth(target.ScorecardId, monthStartDate, monthEndDate);            

            foreach (var holiday in holidayList)
            {
                // If Daily actuals table does not contain this day, add as holiday
                if (!actuals.Any(x => x.Day == holiday.Date.Day))
                {                   
                    actuals.Add(new DailyActualItem()
                    {
                        Day = holiday.Date.Day,
                        Status = ActualStatus.Holiday
                    });
                }
            }

            var days = CalendarUtility.GetAllDaysInMonth(year, month);
            // Fill the remaining days with empty values
            foreach (var day in days)
            {
                if (!actuals.Any(x => x.Day == day))
                {
                    actuals.Add(new DailyActualItem()
                    {
                        Day = day,
                        Status = (day >= monthStartDate.Day && day <= monthEndDate.Day) ?
                        ActualStatus.NotEntered
                        : ActualStatus.NotApplicable
                    });
                }
            }
            return actuals.OrderBy(x => x.Day).ToList();
        }

        /// <summary>
        /// Get monthly actuals for an year
        /// </summary>
        /// <param name="targetId">Corresponding target Id</param>
        /// <param name="year">Year for which we need target Id</param>
        /// <returns>List of monthly actuals for an year</returns>
        private List<MonthlyActualItem> GetMonthlyActuals(Target target)
        {
            Year year = target.CalendarYear;
            var actuals = monthlyActualRepository.GetAll().Where(x =>
            x.TargetId == target.Id).Select(x => new MonthlyActualItem()
            {
                Id = x.Id,
                Month = x.Month,
                Value = x.ActualValue,
                Status = x.Status
            }).ToList();

            // update the goal missing status
            foreach (var actual in actuals.Where(x => x.Status == 0))
            {
                if (actual.Value.HasValue)
                {
                    actual.Status = ActualStatus.NoTarget;
                }
            }

            IEnumerable<MonthItem> monthsList = CalendarUtility.
                GetMonthsBetweenDates(year.StartDate, year.EndDate);
            IEnumerable<MonthItem> targetMonths = CalendarUtility.
                GetMonthsBetweenDates(target.EffectiveStartDate, target.EffectiveEndDate);
            // Fill the remaining days with empty values
            foreach (var month in monthsList)
            {
                if (!actuals.Any(x => x.Month == month.Id))
                {
                    actuals.Add(new MonthlyActualItem()
                    {
                        Month = month.Id,
                        Status = targetMonths.Any(x => x.Id == month.Id) ? ActualStatus.NotEntered
                        : ActualStatus.NotApplicable
                    });
                }
            }
            return actuals.OrderBy(x => x.Month).ToList();
        }

        /// <summary>
        /// Get daily actual for a day
        /// </summary>
        /// <param name="targetId">Corresponding target Id</param>
        /// <param name="date">Required date</param>
        /// <returns>Date's actual</returns>
        private DailyActual GetDailyActual(int targetId, DateTime date)
        {

            var actual = dailyActualRepository.GetAll().Where(x =>
            x.TargetId == targetId && x.Date == date).FirstOrDefault();
            return actual;
        }

        /// <summary>
        /// Get monthly actual for a month
        /// </summary>
        /// <param name="targetId">Corresponding target Id</param>
        /// <param name="month">Month for which we need actuals</param>
        /// <returns>Monthly actual</returns>
        private MonthlyActualItem GetMonthlyActual(int targetId, int month)
        {
            var actual = monthlyActualRepository.GetAll().Where(x =>
            x.TargetId == targetId && x.Month == month).Select(x => new MonthlyActualItem()
            {
                Id = x.Id,
                Month = x.Month,
                Value = x.ActualValue,
                Status = x.Status
            }).FirstOrDefault();
            return actual;
        }

        /// <summary>
        /// Get the last work day of month
        /// </summary>
        /// <param name="targetId">Corresponding target Id</param>
        /// <param name="month">Month for which we need last day for</param>
        /// <returns>LastWorkDate of month</returns>
        private DateTime? GetLastWorkDayOfMonth(int targetId, int month)
        {
            var lastWorkDayActual = dailyActualRepository.GetAll().Where(x =>
            x.TargetId == targetId && x.Date.Month == month && x.ActualValue != null).OrderByDescending(x => x.Date).FirstOrDefault();

            return lastWorkDayActual?.Date;
        }

        /// <summary>
        /// Gets the primary metric data associated with a score card
        /// </summary>
        /// <param name="target">Target corresponding</param>
        /// <param name="year">Year for which we need actual values</param>
        /// <param name="month">Month for which we need actual values</param>
        /// <returns>Primary metric data retrieved</returns>
        private PrimaryMetricData GetPrimaryMetricData(Target target, int year, int month)
        {
            PrimaryMetricData primMetricData = new PrimaryMetricData();
            primMetricData.TargetId = target.Id;
            primMetricData.MetricId = target.MetricId;
            primMetricData.MetricName = target.Metric.Name;
            primMetricData.MetricDataTypeId = target.Metric.DataTypeId;
            primMetricData.IsCascaded = target.IsCascaded;
            primMetricData.TrackingMethodId = target.TrackingMethodId;
            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                primMetricData.DailyActuals = GetDailyActuals(target, year, month);
            }
            else
            {
                primMetricData.MonthlyActuals = GetMonthlyActuals(target);
            }
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault
                (x => x.Month == month);
            primMetricData.TargetEntryMethodId = target.TargetEntryMethodId.Value;
            if (target.GraphPlottingMethodId.HasValue)
            {
                primMetricData.GraphPlottingMethodId = target.GraphPlottingMethodId.Value;
            }
            if (monthlyTarget != null)
            {
                primMetricData.MonthlyGoalValue = GetMonthlyGoal(target.Id, month);
                primMetricData.MonthlyStretchGoalValue = monthlyTarget.StretchGoalValue ?? null;
                primMetricData.DailyRate = monthlyTarget.DailyRate ?? null;
            }
            if(target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                primMetricData.MonthlyGoalValue = mtdCalculator.GetMonthToDateGoalValue(target, year, month);

            }           
            return primMetricData;
        }

        /// <summary>
        /// Gets the secondary metric data associated with a score card
        /// </summary>
        /// <param name="target">Target corresponding</param>
        /// <param name="month">Month for which we need actual values</param>
        /// <returns>Secondary metrics data retrieved</returns>
        private SecondaryMetricData GetSecondaryMetricData(Target target, int month)
        {
            var currentDateTime = TimeZoneUtility.GetCurrentTimestamp();
            var lastDayOfMonth = GetLastWorkDayOfMonth(target.Id, month);
            SecondaryMetricData secMetricData = new SecondaryMetricData();
            secMetricData.TargetId = target.Id;
            secMetricData.MetricId = target.MetricId;
            secMetricData.MetricName = target.Metric.Name;
            secMetricData.MetricDataTypeId = target.Metric.DataTypeId;
            secMetricData.IsCascaded = target.IsCascaded;

            // responsible for showing actual value of secondary in KPI screen
            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                secMetricData.DailyActual = new DailyActualItem();

                
                if (target.GraphPlottingMethodId != null)
                {
                    var mTDPerformance = mtdCalculator.GetMonthToDatePerformance(target, target.EffectiveStartDate.Year, month);                   

                    secMetricData.DailyActual.Value = mTDPerformance.ActualValue;
                    secMetricData.DailyActual.Status = mTDPerformance.Status;
                }
            }
            else
            {
                secMetricData.MonthlyActual = GetMonthlyActual(target.Id, month);
            }
            secMetricData.TargetEntryMethodId = target.TargetEntryMethodId.Value;
            if (target.GraphPlottingMethodId.HasValue)
            {
                secMetricData.GraphPlottingMethodId = target.GraphPlottingMethodId.Value;
            }
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == month);

            if (monthlyTarget != null)
            {
                // responsible for showing secondary goal value in KPI Screen
                bool hasMonthlyAndDailyTargets = targetValidator.CheckIfMetricHasMonthlyAndDailyTargetValues(target);
                if (!hasMonthlyAndDailyTargets)
                {
                    if (target.TargetEntryMethodId == Constants.TargetEntryMethodMonthly)
                    {
                        secMetricData.GoalValue = monthlyTarget.MaxGoalValue ?? null;
                    }
                    else if (target.TargetEntryMethodId == Constants.TargetEntryMethodDaily)
                    {
                        secMetricData.GoalValue = monthlyTarget.DailyRate ?? null;
                    }
                }
                else
                {
                    if (monthlyTarget.MaxGoalValue.HasValue)
                    {
                        secMetricData.GoalValue = monthlyTarget.MaxGoalValue ?? null;
                    }
                    else if (monthlyTarget.DailyRate.HasValue)
                    {
                        secMetricData.GoalValue = monthlyTarget.DailyRate ?? null;
                    }
                }

                secMetricData.MonthlyStretchGoalValue = monthlyTarget.StretchGoalValue ?? null;
                secMetricData.DailyRate = monthlyTarget.DailyRate ?? null;
            }

            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                secMetricData.MTDGoal = mtdCalculator.GetMonthToDateGoalValue(target, target.EffectiveStartDate.Year, month);
            }
            else if(target.TrackingMethodId == Constants.TrackingMethodMonthly)
            {
                secMetricData.MTDGoal = GetMonthlyGoal(target.Id, month);
            }
            return secMetricData;
        }

        /// <summary>
        /// Gets the secondary matric daily data of a particular Kpi of a particular scorecard
        /// </summary>
        /// <param name="target">Target corresponding</param>
        /// <param name="year">Year for which we need actual values</param>
        /// <param name="month">Month for which we need actual values</param>
        /// <returns></returns>
        private SecondaryMetricData GetSecondaryMetricTrackingData(Target target, int year, int month)
        {
            SecondaryMetricData secondaryMetricData = new SecondaryMetricData
            {
                TargetId = target.Id,
                MetricId = target.MetricId,
                MetricName = target.Metric.Name,
                MetricDataTypeId = target.Metric.DataTypeId,
                IsCascaded = target.IsCascaded,
                TrackingMethodId = target.TrackingMethodId,

            };
            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                secondaryMetricData.DailyActuals = GetDailyActuals(target, year, month);
            }
            else
            {
                secondaryMetricData.MonthlyActuals = GetMonthlyActuals(target);
            }
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == month);

            if (monthlyTarget != null)
            {
                secondaryMetricData.GoalValue = monthlyTarget.MaxGoalValue ?? null;
                secondaryMetricData.MonthlyStretchGoalValue = monthlyTarget.StretchGoalValue ?? null;
            }
            if (target.TargetEntryMethodId == Constants.TargetEntryMethodDaily)
            {
                secondaryMetricData.GoalValue = monthlyTarget?.DailyRate ?? null;
            }
            if (target.TrackingMethodId == Constants.TrackingMethodDaily)
            {
                secondaryMetricData.MTDGoal = mtdCalculator.GetMonthToDateGoalValue(target, target.EffectiveStartDate.Year, month);
            }
            else if(target.TrackingMethodId == Constants.TrackingMethodMonthly)
            {
                secondaryMetricData.MTDGoal = GetMonthlyGoal(target.Id, month);
            }
            secondaryMetricData.TargetEntryMethodId = target.TargetEntryMethodId.Value;
            if (monthlyTarget != null)
            {
                secondaryMetricData.DailyRate = monthlyTarget.DailyRate ?? null;
            }
            if (target.GraphPlottingMethodId.HasValue)
            {
                secondaryMetricData.GraphPlottingMethodId = target.GraphPlottingMethodId.Value;
            }
            return secondaryMetricData;
        }

        ///<summary>
        /// validate if scorecard is available or throw business exception
        ///</summary>
        private void ValidateScorecardDetails(Scorecard scorecard, int yearId, int month)
        {
            if (scorecard == null)
                throw new NDMSBusinessException(Constants.ScorecardNotExists);                       
        }


        /// <summary>
        /// map scorecard children to DTO
        /// </summary>
        /// <param name="children">scorecard children</param>
        /// <returns>child scorecards</returns>
        private List<ScorecardDrilldownNode> AddChildScorecards(ICollection<Scorecard> children,
            int kpiId, int month, int yearId)
        {
            var childScorecards = new List<ScorecardDrilldownNode>();
            foreach (var child in children.Where(x => x.IsActive))
            {
                childScorecards.Add(new ScorecardDrilldownNode()
                {
                    Id = child.Id,
                    Name = child.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                    CanViewScorecard = true,
                    IsActive = child.IsActive,
                    SortOrder = child.SortOrder,
                    HasPrimaryCascaded = GetCascadedMetricDetails(child.Id, kpiId, month, yearId, MetricType.Primary),
                    HasSecondaryCascaded = GetCascadedMetricDetails(child.Id, kpiId, month, yearId, MetricType.Secondary),
                    CascadedPrimaryMetricStatus = GetCascadedMetricStatus(child.Id, kpiId, month, yearId, MetricType.Primary),
                    CascadedSecondaryMetricStatus = GetCascadedMetricStatus(child.Id, kpiId, month, yearId, MetricType.Secondary),
                    ScorecardStatus = GetKPIDrilldownStatus(child.Id, kpiId, month, yearId),
                    Children = AddChildScorecards(child.ChildScorecards, kpiId, month, yearId)
                });
            }
            return childScorecards;
        }

        /// <summary>
        /// map scorecard children to DTO
        /// </summary>
        /// <param name="children">scorecard children</param>
        /// <returns>child scorecards</returns>
        private List<ScorecardDrilldownNode> AddChildScorecards(ICollection<Scorecard> children,
            int kpiId, DateTime selectedDate)
        {
            var childScorecards = new List<ScorecardDrilldownNode>();
            foreach (var child in children.Where(x => x.IsActive))
            {
                childScorecards.Add(new ScorecardDrilldownNode()
                {
                    Id = child.Id,
                    Name = child.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                    CanViewScorecard = true,
                    IsActive = child.IsActive,
                    SortOrder = child.SortOrder,
                    HasPrimaryCascaded = GetCascadedMetricDetails(child.Id, kpiId, selectedDate, MetricType.Primary),
                    HasSecondaryCascaded = GetCascadedMetricDetails(child.Id, kpiId, selectedDate, MetricType.Secondary),
                    CascadedPrimaryMetricStatus = GetCascadedMetricStatus(child.Id, kpiId, selectedDate, MetricType.Primary),
                    CascadedSecondaryMetricStatus = GetCascadedMetricStatus(child.Id, kpiId, selectedDate, MetricType.Secondary),
                    ScorecardStatus = GetKPIDrilldownStatus(child.Id, kpiId, selectedDate),
                    Children = AddChildScorecards(child.ChildScorecards, kpiId, selectedDate)
                });
            }
            return childScorecards;
        }

        /// <summary>
        /// Gets the cascaded metric status.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="metricType">Type of the metric.</param>
        /// <returns></returns>
        private ActualStatus GetCascadedMetricStatus(int scorecardId, int kpiId, int month, int yearId, MetricType metricType)
        {
            ActualStatus metricStatus = ActualStatus.NotEntered;
            var calendarYear = yearRepository.Get(yearId);
            //form a date using the given month and the corresponding year
            int year = CalendarUtility.GetYearOfTheMonth(calendarYear, month);
            var requestedDate = new DateTime(year, month, 1);

            //get all cascaded child metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.CalendarYearId == yearId && x.IsActive && x.CascadeFromParent);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList?.ToList().Where(x =>
                        requestedDate >= new DateTime
                        (x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
                        && requestedDate <= new DateTime
                        (x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)) &&
                        x.MetricType == metricType);

            if(scorecardKPIMetrics == null)
            {
                return metricStatus;
            }

            if (scorecardKPIMetrics.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.NotAchieved))
            {
                metricStatus = ActualStatus.NotAchieved;
            }
            else if(scorecardKPIMetrics.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.Achieved))
            {
                metricStatus = ActualStatus.Achieved;
            }

            return metricStatus;
        }

        /// <summary>
        /// Gets the cascaded metric status for specific day.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="metricType">Type of the metric.</param>
        /// <returns></returns>
        private ActualStatus GetCascadedMetricStatus(int scorecardId, int kpiId, DateTime selectedDate, MetricType metricType)
        {
            ActualStatus metricStatus = ActualStatus.NotEntered;

            //get all cascaded child metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.IsActive && x.CascadeFromParent);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList?.ToList().Where(x =>
                        selectedDate >= x.EffectiveStartDate
                        && selectedDate <= x.EffectiveEndDate &&
                        x.MetricType == metricType);

            if (scorecardKPIMetrics == null)
            {
                return metricStatus;
            }

            if (scorecardKPIMetrics.Any(x => GetMetricActualStatus(x.Id, x.TrackingMethodId, selectedDate) == ActualStatus.NotAchieved))
            {
                metricStatus = ActualStatus.NotAchieved;
            }
            else if (scorecardKPIMetrics.Any(x => GetMetricActualStatus(x.Id, x.TrackingMethodId, selectedDate) == ActualStatus.Achieved))
            {
                metricStatus = ActualStatus.Achieved;
            }

            return metricStatus;
        }

        /// <summary>
        /// Check cascaded metric of specific type exists in scorecard.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="metricType">Type of the metric.</param>
        /// <returns>
        /// True if cascaded secondary metric is present, otherwise false.
        /// </returns>
        private bool GetCascadedMetricDetails(int scorecardId, int kpiId, int month, int yearId, MetricType metricType)
        {
            bool isCascadedMetricExists = false;

            var calendarYear = yearRepository.Get(yearId);
            //form a date using the given month and the corresponding year
            int year = CalendarUtility.GetYearOfTheMonth(calendarYear, month);
            var requestedDate = new DateTime(year, month, 1);

            //get all cascaded child metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.CalendarYearId == yearId && x.IsActive && x.CascadeFromParent);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList?.ToList().Where(x =>
                        requestedDate >= new DateTime
                        (x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
                        && requestedDate <= new DateTime
                        (x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)) &&
                        x.MetricType == metricType);

            if (scorecardKPIMetrics != null && scorecardKPIMetrics.Any())
            {
                isCascadedMetricExists = true;
            }

            return isCascadedMetricExists;
        }

        /// <summary>
        /// Check cascaded metric of specific type exists in scorecard for specific date.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="metricType">Type of the metric.</param>
        /// <returns>
        /// True if cascaded secondary metric is present, otherwise false.
        /// </returns>
        private bool GetCascadedMetricDetails(int scorecardId, int kpiId, DateTime selectedDate, MetricType metricType)
        {
            bool isCascadedMetricExists = false;
           
            //get all cascaded child metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.IsActive && x.CascadeFromParent);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList?.ToList().Where(x =>
                        selectedDate >= x.EffectiveStartDate
                        && selectedDate <= x.EffectiveEndDate &&
                        x.MetricType == metricType);

            if (scorecardKPIMetrics != null && scorecardKPIMetrics.Any())
            {
                isCascadedMetricExists = true;
            }

            return isCascadedMetricExists;
        }

        /// <summary>
        /// Method to get drill down status based on kpi metrics of given scorecard 
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="month">month</param>
        /// <param name="yearId">Year Id</param>
        /// <returns>drill down status</returns>
        private ScorecardStatus GetKPIDrilldownStatus(int scorecardId, int kpiId, int month,
            int yearId)
        {
            ScorecardStatus scorecardStatus = ScorecardStatus.NotApplicable;
            var calendarYear = yearRepository.Get(yearId);
            //form a date using the given month and the corresponding year
            int year = CalendarUtility.GetYearOfTheMonth(calendarYear, month);
            var requestedDate = new DateTime(year, month, 1);

            //get all metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.CalendarYearId == yearId && x.IsActive);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList.ToList().Where(x =>
                        requestedDate >= new DateTime
                        (x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
                        && requestedDate <= new DateTime
                        (x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)));

           
            var isScorecardActive = scorecardRepository.Get(scorecardId).IsActive;
            if (isScorecardActive)
            {
                if (scorecardKPIMetrics.Any())
                {
                    var primaryMetricTarget = scorecardKPIMetrics.FirstOrDefault(x =>
                    x.MetricType == MetricType.Primary);
                    // get drill down status for primary metric
                    ActualStatus status = mtdCalculator.GetMetricMTDStatus(primaryMetricTarget, year, month);
                    scorecardStatus = GetDrillDownStatusFromActualStatus(status);
                    if (scorecardStatus == ScorecardStatus.PrimaryNotAchieved)
                    {
                        return scorecardStatus;
                    }
                    //get all secondary metrics
                    var secondaryMetricTargets = scorecardKPIMetrics.
                            Where(x => x.MetricType == MetricType.Secondary);


                    //check if any of the secondary metrics is not achieved set
                    //drill down status as primary status not achieved
                    if (secondaryMetricTargets != null && secondaryMetricTargets
                        .Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.NotAchieved))
                    {
                        scorecardStatus = ScorecardStatus.SecondaryNotAchieved;
                    }
                    //check if any of the secondary metrics is achieved
                    else if (secondaryMetricTargets.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.Achieved))
                    {
                        scorecardStatus = ScorecardStatus.Achieved;
                    }
                }
            }
            else
            {
                scorecardStatus = ScorecardStatus.Inactive;
            }
            
            return scorecardStatus;
        }

        /// <summary>
        /// Method to get drill down status based on kpi metrics of given scorecard for a specific day
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>
        /// drill down status
        /// </returns>
        private ScorecardStatus GetKPIDrilldownStatus(int scorecardId, int kpiId, DateTime selectedDate)
        {
            ScorecardStatus scorecardStatus = ScorecardStatus.NotApplicable;

            //get all metrics associated with the scorecard kpi
            var scorecardKPIMetricList = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.IsActive);
            //get the metrics which are active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = scorecardKPIMetricList.ToList().Where(x =>
                        selectedDate >= x.EffectiveStartDate
                        && selectedDate <= x.EffectiveEndDate);

            
            var isScorecardActive = scorecardRepository.Get(scorecardId).IsActive;
            if (isScorecardActive)
            {
                if (scorecardKPIMetrics.Any())
                {
                    var primaryMetricTarget = scorecardKPIMetrics.FirstOrDefault(x =>
                    x.MetricType == MetricType.Primary);
                    // get drill down status for primary metric
                    ActualStatus status = GetMetricActualStatus(primaryMetricTarget?.Id, primaryMetricTarget?.TrackingMethodId, selectedDate);
                    scorecardStatus = GetDrillDownStatusFromActualStatus(status);
                    if (scorecardStatus == ScorecardStatus.PrimaryNotAchieved)
                    {
                        return scorecardStatus;
                    }
                    //get all secondary metrics
                    var secondaryMetricTargets = scorecardKPIMetrics.
                            Where(x => x.MetricType == MetricType.Secondary);


                    //check if any of the secondary metrics is not achieved set
                    //drill down status as primary status not achieved
                    if (secondaryMetricTargets != null && secondaryMetricTargets
                        .Any(x => GetMetricActualStatus(x.Id, x.TrackingMethodId, selectedDate) == ActualStatus.NotAchieved))
                    {
                        scorecardStatus = ScorecardStatus.SecondaryNotAchieved;
                    }
                    //check if any of the secondary metrics is achieved
                    else if (secondaryMetricTargets.Any(x => GetMetricActualStatus(x.Id, x.TrackingMethodId, selectedDate) == ActualStatus.Achieved))
                    {
                        scorecardStatus = ScorecardStatus.Achieved;
                    }
                }
            }
            else
            {
                scorecardStatus = ScorecardStatus.Inactive;
            }
            
            return scorecardStatus;
        }

        /// <summary>
        /// Gets the metric actual status for specific date.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns></returns>
        private ActualStatus GetMetricActualStatus(int? targetId, int? trackingMethodId, DateTime selectedDate)
        {
            ActualStatus metricStatus = ActualStatus.NotEntered;

            if (!targetId.HasValue)
            {
                return metricStatus;
            }

            if (trackingMethodId == Constants.TrackingMethodDaily)
            {
                var dailyActualEntry = dailyActualRepository.GetAll().Where(x => x.TargetId == targetId && x.Date == selectedDate.Date).FirstOrDefault();

                if (dailyActualEntry != null)
                {
                    metricStatus = dailyActualEntry.Status;
                }
            }
            else if(trackingMethodId == Constants.TrackingMethodMonthly)
            {
                var monthlyActualEntry = monthlyActualRepository.GetAll().Where(x => x.TargetId == targetId && x.Month == selectedDate.Month).FirstOrDefault();

                if (monthlyActualEntry != null)
                {
                    metricStatus = monthlyActualEntry.Status;
                }
            }

            return metricStatus;
        }


        /// <summary>
        /// Get drill down status from actual status
        /// </summary>
        /// <param name="status">Input actual status</param>
        /// <returns>Drill down status as output</returns>
        private ScorecardStatus GetDrillDownStatusFromActualStatus(ActualStatus status)
        {
            ScorecardStatus scorecardStatus = ScorecardStatus.NotApplicable;
            if (status == ActualStatus.NotAchieved)
            {
                scorecardStatus = ScorecardStatus.PrimaryNotAchieved;
            }
            else if (status == ActualStatus.Achieved)
            {
                scorecardStatus = ScorecardStatus.Achieved;
            }
            return scorecardStatus;
        }

        /// <summary>
        /// checking drilldown status based on daily actuals for graph plotting method "Actual"
        /// </summary>
        /// <param name="dailyActualsList"></param>
        /// <returns></returns>
        private ActualStatus CheckDrillDownStatusForPlottingMethodActual(IQueryable<DailyActual>
            dailyActualsList)
        {
            int totalEntries = dailyActualsList.Count();
            if (totalEntries > 0)
            {
                decimal numberOfDailyTargetAchieved = dailyActualsList.Count(c =>
                c.Status == ActualStatus.Achieved);

                if ((numberOfDailyTargetAchieved / totalEntries) > .5m)
                {
                    return ActualStatus.Achieved;
                }
                else
                {
                    return ActualStatus.NotAchieved;
                }
            }
            else
            {
                return ActualStatus.NotEntered;
            }
        }

        /// <summary>
        /// Gets the scorecard kpis for a given month.
        /// </summary>
        /// <param name="scorecard">The scorecard.</param>
        /// <param name="month">The month.</param>
        /// <param name="requestedYear">The requested year.</param>
        /// <returns></returns>
        private IList<KPI> GetScorecardKPIsByMonth(Scorecard scorecard, int month, int yearId)
        {
            var year = yearRepository.Get(yearId);
            int requestedYear = CalendarUtility.GetYearOfTheMonth(year, month);
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            // for current and future months
            // then show active KPIs in the scorecard
            if ((currentDate.Month <= month && currentDate.Year == requestedYear) || (currentDate.Year < requestedYear))
            {
                return scorecard.KPIs?.ToList();
            }
            // for previous months show KPI for which targets are set
            else
            {
                DateTime monthStartDate = new DateTime(requestedYear, month, 1);
                DateTime monthEndDate = new DateTime(requestedYear, month, DateTime.DaysInMonth(requestedYear, month));
                // get active targets in the month
                var targets = targetRepository.GetAll().Where(x => x.ScorecardId == scorecard.Id
                              && x.IsActive && x.CalendarYearId == yearId
                              && x.EffectiveEndDate >= monthStartDate
                              && x.EffectiveStartDate <= monthEndDate)?.ToList();

                if (targets == null)
                {
                    return null;
                }

                var activeKPIsOfMonth = targets.Select(x => x.KPI).Distinct().OrderBy(x => x.Id);
                return activeKPIsOfMonth?.ToList();
            }
        }
        
        #endregion

        #region Protected Method(s)
        /// <summary>
        /// Creates an instance of ScorecardGoalCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardGoalCalculator CreateScorecardGoalCalculator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
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
        /// Creates an instance of MTDCalculator and returns
        /// </summary>
        /// <returns>The MTDCalculator</returns>
        protected virtual MTDPerformanceCalculator CreateMTDCalculator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (mtdCalculator == null)
            {
                mtdCalculator = new MTDPerformanceCalculator(targetRepository, dailyActualRepository, monthlyActualRepository,
                    scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository, scorecardWorkdayPatternRepository);
            }
            return mtdCalculator;
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
        /// Creates an instance of RecordablesCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardRecordablesCalculator CreateRecordablesCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<Recordable> recordableRepository, IBaseRepository<User> userRepository)
        {
            if (recordablesCalculator == null)
            {
                recordablesCalculator = new ScorecardRecordablesCalculator(targetRepository, recordableRepository, userRepository);
            }
            return recordablesCalculator;
        }

        protected WorkdayCalculator CreateWorkdayPatternCalculator(IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository)
        {
            if (workdayPatternCalculator == null)
            {
                workdayPatternCalculator = new WorkdayCalculator(scorecardWorkdayPatternRepository);
            }
            return workdayPatternCalculator;
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
        /// <param name="scorecardRepository">Scorecard Repository</param>
        /// <param name="targetRepository">Target repository</param>
        /// <param name="yearRepository">Year Repository</param>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        /// <param name="monthlyActualRepository">Monthly Actual Repository</param>
        /// <param name="monthlyTargetRepository">Monthly target repository</param>
        /// <param name="scorecardHolidayPatternRepository">The scorecard holiday pattern repository.</param>
        /// <param name="recordableRepository">The recordable repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="counterMeasureManager">Counter measure manager</param>
        /// <param name="scorecardWorkdayTrackerRepository">The scorecard workday tracker repository.</param>
        /// <param name="scorecardWorkdayPatternRepository">The scorecard workday pattern repository.</param>
        /// <param name="metricRepository">The metric repository.</param>
        /// <exception cref="System.ArgumentNullException">Repository - The given parameter cannot be null.</exception>
        public ScorecardManager(IScorecardRepository scorecardRepository,
            IBaseRepository<Target> targetRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<MonthlyTarget> monthlyTargetRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<Recordable> recordableRepository,
            INDMSUserRepository userRepository,
            ICounterMeasureManager counterMeasureManager,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<Metric> metricRepository,
            IActualsManager actualManager
            )
        {
            if (targetRepository == null || scorecardRepository == null ||
                yearRepository == null || dailyActualRepository == null ||
                monthlyActualRepository == null || monthlyTargetRepository == null ||
                scorecardWorkdayTrackerRepository == null || counterMeasureManager == null ||
                recordableRepository == null || userRepository == null || 
                metricRepository == null || actualManager == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.scorecardRepository = scorecardRepository;
            this.yearRepository = yearRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.monthlyTargetRepository = monthlyTargetRepository;
            this.userRepository = userRepository;
            this.metricRepository = metricRepository;
            this.recordableRepository = recordableRepository;
            this.counterMeasureManager = counterMeasureManager;
            this.actualManager = actualManager;
            this.scorecardWorkdayTrackerRepository = scorecardWorkdayTrackerRepository;
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository, scorecardWorkdayPatternRepository, scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
            this.holidayCalculator = CreateHolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository
                , scorecardWorkdayTrackerRepository, scorecardWorkdayPatternRepository);
            this.recordablesCalculator = CreateRecordablesCalculator(targetRepository, recordableRepository, userRepository);
            this.workdayPatternCalculator = CreateWorkdayPatternCalculator(scorecardWorkdayPatternRepository);
            this.targetValidator = CreateTargetValidator(targetRepository, dailyActualRepository, monthlyActualRepository,
             yearRepository, scorecardRepository, metricRepository);
            this.mtdCalculator = CreateMTDCalculator(targetRepository, dailyActualRepository, monthlyActualRepository, scorecardWorkdayPatternRepository, scorecardHolidayPatternRepository
                , scorecardWorkdayTrackerRepository);
        }

        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves the data belongs to a score with all KPI's and metrics associated for the
        /// current year/month depending on the tracking method associated
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="yearId">Identifier of the year for which we need score card data</param>
        /// <param name="month">Month for which we need score card data</param>
        /// <returns>
        /// Scorecard data for the current month/year
        /// </returns>
        public ScorecardData GetScorecardData(int scorecardId, int yearId, int month)
        {
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            ValidateScorecardDetails(scorecard, yearId, month);
            if (scorecard != null)
            {
                ScorecardData scorecardData = new ScorecardData();
                scorecardData.ScorecardId = scorecard.Id;
                scorecardData.ScorecardName = scorecard.Name;
                //get scorecard kpi owners list 
                scorecardData.KPIOwners = scorecard.KPIOwners.Where(x => x.IsActive)
                    .Select(x => x.User.FirstName + " " + x.User.LastName).ToList();
                var year = yearRepository.Get(yearId);
                // If the current year is present in NDMS year repository
                if (year != null)
                {
                    int requestedYear = CalendarUtility.GetYearOfTheMonth(year, month);
                    var scorecardKPIs = GetScorecardKPIsByMonth(scorecard, month, yearId);
                    scorecardData.IsPatternAssigned = holidayCalculator.CheckIfPatternIsSetForScorecard(scorecard.Id, month, requestedYear);
                    foreach (var kpi in scorecardKPIs)
                    {
                        ScorecardKPIData kpiData = new ScorecardKPIData();
                        kpiData.KpiId = kpi.Id;
                        kpiData.KpiName = kpi.Name;
                        //get counter measure count for the kpi
                        kpiData.CounterMeasureCount = counterMeasureManager
                            .GetCounterMeasureCount(scorecard.Id, kpi.Id);

                        // Find all targets under this KPI
                        var targets = targetRepository.GetAll().Where(x =>
                            x.ScorecardId == scorecardId && x.KPIId == kpi.Id &&
                            x.CalendarYearId == year.Id && x.IsActive).ToList();

                        foreach (var target in targets)
                        {
                            // Check if metrics are applicable for this month
                            var currentMonthDate = new DateTime(requestedYear, month, 1);
                            //set target effective start date day to first day of the month
                            var targetEffectiveStartDate = new DateTime(target.EffectiveStartDate.Year,
                                target.EffectiveStartDate.Month, 1);
                            //set target effective end date day as last day of the month
                            var targetEffectiveEndDate = new DateTime(target.EffectiveEndDate.Year,
                                target.EffectiveEndDate.Month,
                                DateTime.DaysInMonth(target.EffectiveEndDate.Year, target.EffectiveEndDate.Month));
                            if (currentMonthDate >= targetEffectiveStartDate &&
                                currentMonthDate <= targetEffectiveEndDate)
                            {                                
                                if (target.MetricType == MetricType.Primary)
                                {                                    
                                    kpiData.PrimaryMetricData = GetPrimaryMetricData(target,
                                        requestedYear, month);
                                    if (target.TrackingMethodId == Constants.TrackingMethodDaily)
                                    {
                                        // populate daily targets if not found
                                        actualManager.PopulateDailyTargets(target, month, null);
                                    }
                                }
                                else if (target.MetricType == MetricType.Secondary)
                                {
                                    kpiData.SecondaryMetricsData.Add(GetSecondaryMetricData(target,
                                        month));
                                }

                               
                            }
                        }

                        // Add to KPI List
                        scorecardData.Kpis.Add(kpiData);
                    }

                    scorecardData.DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(scorecard);

                    return scorecardData;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves the metric data belongs to a KPI of a particular scorecard for
        /// an year and month
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="kpiId">KPI Identifier</param>
        /// <param name="yearId">Calendar year Identifier</param>
        /// <param name="month">Month</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// Requested KPI data for a scorecard
        /// </returns>
        public ScorecardKPIData GetScorecardKPIData(int scorecardId, int kpiId, int yearId,
            int month)
        {
            ScorecardKPIData scorecardKPIData = null;
            // Find all targets under this KPI
            var targets = targetRepository.GetAll().Where(x =>
                x.ScorecardId == scorecardId && x.KPIId == kpiId &&
                x.CalendarYearId == yearId && x.IsActive).ToList();

            if (targets.Count > 0)
            {
                scorecardKPIData = new ScorecardKPIData();
                // Find the year corresponding to the month
                var requestedYear = yearRepository.Get(yearId);
                var curMonthItem = CalendarUtility.GetMonthsBetweenDates(requestedYear.StartDate,
                    requestedYear.EndDate).FirstOrDefault(x => x.Id == month);

                foreach (var target in targets)
                {
                    // Check if metrics are applicable for this month
                    var currentMonthDate = new DateTime(curMonthItem.Year, curMonthItem.Id, 1);
                    //set target effective start date day to first day of the month
                    var targetEffectiveStartDate = new DateTime(target.EffectiveStartDate.Year,
                        target.EffectiveStartDate.Month, 1);
                    //set target effective end date day as last day of the month
                    var targetEffectiveEndDate = new DateTime(target.EffectiveEndDate.Year,
                        target.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(target.EffectiveEndDate.Year, target.EffectiveEndDate.Month));
                    if (currentMonthDate >= targetEffectiveStartDate &&
                        currentMonthDate <= targetEffectiveEndDate)
                    {
                        if (target.TrackingMethodId == Constants.TrackingMethodDaily)
                        {
                            // populate daily targets if not found
                            actualManager.PopulateDailyTargets(target, month, null);
                        }

                        if (target.MetricType == MetricType.Primary)
                        {
                            scorecardKPIData.PrimaryMetricData = GetPrimaryMetricData(target,
                                curMonthItem.Year, curMonthItem.Id);
                        }
                        else if (target.MetricType == MetricType.Secondary)
                        {
                            scorecardKPIData.SecondaryMetricsData.Add(
                                GetSecondaryMetricData(target, month));
                        }                        
                    }
                }
            }
            return scorecardKPIData;
        }

        public SecondaryMetricData GetScorecardKPISecondaryMetricData(int kpiId, int targetId, int yearId, int month)
        {
            SecondaryMetricData secondaryMetricDailyData = null;
            var target = targetRepository.Get(targetId);
            if (target != null && target.IsActive)
            {
                secondaryMetricDailyData = new SecondaryMetricData();
                var requestedYear = yearRepository.Get(yearId);

                var curMonthItem = CalendarUtility.GetMonthsBetweenDates(requestedYear.StartDate,
                    requestedYear.EndDate).FirstOrDefault(x => x.Id == month);
                secondaryMetricDailyData = GetSecondaryMetricTrackingData(target, curMonthItem.Year, curMonthItem.Id);
            }
            else if (!target.IsActive)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }

            return secondaryMetricDailyData;
        }
        /// <summary>
        /// Retrieve fiscal month statuses of the primary metric of a KPI for an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>Fiscal month status list</returns>
        public IEnumerable<FiscalMonthStatus> GetFiscalMonthStatusForKPI(int scorecardId,
            int kpiId, int yearId)
        {
            // Declaring the variables
            List<FiscalMonthStatus> fiscalMonthStatusList = new List<FiscalMonthStatus>();

            // Get months list for the current year
            Year year = yearRepository.Get(yearId);
            IEnumerable<MonthItem> monthsList = CalendarUtility.
                GetMonthsBetweenDates(year.StartDate, year.EndDate);
            var primaryMetricTargetList = targetRepository.GetAll()
                   .Where(x => x.ScorecardId == scorecardId &&
                   x.KPIId == kpiId && x.IsActive &&
                   x.CalendarYearId == yearId &&
                   x.MetricType == MetricType.Primary).ToList();
            // Getting primary metric target and finding its actual status for each month
            foreach (var month in monthsList)
            {
                ActualStatus currentStatus = ActualStatus.NotEntered;
                var requestedDate = new DateTime(month.Year, month.Id, 1);
                var activePrimaryMetricTarget = primaryMetricTargetList.FirstOrDefault(x =>
                    requestedDate >= new DateTime(x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1) &&
                    requestedDate <= new DateTime(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)));
                if (activePrimaryMetricTarget != null)
                {
                    currentStatus = mtdCalculator.GetMetricMTDStatus(activePrimaryMetricTarget,
                        month.Year, month.Id);
                }
                var monthlyStatus = new FiscalMonthStatus()
                {
                    Month = month.Id,
                    Status = currentStatus
                };
                //adding to the list
                fiscalMonthStatusList.Add(monthlyStatus);
            }
            return fiscalMonthStatusList.OrderBy(x => x.Month);
        }

        /// <summary>
        /// Retrieve fiscal month status of a scorecard considering only primary metrics for 
        /// all KPI's
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>Fiscal month status list for all KPI's</returns>
        public IEnumerable<KpiFiscalMonthStatus> GetFiscalMonthStatusForScorecard
            (int scorecardId, int yearId)
        {
            List<KpiFiscalMonthStatus> kpiFiscalMonthStatusList = new List<KpiFiscalMonthStatus>();
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard != null)
            {
                foreach (var kpi in scorecard.KPIs)
                {
                    KpiFiscalMonthStatus kpiFiscalMonthStatus = new KpiFiscalMonthStatus();
                    kpiFiscalMonthStatus.FiscalMonthStatusList.AddRange(
                        GetFiscalMonthStatusForKPI(scorecardId, kpi.Id, yearId));
                    kpiFiscalMonthStatus.KpiId = kpi.Id;
                    kpiFiscalMonthStatusList.Add(kpiFiscalMonthStatus);
                }
            }
            return kpiFiscalMonthStatusList;
        }

        /// <summary>
        /// Method to get monthly target 
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="month">month</param>
        /// <returns>monthly target value</returns>
        public decimal? GetMonthlyGoal(int targetId, int month)
        {
            return goalCalculator.GetMonthlyGoal(targetId, month);
        }

        /// <summary>
        /// Method to return scorecard hierarchy with status corresponding to selected scorecard kpi
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="month">month</param>
        /// <param name="yearId">year id</param>
        /// <returns>scorecard hierarchy</returns>
        public ScorecardDrilldownNode GetDrillDownHierarchy(int scorecardId, int kpiId, int month,
            int yearId)
        {
            var scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard == null || !scorecard.IsActive)
            {
                return null;
            }
            var scorecardData = new ScorecardDrilldownNode();
            scorecardData.Id = scorecard.Id;
            scorecardData.Name = scorecard.Name;
            scorecardData.DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(scorecard);
            scorecardData.CanViewScorecard = true;
            scorecardData.IsRootNode = true;
            scorecardData.IsActive = scorecard.IsActive;
            scorecardData.SortOrder = scorecard.SortOrder;
            scorecardData.ExpandTillDrilldownLevel = true;
            scorecardData.DrillDownLevel = 1;
            scorecardData.HasPrimaryCascaded = false;
            scorecardData.HasSecondaryCascaded = false;
            scorecardData.ScorecardStatus = GetKPIDrilldownStatus(scorecardId, kpiId,
                month, yearId);
            scorecardData.Children = AddChildScorecards(scorecard.ChildScorecards, kpiId,
                month, yearId);
            return scorecardData;
        }

        /// <summary>
        /// Method to return scorecard hierarchy with status corresponding to selected scorecard kpi
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>
        /// scorecard hierarchy
        /// </returns>
        public ScorecardDrilldownNode GetDrillDownHierarchy(int scorecardId, int kpiId, DateTime selectedDate)
        {
            var scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard == null || !scorecard.IsActive)
            {
                return null;
            }
            var scorecardData = new ScorecardDrilldownNode();
            scorecardData.Id = scorecard.Id;
            scorecardData.Name = scorecard.Name;
            scorecardData.DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(scorecard);
            scorecardData.CanViewScorecard = true;
            scorecardData.IsRootNode = true;
            scorecardData.SortOrder = scorecard.SortOrder;
            scorecardData.IsActive = scorecard.IsActive;
            scorecardData.ExpandTillDrilldownLevel = true;
            scorecardData.DrillDownLevel = 1;
            scorecardData.HasPrimaryCascaded = false;
            scorecardData.HasSecondaryCascaded = false;
            scorecardData.ScorecardStatus = GetKPIDrilldownStatus(scorecardId, kpiId, selectedDate);
            scorecardData.Children = AddChildScorecards(scorecard.ChildScorecards, kpiId, selectedDate);
            return scorecardData;
        }

        /// <summary>
        ///  Method to retrieve daily goal of a day in a month
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>Daily goal value</returns>
        public decimal? GetDailyGoal(int targetId, int month, int day)
        {
            return goalCalculator.GetDailyGoal(targetId, month, day);
        }

        /// <summary>
        /// Gets the basic scorecard data like scorecard name and kpi owners.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <returns>The scorecard data</returns>
        public ScorecardData GetBasicScorecardData(int scorecardId, int yearId, int month)
        {
            var scorecard = scorecardRepository.Get(scorecardId);
            ValidateScorecardDetails(scorecard, yearId, month);
            var scorecardData = new ScorecardData();
            scorecardData.ScorecardId = scorecard.Id;
            scorecardData.ScorecardName = scorecard.Name;
            //get scorecard kpi owners list 
            scorecardData.KPIOwners = scorecard.KPIOwners.Where(x => x.IsActive)
                .Select(x => x.User.FirstName + " " + x.User.LastName).ToList();

            scorecardData.DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(scorecard);

            var year = yearRepository.Get(yearId);
            // If the current year is present in NDMS year repository
            if (year != null)
            {
                int requestedYear = CalendarUtility.GetYearOfTheMonth(year, month);
                scorecardData.IsPatternAssigned = holidayCalculator.CheckIfPatternIsSetForScorecard(scorecard.Id, month, requestedYear);
            }

            return scorecardData;
        }

        /// <summary>
        /// Method to retrieve scorecard KPI's
        /// </summary>
        /// <param name="scorecardId">identifier of scorecard</param>
        /// <param name="yearId">identifier of calendar year</param>
        /// <returns>List of KPI's</returns>
        public List<KPIItem> GetScorecardKPIs(int scorecardId, int yearId)
        {
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard != null)
            {
                var scorecardKPIs = new List<KPIItem>();
                foreach (var kpi in scorecard.KPIs)
                {
                    scorecardKPIs.Add(new KPIItem()
                    {
                        Id = kpi.Id,
                        Name = kpi.Name,
                    });
                }
                return scorecardKPIs;
            }
            return null;
        }

        // <summary>
        /// <summary>
        /// Gets the scorecard KPIs of given month.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <returns>List of KPIs</returns>
        public List<KPIItem> GetScorecardKPIs(int scorecardId, int month, int yearId)
        {
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            IList<KPI> scorecardKPIs = GetScorecardKPIsByMonth(scorecard, month, yearId);
            List<KPIItem> scorecardKPIsOfMonth = new List<KPIItem>();
            scorecardKPIs.ToList().ForEach(kpi =>
            {
                scorecardKPIsOfMonth.Add(new KPIItem
                {
                    Id = kpi.Id,
                    Name = kpi.Name
                });
            });

            return scorecardKPIsOfMonth;
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
                    if (monthlyTargetRepository != null)
                    {
                        monthlyTargetRepository.Dispose();
                    }
                    if (targetRepository != null)
                    {
                        targetRepository.Dispose();
                    }
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    if (scorecardRepository != null)
                    {
                        scorecardRepository.Dispose();
                    }
                    if (counterMeasureManager != null)
                    {
                        counterMeasureManager.Dispose();
                    }
                    
                    if(recordableRepository != null)
                    {
                        recordableRepository.Dispose();
                    }
                    if(scorecardWorkdayTrackerRepository != null)
                    {
                        scorecardWorkdayTrackerRepository.Dispose();
                    }

                    if(metricRepository != null)
                    {
                        metricRepository.Dispose();
                    }
                    // Assign references to null
                    dailyActualRepository = null;
                    monthlyActualRepository = null;
                    targetRepository = null;
                    yearRepository = null;
                    scorecardRepository = null;
                    monthlyTargetRepository = null;
                    counterMeasureManager = null;
                    recordableRepository = null;
                    scorecardWorkdayTrackerRepository = null;
                    metricRepository = null;
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
