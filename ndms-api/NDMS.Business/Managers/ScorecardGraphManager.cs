using NDMS.Business.Common;
using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Managers
{
    public class ScorecardGraphManager : IScorecardGraphManager
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
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Get Monthly Graph data for the Primary Metric based on the 
        /// Graph Plotting method selected for that Metric
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="months">months in the calendar year</param>
        /// <returns>Monthly list of Actuals and Goals</returns>
        private List<MonthlyGraphData> GetMonthlyGraphData(Target target,
            IEnumerable<MonthItem> monthList)
        {
            List<MonthlyGraphData> monthlyGraphData = new List<MonthlyGraphData>();

            //get monthly targets which falls in the month list and actuals 
            //corresponding to the target
            var monthlyData = target.MonthlyTargets.Where(x =>
               x.MaxGoalValue.HasValue && monthList.Any(d => d.Id == x.Month))
               .Select(x => new MonthlyGraphData()
               {
                   Month = x.Month,
                   GoalValue = x.MaxGoalValue,
                   StretchGoalValue = x.StretchGoalValue,
                   ActualValue = monthlyActualRepository.GetAll().FirstOrDefault(y =>
                                 y.TargetId == target.Id && y.Month == x.Month
                                 && y.ActualValue.HasValue)?.ActualValue

               }).ToList();
            monthlyGraphData.AddRange(monthlyData);

            //Fill the remaining months with null values
            if (monthlyGraphData != null && monthlyGraphData.Count > 0)
            {
                foreach (var month in monthList)
                {
                    if (!monthlyGraphData.Any(x => x.Month == month.Id))
                    {
                        monthlyGraphData.Add(new MonthlyGraphData()
                        {
                            Month = month.Id
                        });
                    }
                }
            }
            else
            {
                return null;
            }

            monthlyGraphData = monthlyGraphData.OrderBy(o => o.Month).ToList();
            //If the graph plotting method is cumulative sum up the actual 
            //and goal value for each month
            if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
            {
                var latestActualEntry = monthlyGraphData.LastOrDefault(x => x.ActualValue.HasValue);
                var latestGoalEntry = monthlyGraphData.LastOrDefault(x => x.GoalValue.HasValue);
                decimal? cumulativeActualValue = 0;
                decimal? cumulativeGoalValue = 0;
                decimal? cumulativeStretchGoalValue = 0;
                foreach (var month in monthlyGraphData)
                {
                    //if actual value of a month is having a value it will sum up the value 
                    //with the cumulative actual value, else return cumulative actual value 
                    if (latestActualEntry != null && month.Month <= latestActualEntry.Month)
                    {
                        if (month.ActualValue.HasValue)
                        {
                            month.ActualValue = cumulativeActualValue =
                                cumulativeActualValue + month.ActualValue;
                        }
                    }
                    //if goal value of a month is having a value it will sum up the value 
                    //with the cumulative goal value, else return cumulative goal value 
                    if (latestGoalEntry != null && month.Month <= latestGoalEntry.Month)
                    {
                        if (month.GoalValue.HasValue)
                        {
                            month.GoalValue = cumulativeGoalValue =
                                cumulativeGoalValue + month.GoalValue;
                        }

                        //if stretch goal value of a day is having a value it will sum up the value 
                        //with the cumulative stretch goal value, else return cumulative stretch goal value
                        if (month.StretchGoalValue.HasValue)
                        {
                            month.StretchGoalValue = cumulativeStretchGoalValue =
                                cumulativeStretchGoalValue + month.StretchGoalValue;
                        }
                    }
                }
            }

            return monthlyGraphData;
        }

        /// <summary>
        /// Get Daily Graph data for the Primary Metric based on the 
        /// Graph Plotting method selected for that Metric
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="month">selected month</param>
        /// <param name="daysInMonth">days in the selected month</param>
        /// <returns>Daily list of Actuals and Goals</returns>
        private List<DailyGraphData> GetDailyGraphData(Target target,
            int month, int requestedYear)
        {
            IEnumerable<int> daysInMonth = CalendarUtility
                        .GetAllDaysInMonth(requestedYear, month);

            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            List<DailyGraphData> dailyGraphData = new List<DailyGraphData>();

            //return empty graph data if monthly target is null for the month
            if (!target.MonthlyTargets.Any(x => x.Month == month))
            {
                return null;
            }

            //iterate through days in a month to set actual and goal value
            foreach (int day in daysInMonth)
            {
                DailyTargetData dailyTargetData = goalCalculator.
                    GetDailyGoalAndStretchGoal(target, month, day);                

                dailyGraphData.Add(new DailyGraphData()
                {
                    Day = day,
                    ActualValue = dailyActualRepository.GetAll().FirstOrDefault(y =>
                                  y.TargetId == target.Id && y.Date.Day == day &&
                                  y.Date.Month == month && y.ActualValue.HasValue)?.ActualValue,
                    GoalValue = dailyTargetData?.GoalValue,
                    StretchGoalValue = dailyTargetData?.StretchGoalValue
                });                
            }
            //If the graph plotting method is cumulative sum up the actual 
            //and stretch goal value for each day
            if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
            {

                var latestActualEntry = dailyActualRepository.GetAll().OrderByDescending(x => x.Date)
                    .FirstOrDefault(y => y.TargetId == target.Id && y.Date.Month == month &&
                    y.ActualValue.HasValue &&
                    y.Status != ActualStatus.Holiday);
                DateTime lastWorkingDay = goalCalculator.GetLastWorkingDayOfMonthForTarget(target, month, requestedYear);
                var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == month);
                decimal? cumulativeActualValue = 0;
                decimal? cumulativeStretchGoalValue = 0;
                foreach (var day in dailyGraphData)
                {
                    //if actual value of a day is having a value it will sum up the value 
                    //with the cumulative actual value, else return cumulative actual value 
                    if (latestActualEntry != null && day.Day <= latestActualEntry.Date.Day)
                    {
                        if (day.ActualValue.HasValue)
                        {
                            day.ActualValue = cumulativeActualValue =
                                cumulativeActualValue + day.ActualValue;
                        }
                    }

                    //if stretch goal value of a day is having a value it will sum up the value 
                    //with the cumulative stretch goal value, else return cumulative stretch goal value 
                    if (day.StretchGoalValue.HasValue)
                    {
                        day.StretchGoalValue = cumulativeStretchGoalValue =
                            cumulativeStretchGoalValue + day.StretchGoalValue;
                    }

                    if (target.Metric.DataTypeId == Constants.DataTypeWholeNumber)
                    {
                        day.ActualValue = day.ActualValue.HasValue ?
                            Math.Round(day.ActualValue.Value, 0, MidpointRounding.AwayFromZero) : day.ActualValue;
                        day.StretchGoalValue = (day.StretchGoalValue.HasValue) ?
                            Math.Round(day.StretchGoalValue.Value, 0, MidpointRounding.AwayFromZero) : day.StretchGoalValue;
                    }

                    // set the Stretch Goal value of last working day of month as the monthly Stretch Goal value if target entry is monthly.
                    if(day.Day == lastWorkingDay.Day && (monthlyTarget?.MaxGoalValue.HasValue ?? false))
                    {
                        day.StretchGoalValue = monthlyTarget?.StretchGoalValue;
                    }
                }
            }
            else
            {
                if (target.Metric.DataTypeId == Constants.DataTypeWholeNumber)
                {                    
                    dailyGraphData.ForEach(x => {
                        x.StretchGoalValue = (x.StretchGoalValue.HasValue) ?
                            Math.Round(x.StretchGoalValue.Value, 0, MidpointRounding.AwayFromZero) : x.StretchGoalValue;
                        x.ActualValue = x.ActualValue.HasValue ?
                            Math.Round(x.ActualValue.Value, 0, MidpointRounding.AwayFromZero) : x.ActualValue;
                        x.GoalValue = x.GoalValue.HasValue ?
                            Math.Round(x.GoalValue.Value, 0, MidpointRounding.AwayFromZero) : x.GoalValue;
                    });
                }
            }

            return dailyGraphData;
        }

        /// <summary>
        /// Method to get min value of actual, goal and stretch goal from monthly graph data
        /// </summary>
        /// <param name="kpiGraphData">graph data</param>
        /// <returns>graph data with the values set</returns>
        private KPIGraphData GetMinandMaxValueForMonthlyGraphData(KPIGraphData kpiGraphData)
        {
            //get min values of actual, goal and stretch goal
            List<decimal?> minValues = new List<decimal?>();
            minValues.Add(kpiGraphData.MonthlyGraphData.Min(x => x.ActualValue));
            minValues.Add(kpiGraphData.MonthlyGraphData.Min(x => x.GoalValue));
            minValues.Add(kpiGraphData.MonthlyGraphData.Min(x => x.StretchGoalValue));
            //get max values of actual, goal and stretch goal
            List<decimal?> maxValues = new List<decimal?>();
            maxValues.Add(kpiGraphData.MonthlyGraphData.Max(x => x.ActualValue));
            maxValues.Add(kpiGraphData.MonthlyGraphData.Max(x => x.GoalValue));
            maxValues.Add(kpiGraphData.MonthlyGraphData.Max(x => x.StretchGoalValue));

            //set min and max among actual, goal and stretch goal to graph data
            kpiGraphData.MinValue = minValues.Min();
            kpiGraphData.MaxValue = maxValues.Max();

            return kpiGraphData;
        }

        /// <summary>
        /// Method to get min value of actual and goal from daily graph data
        /// </summary>
        /// <param name="kpiGraphData">graph data</param>
        /// <returns>graph data with the values set</returns>
        private KPIGraphData GetMinandMaxValueForDailyGraphData(KPIGraphData kpiGraphData)
        {
            //get min values of actual and goal
            List<decimal?> minValues = new List<decimal?>();
            minValues.Add(kpiGraphData.DailyGraphData.Min(x => x.ActualValue));
            minValues.Add(kpiGraphData.DailyGraphData.Min(x => x.GoalValue));
            minValues.Add(kpiGraphData.DailyGraphData.Min(x => x.StretchGoalValue));
            //get max values of actual and goal
            List<decimal?> maxValues = new List<decimal?>();
            maxValues.Add(kpiGraphData.DailyGraphData.Max(x => x.ActualValue));
            maxValues.Add(kpiGraphData.DailyGraphData.Max(x => x.GoalValue));
            maxValues.Add(kpiGraphData.DailyGraphData.Max(x => x.StretchGoalValue));
            //set min and max among actual, goal and stretch goal to graph data
            kpiGraphData.MinValue = minValues.Min();
            kpiGraphData.MaxValue = maxValues.Max();

            return kpiGraphData;
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
        /// <param name="holidayRepository">Holiday repository</param>
        public ScorecardGraphManager(IScorecardRepository scorecardRepository,
            IBaseRepository<Target> targetRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (targetRepository == null || scorecardRepository == null ||
                yearRepository == null || dailyActualRepository == null ||
                monthlyActualRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.scorecardRepository = scorecardRepository;
            this.yearRepository = yearRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository,  scorecardHolidayPatternRepository, 
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieve graph data of the primary metric of a KPI for a month in an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <param name="month">Month</param>
        /// <returns>Graph data</returns>
        public KPIGraphData GetScorecardKPIGraphData(int scorecardId,
            int kpiId, int yearId, int month)
        {
            KPIGraphData kpiGraphData = null;

            //get the year in which month belongs
            Year year = yearRepository.Get(yearId);
            int requestedYear = CalendarUtility.GetYearOfTheMonth(year, month);
            DateTime requestDate = new DateTime(requestedYear, month, 1);
            // Get the primary metrics active for a KPI in a calendar year
            var primaryMetricTargets = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecardId && x.KPIId == kpiId && x.CalendarYearId == yearId &&
            x.MetricType == MetricType.Primary && x.IsActive);
            //get the metric which is active in the selected month by 
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var primaryMetricTarget = primaryMetricTargets.ToList().FirstOrDefault(x =>
            requestDate >= new DateTime(x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
            && requestDate <= new DateTime(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
            DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)));

            if (primaryMetricTarget != null)
            {
                kpiGraphData = new KPIGraphData();
                kpiGraphData.KpiId = kpiId;
                //if metrics tracking method is monthly, get the monthly graph details
                if (primaryMetricTarget.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    IEnumerable<MonthItem> monthsInYear = CalendarUtility.
                        GetMonthsBetweenDates(year.StartDate, year.EndDate);
                    kpiGraphData.MonthlyGraphData = GetMonthlyGraphData(primaryMetricTarget, monthsInYear);
                    if (kpiGraphData.MonthlyGraphData != null)
                    {
                        kpiGraphData = GetMinandMaxValueForMonthlyGraphData(kpiGraphData);
                    }
                }
                //else get daily graph details
                else
                {
                    kpiGraphData.DailyGraphData = GetDailyGraphData(
                        primaryMetricTarget, month, requestedYear);
                    if (kpiGraphData.DailyGraphData != null)
                    {
                        kpiGraphData = GetMinandMaxValueForDailyGraphData(kpiGraphData);
                    }
                }
            }

            return kpiGraphData;
        }

        /// <summary>
        /// Retrieve graph data of the primary metric of all KPIs in a scorecard
        /// for a month in an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <param name="month">Month</param>
        /// <returns>Graph data</returns>
        public IEnumerable<KPIGraphData> GetScorecardGraphData(int scorecardId,
            int yearId, int month)
        {
            List<KPIGraphData> scorecardKPIsGraphData = new List<KPIGraphData>();
            Scorecard scorecard = scorecardRepository.Get(scorecardId);
            if (scorecard != null)
            {
                foreach (var kpi in scorecard.KPIs)
                {
                    KPIGraphData kpiGraphData = GetScorecardKPIGraphData(scorecardId,
                        kpi.Id, yearId, month);
                    scorecardKPIsGraphData.Add(kpiGraphData);
                }
            }

            return scorecardKPIsGraphData;
        }

        /// <summary>
        /// Gets the scorecard kpi graph data for selected metric.
        /// </summary>
        /// <param name="metricTargetId">The metric target identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="month">The month.</param>
        /// <returns>
        /// The graph data
        /// </returns>
        public KPIGraphData GetScorecardMetricKPIGraphData(int metricTargetId, int kpiId, int yearId, int month)
        {
            KPIGraphData graphData = null;
            //get the year in which month belongs
            Year year = yearRepository.Get(yearId);
            int requestedYear = CalendarUtility.GetYearOfTheMonth(year, month);
            DateTime requestDate = new DateTime(requestedYear, month, 1);
            // Get the metric target details for a KPI in a calendar year
            var metricTarget = targetRepository.Get(metricTargetId);

            if (metricTarget != null && metricTarget.IsActive && metricTarget.GraphPlottingMethod != null)
            {                
                graphData = new KPIGraphData();
                graphData.KpiId = kpiId;
                //if metrics tracking method is monthly, get the monthly graph details
                if (metricTarget.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    IEnumerable<MonthItem> monthsInYear = CalendarUtility.
                        GetMonthsBetweenDates(year.StartDate, year.EndDate);
                    graphData.MonthlyGraphData = GetMonthlyGraphData(metricTarget, monthsInYear);
                    if (graphData.MonthlyGraphData != null)
                    {
                        graphData = GetMinandMaxValueForMonthlyGraphData(graphData);
                    }
                }
                //else get daily graph details
                else if(metricTarget.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    graphData.DailyGraphData = GetDailyGraphData(
                        metricTarget, month, requestedYear);
                    if (graphData.DailyGraphData != null)
                    {
                        graphData = GetMinandMaxValueForDailyGraphData(graphData);
                    }
                }
            }

            return graphData;
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
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    if (scorecardRepository != null)
                    {
                        scorecardRepository.Dispose();
                    }

                    // Assign references to null
                    dailyActualRepository = null;
                    monthlyActualRepository = null;
                    targetRepository = null;
                    yearRepository = null;
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
