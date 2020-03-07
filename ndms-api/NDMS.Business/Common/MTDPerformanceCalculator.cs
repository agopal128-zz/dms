using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Linq;

namespace NDMS.Business.Common
{
    public class MTDPerformanceCalculator
    {
        #region Field(s)
        /// <summary>
        /// Reference to ScorecardGoalCalculator
        /// </summary>
        private ScorecardGoalCalculator goalCalculator;

        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        ///<summary>
        /// Reference to Holiday Calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        #endregion

        #region Private Method(s)        

        /// <summary>
        /// Gets the last day with target for current month.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="monthlyTarget">The monthly target.</param>
        private int GetLastDayWithTargetForCurrentMonth(Target target, MonthlyTarget monthlyTarget)
        {
            bool isRolledUpTarget = false;
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var day = currentDate.Day;
            DailyTarget dailyTarget = new DailyTarget();
            if (target.CascadedMetricsTrackingMethodId.HasValue)
            {
                // check whether cascaded metrics tracking method is rolled up targets
                isRolledUpTarget = (target.CascadedMetricsTrackingMethodId.Value == (int)CascadedMetricsTrackingMethod.RolledUpTargets && target.IsCascaded);
            }
            if (isRolledUpTarget)
            {
                dailyTarget = monthlyTarget.DailyTargets
                                                   .Where(x => x.Day <= currentDate.Day && (x.RolledUpGoalValue != null))
                                                   .OrderByDescending(x => x.Day)
                                                   .FirstOrDefault();
            }
            else
            {
                dailyTarget = monthlyTarget.DailyTargets
                                                  .Where(x => x.Day <= currentDate.Day && (x.MaxGoalValue != null))
                                                  .OrderByDescending(x => x.Day)
                                                  .FirstOrDefault();
            }
            if (dailyTarget != null)
            {
                day = dailyTarget.Day;
            }
            return day;
        }

        /// <summary>
        /// Gets the last day with target for previous month.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="monthlyTarget">The monthly target.</param>
        /// <param name="yearId">The year.</param>
        private int GetLastDayWithTargetForPreviousMonth(Target target, MonthlyTarget monthlyTarget, int yearId)
        {
            bool isRolledUpTarget = false;
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var day = currentDate.Day;
            DailyTarget dailyTarget = new DailyTarget();
            if (target.CascadedMetricsTrackingMethodId.HasValue)
            {
                // check whether cascaded metrics tracking method is rolled up targets
                isRolledUpTarget = (target.CascadedMetricsTrackingMethodId.Value == (int)CascadedMetricsTrackingMethod.RolledUpTargets && target.IsCascaded);
            }
            if (isRolledUpTarget)
            {
                dailyTarget = monthlyTarget.DailyTargets
                              .Where(x => x.RolledUpGoalValue != null)
                              .OrderByDescending(x => x.Day)
                              .FirstOrDefault();
            }
            else
            {
                dailyTarget = monthlyTarget.DailyTargets
                           .Where(x => x.MaxGoalValue != null)
                           .OrderByDescending(x => x.Day)
                           .FirstOrDefault();
            }

            if (dailyTarget != null)
            {
                day = dailyTarget.Day;
            }
            else
            {
                day = holidayCalculator.GetLastWorkingDayOfMonthForTarget(target, monthlyTarget.Month, yearId).Day;
            }
            return day;
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
                holidayCalculator = new HolidayCalculator(
                                dailyActualRepository,
                                scorecardHolidayPatternRepository,
                                scorecardWorkdayPatternRepository, 
                                scorecardWorkdayTrackerRepository);
            }
            return holidayCalculator;
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
        /// <param name="holidayRepository">Holiday repository</param>
        /// <param name="counterMeasureManager">Counter measure manager</param>
        public MTDPerformanceCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository
            )
        {
            if (targetRepository == null || dailyActualRepository == null ||                
                scorecardWorkdayTrackerRepository == null || scorecardHolidayPatternRepository == null||
                scorecardWorkdayPatternRepository == null || monthlyActualRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }

            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.holidayCalculator = CreateHolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository
                , scorecardWorkdayTrackerRepository, scorecardWorkdayPatternRepository);
            this.goalCalculator = CreateScorecardGoalCalculator(targetRepository,
                dailyActualRepository, scorecardWorkdayPatternRepository, scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository);
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// Get Month To date performance to determine month Color
        /// </summary>
        /// <param name="target"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public MonthToDatePerformanceItem GetMonthToDatePerformance(Target target, int year, int month)
        {
            if(target.MTDPerformanceTrackingMethodId ==null)
            {

            }
            var mTDPerformance = new MonthToDatePerformanceItem
            {
                Year = year,
                Month = month,
                Status = ActualStatus.NotEntered
            };

            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var day = currentDate.Day;

            if ((month > currentDate.Month && currentDate.Year == year) || year > currentDate.Year)
            {
                return mTDPerformance;
            }
            else if((currentDate.Month > month && currentDate.Year == year) || (currentDate.Year > year))
            {
                day = holidayCalculator.GetLastWorkingDayOfMonthForTarget(target, month, year).Day;
            }

            var selectedDate = new DateTime(year, month, day).Date;

            var dailyActualsList = dailyActualRepository.GetAll()
                                                    .Where(x =>
                                                    x.TargetId == target.Id
                                                    && x.Date.Month == month && x.ActualValue != null
                                                    && x.Status != ActualStatus.Holiday)
                                                    ?.ToList();
            
            if (dailyActualsList.Any())
            {
                var latestActualDate = dailyActualsList.OrderBy(x => x.Date).LastOrDefault().Date;

                switch ((MTDPerformanceTrackingMethod)target.MTDPerformanceTrackingMethodId)
                {
                    case MTDPerformanceTrackingMethod.Cumulative:
                        {
                            mTDPerformance.GoalValue = goalCalculator.CalculateCumulativeGoal(target, latestActualDate);
                            mTDPerformance.ActualValue = dailyActualsList.Sum(x => x.ActualValue);
                        }
                        break;
                    case MTDPerformanceTrackingMethod.Average:
                        {
                            mTDPerformance.GoalValue = goalCalculator.CalculateAverageMTDGoal(target, selectedDate.Month);
                            mTDPerformance.ActualValue = Math.Round(dailyActualsList.Average(x => x.ActualValue).Value, 2, MidpointRounding.AwayFromZero);

                            if (target.Metric.DataTypeId == Constants.DataTypeWholeNumber)
                            {
                                mTDPerformance.ActualValue = Math.Round(mTDPerformance.ActualValue.Value, MidpointRounding.AwayFromZero);
                            }
                        }
                        break;
                    case MTDPerformanceTrackingMethod.Latest:
                        {
                            mTDPerformance.GoalValue = dailyActualsList?.OrderBy(x => x.Date).LastOrDefault(x => x.GoalValue != null)?.GoalValue;
                            if (!mTDPerformance.GoalValue.HasValue)
                            {
                                mTDPerformance.GoalValue = target.IsCascaded && target.CascadedMetricsTrackingMethodId.Value == (int)CascadedMetricsTrackingMethod.RolledUpTargets ?
                                            target.MonthlyTargets?.FirstOrDefault(x => x.Month == month)?.DailyTargets?.OrderBy(d => d.Day).LastOrDefault(y => y.Day <= latestActualDate.Day)?.RolledUpGoalValue :
                                            target.MonthlyTargets?.FirstOrDefault(x => x.Month == month)?.DailyTargets?.OrderBy(d => d.Day).LastOrDefault(y => y.Day <= latestActualDate.Day)?.MaxGoalValue;
                            }

                            mTDPerformance.ActualValue = dailyActualsList?.FirstOrDefault(x => x.Date == latestActualDate.Date)?.ActualValue;
                        }
                        break;
                    default: break;
                }
                //set status if actual and goal has values
                if (mTDPerformance.ActualValue.HasValue && mTDPerformance.GoalValue.HasValue)
                {
                    mTDPerformance.Status = TargetActualComparer.GetActualStatus(mTDPerformance.GoalValue, mTDPerformance.ActualValue, target.Metric.GoalTypeId);
                }
            }
            return mTDPerformance;

        }

        /// <summary>
        /// Getting MTD status for a metric
        /// </summary>
        /// <param name="target">Target whose status needs to be find out</param>
        /// <param name="year">Year for which status is required</param>
        /// <param name="month">Month for which status is required</param>
        /// <returns>Drill down status of metric</returns>
        public ActualStatus GetMetricMTDStatus(Target
            target, int year, int month)
        {
            ActualStatus status = ActualStatus.NotEntered;
            var primaryMetricTargetId = target?.Id;
            if (primaryMetricTargetId.HasValue)
            {
                int trackingMethod = target.TrackingMethodId;
                int? mtdPerformanceTrackingId = target.MTDPerformanceTrackingMethodId;
                if (trackingMethod == Constants.TrackingMethodDaily && mtdPerformanceTrackingId.HasValue)
                {
                    status = GetMonthToDatePerformance(target, year, month).Status;
                }
                else if (trackingMethod == Constants.TrackingMethodMonthly)
                {
                    var monthlyActual = monthlyActualRepository.GetAll()
                       .FirstOrDefault(x => x.TargetId == primaryMetricTargetId.Value && x.Month == month
                       && (x.Status == ActualStatus.Achieved || x.Status == ActualStatus.NotAchieved));
                    if (monthlyActual != null)
                    {
                        status = monthlyActual.Status;
                    }
                }
            }
            return status;
        }


        /// <summary>
        /// Get Month To date performance to determine Goal Value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public decimal? GetMonthToDateGoalValue(Target target, int year, int monthId)
        {
            decimal? goal = null;
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var day = currentDate.Day;
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.TargetId == target.Id && x.Month == monthId);
            if (monthlyTarget != null)
            {
                if (!target.MTDPerformanceTrackingMethodId.HasValue)
                {
                    return monthlyTarget.MaxGoalValue;
                }

                switch ((MTDPerformanceTrackingMethod)target.MTDPerformanceTrackingMethodId)
                {
                    case MTDPerformanceTrackingMethod.Cumulative:
                        {
                            goal = goalCalculator.CalculateCumulativeMTDGoal(target, monthId);
                        }
                        break;
                    case MTDPerformanceTrackingMethod.Average:
                        {
                            goal = goalCalculator.CalculateAverageMTDGoal(target, monthId);

                        }
                        break;
                    case MTDPerformanceTrackingMethod.Latest:
                        {
                            goal = goalCalculator.GetDailyMTDGoal(target, monthId);
                        }
                        break;

                    default: break;
                }
                if (goal.HasValue)
                {
                    if (target.Metric.DataTypeId == Constants.DataTypeWholeNumber)
                    {
                        goal = Math.Round(goal.Value, 0, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        goal = Math.Round(goal.Value, 2, MidpointRounding.AwayFromZero);
                    }
                }

            }
            return goal;

        }

        #endregion
    }
}
