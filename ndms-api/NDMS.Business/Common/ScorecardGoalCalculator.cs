using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.DomainModel.Enums;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Common
{
    /// <summary>
    /// Implements methods which helps to calculate daily/monthly goals
    /// </summary>
    public class ScorecardGoalCalculator
    {
        #region Field(s)
        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        ///<summary>
        ///Holiday Calculator
        /// </summary>
        private HolidayCalculator holidayCalculator;

        #endregion

        #region Private Method(s)
        /// <summary>
        /// Method to calculate daily target 
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="selectedDate">Selected date</param>
        /// <returns>Daily goal</returns>
        private decimal? GetDailyGoal(Target target, DateTime selectedDate)
        {
            decimal? dailyTargetValue = null;

            // Get monthly target
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
            x => x.Month == selectedDate.Month);

            bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, selectedDate.Month);

            //check if monthly target has value
            if (monthlyTarget != null)
            {
                //check if any daily targets is entered for the month
                if (monthlyTarget.DailyTargets?.Count > 0)
                {
                    //get daily target for the selected day
                    var dailyTarget = monthlyTarget.DailyTargets
                        .FirstOrDefault(x => x.Day == selectedDate.Day);

                    //if daily target is present return the daily entered target value/rolled up target
                    if (dailyTarget != null)
                    {
                        dailyTargetValue = isRolledUpTarget ?
                                           dailyTarget.RolledUpGoalValue :
                                           dailyTarget.MaxGoalValue;
                    }
                }

                else
                {
                    // code for backward compatibility
                    var dailyActual = dailyActualRepository.GetAll().FirstOrDefault(x => x.TargetId == target.Id && x.Date == selectedDate && x.ActualValue != null);
                    if (dailyActual != null)
                    {
                        dailyTargetValue = dailyActual.GoalValue;
                    }
                    else
                    {
                        dailyTargetValue = CalculateDailyGoal(target, monthlyTarget, selectedDate);
                        if (dailyTargetValue.HasValue)
                        {
                            dailyTargetValue = Math.Round(dailyTargetValue.Value, 2, MidpointRounding.AwayFromZero);
                        }
                    }
                }
            }

            return dailyTargetValue;
        }

        /// <summary>
        /// Calculates daily goal considering metric datatype, total days in month and
        /// total number of holidays
        /// </summary>
        /// <param name="target">Corresponding Target entity</param>
        /// <param name="monthlyTarget">Corresponding Monthly target entity</param>
        /// <param name="selectedDate">Day for which goal is required</param>
        /// <returns>Calculated goal</returns>
        private decimal? CalculateDailyGoal(Target target, MonthlyTarget monthlyTarget,
            DateTime selectedDate)
        {
            decimal? dailyTargetValue = null;
            // If metric data type is whole number/amount/decimal number get the goal value by 
            // dividing monthly target by no of remaining working days
            if (target.Metric.DataTypeId != Constants.DataTypePercentage)
            {
                int year = selectedDate.Year;
                int month = selectedDate.Month;
                decimal sumOfTargetsInDailyActual = 0;
                int countActualEnteredTillDate = 0;
                //set start date and end date for the month as the start and end date can be 
                //any date in the month
                DateTime monthStartDate = new DateTime(year, month, 1);
                DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth
                    (year, month));
                if (target.EffectiveStartDate > monthStartDate)
                {
                    monthStartDate = target.EffectiveStartDate;
                }
                if (target.EffectiveEndDate < monthEndDate)
                {
                    monthEndDate = target.EffectiveEndDate;
                }

                if (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
                {
                    var dailyTargetDays = monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue.HasValue).Select(x => x.Day);
                    if (dailyTargetDays?.Count() > 0)
                    {
                        // Count of actual entries till date
                        countActualEnteredTillDate = dailyActualRepository.GetAll()
                                              .Where(x => x.TargetId == target.Id && x.Date <= monthEndDate && x.Date >= monthStartDate && x.ActualValue != null && !dailyTargetDays.Any(y => y == selectedDate.Day))?.Count() ?? 0;

                        if (countActualEnteredTillDate > 0)
                        {
                            // Get the sum of goal value entered for previous dates, excluding the daily targets
                            sumOfTargetsInDailyActual = dailyActualRepository.GetAll()
                                                        .Where(x => x.TargetId == target.Id && x.Date <= monthEndDate && x.Date >= monthStartDate && !dailyTargetDays.Any(y => y == selectedDate.Day))
                                                        .Sum(x => x.GoalValue) ?? 0;
                        }
                    }
                    else
                    {
                        // Count of actual entries till date
                        countActualEnteredTillDate = dailyActualRepository.GetAll()
                                              .Where(x => x.TargetId == target.Id && x.Date <= monthEndDate && x.Date >= monthStartDate && x.ActualValue != null)?.Count() ?? 0;

                        if (countActualEnteredTillDate > 0)
                        {
                            // Get the sum of goal value entered for previous dates, excluding the daily targets
                            sumOfTargetsInDailyActual = dailyActualRepository.GetAll()
                                                        .Where(x => x.TargetId == target.Id && x.Date <= monthEndDate && x.Date >= monthStartDate)
                                                        .Sum(x => x.GoalValue) ?? 0;
                        }
                    }
                }

                //Get total holidays between the dates
                int totalNumberofHolidays = holidayCalculator.CountHolidaysBetweenDaysOfMonth(target.Id,
                    target.ScorecardId, monthStartDate, monthEndDate);

                //get count total effective days in a month 
                int totalDaysInMonth = (int)(monthEndDate - monthStartDate).TotalDays + 1;

                // Calculate the remaining days by subtracting the sum of holidays and days
                // for which daily targets are entered
                // and the actual entered days from the total days in a month
                int? remainingDays = (totalDaysInMonth) - (totalNumberofHolidays
                    + monthlyTarget.DailyTargets?.Count(x => x.MaxGoalValue.HasValue) + countActualEnteredTillDate);

                if (remainingDays > 0)
                {
                    // Get the sum of target of already entered days
                    var sumOfDailyTargets = monthlyTarget.DailyTargets.Sum(x => x.MaxGoalValue);
                    dailyTargetValue = (monthlyTarget.MaxGoalValue - sumOfDailyTargets - sumOfTargetsInDailyActual)
                           / remainingDays;
                }
            }
            // Daily goal is same as monthly goal in case metric data type is percentage
            else
            {
                dailyTargetValue = monthlyTarget.MaxGoalValue;
            }
            return dailyTargetValue;
        }

        /// <summary>
        /// Distribute whole number GoalValue Evenly among workdays
        /// </summary>
        /// <param name="dailyTargetList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<DailyTargetItem> DistributeMonthlyGoalAsWholeNumbers(List<DailyTargetItem> dailyTargetList, int monthlyGoalValue)
        {
            var workDaysList = dailyTargetList.Where(x => !(x.IsHoliday || x.IsOutofRange));
            int noOfWorkDays = workDaysList.Count();
            if (noOfWorkDays > 0)
            {
                int perDayWholeNumberGoal = monthlyGoalValue / noOfWorkDays;
                decimal perDayDecimalGoal = ((decimal)monthlyGoalValue) / noOfWorkDays;
                decimal perDaySurplusGoal = perDayDecimalGoal - perDayWholeNumberGoal;
                decimal cumulatedGoalValue = 0;

                workDaysList.ToList().ForEach(x =>
                {
                    cumulatedGoalValue = cumulatedGoalValue + perDaySurplusGoal;
                    x.GoalValue = (int)(perDayWholeNumberGoal + cumulatedGoalValue);
                    cumulatedGoalValue = cumulatedGoalValue >= 1 ? cumulatedGoalValue - 1 : cumulatedGoalValue;
                });

                var sumAfterDistribution = (int)workDaysList.Sum(x => x.GoalValue);
                if (sumAfterDistribution > monthlyGoalValue)
                {
                    int diff = (sumAfterDistribution - monthlyGoalValue);
                    while (diff > 0)
                    {
                        workDaysList.OrderBy(x => x.Day).FirstOrDefault(x => x.GoalValue == perDayWholeNumberGoal + 1).GoalValue--;
                        diff--;
                    }
                }
                else if (sumAfterDistribution < monthlyGoalValue)
                {
                    int diff = (monthlyGoalValue - sumAfterDistribution);
                    while (diff > 0)
                    {
                        workDaysList.OrderBy(x => x.Day).LastOrDefault(x => x.GoalValue == perDayWholeNumberGoal).GoalValue++;
                        diff--;
                    }
                }
            }
            return dailyTargetList;
        }

        /// <summary>
        /// Distribute decimal GoalValue Evenly among workdays
        /// </summary>
        /// <param name="dailyTargetList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private List<DailyTargetItem> DistributeMonthlyGoalAsDecimal(List<DailyTargetItem> dailyTargetList, decimal goalValue)
        {
            int sign = 1;
            if (goalValue < 0)
            {
                sign = -1;
            }

            dailyTargetList = DistributeMonthlyGoalAsWholeNumbers(dailyTargetList, (int)Math.Abs(goalValue * 100));
            dailyTargetList.ForEach(x =>
            {
                x.GoalValue = sign * x.GoalValue / 100;
            });
            return dailyTargetList;

        }
        /// <summary>
        /// Method to check whether the selected date is within the target effective dates
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="selectedDate">selected date</param>
        /// <returns></returns>
        private bool CheckIfDateIsWithinTheTargetDates(Target target, DateTime selectedDate)
        {
            if (selectedDate >= target.EffectiveStartDate &&
                selectedDate <= target.EffectiveEndDate)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if target or actual exists.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns></returns>
        private bool CheckIfTargetOrActualExists(Target target, DateTime selectedDate)
        {
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == selectedDate.Month);
            bool targetExists = false;
            bool actualExists = false;
            if (target.IsCascaded)
            {
                if (monthlyTarget != null)
                {
                    if (target.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets)
                    {
                        int dailyTargetCount = monthlyTarget.DailyTargets?.Count(x => x.Day == selectedDate.Day && x.RolledUpGoalValue != null) ?? 0;
                        targetExists = dailyTargetCount > 0;
                    }
                    else
                    {
                        int dailyTargetCount = monthlyTarget.DailyTargets?.Count(x => x.Day == selectedDate.Day && x.MaxGoalValue != null) ?? 0;
                        targetExists = dailyTargetCount > 0;
                    }
                }

                var actualCount = dailyActualRepository.GetAll().Where(x => x.TargetId == target.Id && x.Date == selectedDate)?.Count() ?? 0;
                actualExists = actualCount > 0;
            }

            return targetExists || actualExists;
        }

        /// <summary>
        /// Checks if actual exists.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns></returns>
        private bool CheckIfEnteredTargetOrActualExists(Target target, DateTime selectedDate)
        {
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == selectedDate.Month);
            bool targetExists = false;
            bool actualExists = false;
            int dailyTargetCount = monthlyTarget.DailyTargets?.Count(x => x.Day == selectedDate.Day && x.MaxGoalValue != null) ?? 0;
            targetExists = dailyTargetCount > 0;
            if (target.IsCascaded)
            {               
                var actualCount = dailyActualRepository.GetAll().Where(x => x.TargetId == target.Id && x.Date == selectedDate)?.Count() ?? 0;
                actualExists = actualCount > 0;
            }

            return targetExists || actualExists; 
        }

        /// <summary>
        /// Determines the Cascaded Metrics Tracking Method for the month for the given target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="month">The month.</param>
        /// <returns>
        ///   <c>true</c> if [is rolled up targets for month] [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsRolledUpTargetsForMonth(Target target, int month)
        {
            bool isLegendaryData = false;

            // non-cascaded targets always go with entered targets
            if (!target.IsCascaded)
            {
                return false;
            }
            
            // logic to find out legend data
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            int targetYear = target.CalendarYear.StartDate.Year;
            isLegendaryData = ((month < (currentDate.Month - 1) && targetYear == currentDate.Year) || (targetYear < currentDate.Year));
            if (isLegendaryData)
            {
                var monthlyTarget = target.MonthlyTargets?.FirstOrDefault(x => x.Month == month);
                return monthlyTarget?.IsRolledUpGoal ?? false;
            }
            else
            {
                return target.CascadedMetricsTrackingMethodId == (int)CascadedMetricsTrackingMethod.RolledUpTargets;
            }
        }

        /// <summary>
        /// Calculates the cumulative goal for a day for old targets
        /// </summary>
        /// <param name="target">Target entity</param>
        /// <param name="selectedDate">Day for which goal is required</param>
        /// <returns></returns>
        private decimal? CalculateCumulativeGoalForLegendTargets(Target target, DateTime selectedDate)
        {
            int year = selectedDate.Year;
            int month = selectedDate.Month;
            DateTime effectiveStartDate = new DateTime(year, month, 1);
            if (target.EffectiveStartDate > effectiveStartDate)
            {
                effectiveStartDate = target.EffectiveStartDate;
            }
            // Get monthly target
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
                x => x.Month == selectedDate.Month);

            // If selected date is last working day of month it will just return the max goal value for the month.
            if (selectedDate == holidayCalculator.GetLastWorkingDayOfMonthForTarget(target, month, year))
            {
                return monthlyTarget?.MaxGoalValue;
            }

            //get daily goal
            decimal? perDayGoal = GetDailyGoal(target, selectedDate);
            decimal? dailyTargetsTillSelDate = null;
            decimal? dailyActualGoalValueTillSelectedDate = null;
            int? countActualEnteredTillDate = null;
            List<int> dailyTargetDaysTillDate = null;
            //get number of holidays till the selected date
            int holidaysTillSelDate = holidayCalculator.CountHolidaysBetweenDaysOfMonth(target.Id,
                target.ScorecardId, effectiveStartDate, selectedDate);

            //get number of days for which daily target is entered till selected day
            int? dailyTargetDaysTillSelDate = monthlyTarget.DailyTargets?.Where
               (x => x.Day <= selectedDate.Day && x.MaxGoalValue.HasValue).Count();

            //get total number of effective days between selectedDate and
            //target effective start date excluding holidays and daily target entered days

            if (dailyTargetDaysTillSelDate != null)
            {
                // get days having daily target entry
                dailyTargetDaysTillDate = monthlyTarget.DailyTargets?.Where
                (x => x.Day <= selectedDate.Day && x.MaxGoalValue.HasValue).Select(x => x.Day).ToList();

                // get number of actual entries till date, excluding the days having daily targets            
                countActualEnteredTillDate = dailyActualRepository.GetAll()
                                  .Where(x => x.TargetId == target.Id && x.Date < selectedDate && x.Date >= effectiveStartDate && x.ActualValue != null && !dailyTargetDaysTillDate.Any(y => y == x.Date.Day)).Count();
            }
            else
            {
                countActualEnteredTillDate = dailyActualRepository.GetAll()
                                      .Where(x => x.TargetId == target.Id && x.Date < selectedDate && x.Date >= effectiveStartDate && x.ActualValue != null).Count();
            }

            int effectiveDays = (selectedDate.Day - effectiveStartDate.Day + 1) -
            (holidaysTillSelDate + (dailyTargetDaysTillSelDate ?? 0) + (countActualEnteredTillDate ?? 0));

            if (dailyTargetDaysTillSelDate != null)
            {
                dailyTargetsTillSelDate = monthlyTarget.DailyTargets?.Where
                (x => x.Day <= selectedDate.Day && x.MaxGoalValue.HasValue).Sum(x => x.MaxGoalValue);
            }

            if (countActualEnteredTillDate != null)
            {
                // Get the sum of goal value entered till date, excluding days having daily targets
                if (dailyTargetDaysTillSelDate != null)
                {
                    dailyActualGoalValueTillSelectedDate = dailyActualRepository.GetAll()
                                            .Where(x => x.TargetId == target.Id && x.Date < selectedDate && x.Date >= effectiveStartDate && !dailyTargetDaysTillDate.Any(y => y == x.Date.Day))
                                            .Sum(x => x.GoalValue);
                }
                else
                {
                    dailyActualGoalValueTillSelectedDate = dailyActualRepository.GetAll()
                                            .Where(x => x.TargetId == target.Id && x.Date < selectedDate && x.Date >= effectiveStartDate)
                                            .Sum(x => x.GoalValue);
                }
            }

            //sum up daily targets till selectedDate, calculated goal value in daily actuals and effective days goal to get cumulative goal
            var cumulativeGoal = (dailyTargetsTillSelDate ?? 0) + (dailyActualGoalValueTillSelectedDate ?? 0) + (perDayGoal * effectiveDays);

            return (cumulativeGoal.HasValue && target.Metric.DataTypeId == Constants.DataTypeWholeNumber)
                ? Math.Floor(cumulativeGoal.Value) : cumulativeGoal;

        }

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target repository</param>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        public ScorecardGoalCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (targetRepository == null ||
                dailyActualRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.holidayCalculator = CreateHolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository,
                scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Creates an instance of HolidayCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual HolidayCalculator CreateHolidayCalculator(IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository, IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (holidayCalculator == null)
            {
                holidayCalculator = new HolidayCalculator(dailyActualRepository, scorecardHolidayPatternRepository,
                    scorecardWorkdayPatternRepository, scorecardWorkdayTrackerRepository);
            }
            return holidayCalculator;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Get daily target of a day in a month
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public virtual decimal? GetDailyGoal(int targetId, int month, int day)
        {
            // Find target with the given id
            var target = targetRepository.Get(targetId);
            if (target != null && target.IsActive)
            {
                //get the year to which the month belongs to
                int year = CalendarUtility.GetYearOfTheMonth(target.CalendarYear, month);
                DateTime selectedDate = new DateTime(year, month, day).Date;
                bool istargetOrActualExists = CheckIfTargetOrActualExists(target, selectedDate);

                //if given day is holiday target will be null
                if (holidayCalculator.CheckIfDateIsaHoliday(target.ScorecardId, selectedDate) && !istargetOrActualExists)
                {
                    return null;
                }
                if (!CheckIfDateIsWithinTheTargetDates(target, selectedDate))
                {
                    return null;
                }
                decimal? goalValue = GetDailyGoal(target, selectedDate);
                return goalValue;
            }
            else {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            };
        }

        /// <summary>
        /// Get daily stretch goal for a selected date
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="selectedDate">selected date</param>
        /// <returns></returns>
        public virtual decimal? GetDailyStretchGoal(Target target, DateTime selectedDate)
        {
            if (target != null)
            {
                int metricDataTypeId = target.Metric.DataTypeId;
                decimal? dailyStretchGoalValue = null;
                int year = selectedDate.Year;
                int month = selectedDate.Month;

                //get monthly target
                var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == month);

                //check if monthly target has value
                if (monthlyTarget != null && monthlyTarget.StretchGoalValue.HasValue)
                {
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
                    //get the count of total holidays between two dates
                    int totalNumberofHolidays = holidayCalculator.CountHolidaysBetweenDaysOfMonth(target.Id,
                            target.ScorecardId, monthStartDate, monthEndDate);

                    //get count total days in a month
                    int totalDaysInMonth = (int)(monthEndDate - monthStartDate).TotalDays + 1;

                     //dividing monthly target by no of remaining working days
                    int totalDays = (totalDaysInMonth) - (totalNumberofHolidays);
                    if (monthlyTarget.DailyRate != null)
                    {
                        dailyStretchGoalValue = monthlyTarget.StretchGoalValue;
                    }
                    else if(CheckIfEnteredTargetOrActualExists(target, selectedDate))
                    {
                        dailyStretchGoalValue = monthlyTarget.StretchGoalValue / totalDays;
                    }
                }
                if (dailyStretchGoalValue.HasValue)
                {
                    //if the metric data type is whole number round value to its upper limit, else
                    //round decimal points to 2 digits
                    dailyStretchGoalValue = Math.Round(dailyStretchGoalValue.Value, 2,
                        MidpointRounding.AwayFromZero);
                }
                return dailyStretchGoalValue;
            }
            return null;
        }

        /// <summary>
        /// Method to get daily goal and Stretch goal
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="month">month</param>
        /// <param name="day">day</param>
        /// <returns>daily goal and Stretch goal value</returns>
        public virtual DailyTargetData GetDailyGoalAndStretchGoal(Target target, int month,
            int day)
        {
            //get the year to which the month belongs to
            int year = CalendarUtility.GetYearOfTheMonth(target.CalendarYear, month);
            DateTime selectedDate = new DateTime(year, month, day).Date;
            bool istargetOrActualExists = CheckIfTargetOrActualExists(target, selectedDate);
            //check if the selected date is a holiday
            if (holidayCalculator.CheckIfDateIsaHoliday(target.ScorecardId, selectedDate) && !istargetOrActualExists)
            {
                return null;
            }
            //check if the selected date is outside target effective start date and end date
            if (!CheckIfDateIsWithinTheTargetDates(target, selectedDate))
            {
                return null;
            }
            else
            {
                return new DailyTargetData()
                {
                    GoalValue = (target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
                    ? CalculateCumulativeGoal(target, selectedDate) : GetDailyGoal(target, selectedDate),
                    StretchGoalValue = GetDailyStretchGoal(target, selectedDate)
                };
            }
        }

        /// <summary>
        /// Method to get monthly goal 
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="month">month</param>
        /// <returns>monthly target value</returns>
        public virtual decimal? GetMonthlyGoal(int targetId, int month)
        {
            var target = targetRepository.Get(targetId);

            if (target == null || !target.IsActive)
            {
                throw new NDMSBusinessException(Constants.TargetNotFound);
            }

            // logic to find out legend data
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            int targetYear = target.CalendarYear.StartDate.Year;
            // check whether cascaded metrics tracking method is rolled up targets
            bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, month);
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(x => x.Month == month);           

            return isRolledUpTarget ? monthlyTarget?.RolledUpGoalValue : monthlyTarget?.MaxGoalValue;
        }

        /// <summary>
        /// Calculates the cumulative goal for a day if daily target exists
        /// </summary>
        /// <param name="target">Target entity</param>
        /// <param name="selectedDate">Day for which goal is required</param>
        /// <returns></returns>
        public virtual decimal? CalculateCumulativeGoal(Target target, DateTime selectedDate)
        {
            int year = selectedDate.Year;
            int month = selectedDate.Month;
            DateTime effectiveStartDate = new DateTime(year, month, 1);
            if (target.EffectiveStartDate > effectiveStartDate)
            {
                effectiveStartDate = target.EffectiveStartDate;
            }
            // Get monthly target
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
                x => x.Month == selectedDate.Month);

            if (monthlyTarget == null)
            {
                return null;
            }

                           // check whether cascaded metrics tracking method is rolled up targets
             bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, month);

            //get daily goal
            decimal? dailyTargetsTillSelDate = null;

            //get number of days for which daily target is entered till selected day
            int? dailyTargetDaysTillSelDate = monthlyTarget.DailyTargets?.Where
               (x => x.Day <= selectedDate.Day && (x.MaxGoalValue.HasValue || (isRolledUpTarget && x.RolledUpGoalValue.HasValue))).Count();

            if (dailyTargetDaysTillSelDate != null && dailyTargetDaysTillSelDate.Value > 0)
            {
                dailyTargetsTillSelDate = isRolledUpTarget ?
                    monthlyTarget.DailyTargets?.Where(x => x.Day <= selectedDate.Day && x.RolledUpGoalValue.HasValue).Sum(x => x.RolledUpGoalValue) :
                    monthlyTarget.DailyTargets?.Where(x => x.Day <= selectedDate.Day && x.MaxGoalValue.HasValue).Sum(x => x.MaxGoalValue);

                return dailyTargetsTillSelDate;
            }
            else
            {
                return CalculateCumulativeGoalForLegendTargets(target, selectedDate);
            }
        }

        /// <summary>
        /// Calculates the cumulative MTD goal for a day if daily target exists
        /// </summary>
        /// <param name="target">Target entity</param>
        /// <param name="monthId">Month</param>
        /// <returns>Cumulative MTD Goal</returns>
        public virtual decimal? CalculateCumulativeMTDGoal(Target target, int monthId)
        {
            decimal? cumulativeMTDGoal = null;
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
                x => x.Month == monthId);

            // check whether cascaded metrics tracking method is rolled up targets
            bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, monthId);
            if (monthlyTarget.DailyTargets != null && monthlyTarget.DailyTargets.Any())
            {
                cumulativeMTDGoal = isRolledUpTarget ?
                                 monthlyTarget.DailyTargets.Where(x => x.RolledUpGoalValue.HasValue).Sum(x => x.RolledUpGoalValue) :
                                 monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue.HasValue).Sum(x => x.MaxGoalValue);
            }
            else
            {//Backward Compatibility
                cumulativeMTDGoal = monthlyTarget.MaxGoalValue;
            }
            return cumulativeMTDGoal;


        }

        /// <summary>
        /// Calculates the average MTD goal for a day
        /// </summary>
        /// <param name="target">Target entity</param>
        /// <param name="monthId">Month</param>
        /// <returns>Average MTD Goal Value</returns>
        public decimal? CalculateAverageMTDGoal(Target target, int monthId)
        {
            decimal? averageMTDGoal = null;
            // Get monthly target
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
                x => x.Month == monthId);

            if (monthlyTarget != null)
            {
                // check whether cascaded metrics tracking method is rolled up targets
                bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, monthId);
                if (monthlyTarget.DailyTargets?.Any() ?? false)
                {
                    averageMTDGoal = isRolledUpTarget ?
                                     monthlyTarget.DailyTargets.Where(x => x.RolledUpGoalValue.HasValue).Average(x => x.RolledUpGoalValue) :
                                     monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue.HasValue).Average(x => x.MaxGoalValue);
                }
                else
                {
                    //Backward Compatibility
                    var calendarYear = target.CalendarYear;
                    int requestedYear = CalendarUtility.GetYearOfTheMonth(calendarYear, monthId);
                    DateTime monthStartDate = new DateTime(requestedYear, monthId, 1);
                    DateTime monthEndDate = new DateTime(requestedYear, monthId, DateTime.DaysInMonth(requestedYear, monthId));
                    int holidaysTillDate = holidayCalculator.CountHolidaysBetweenDaysOfMonth(target.Id, target.ScorecardId, monthStartDate, monthEndDate);
                    int workDaysTillDate = monthEndDate.Day - (monthStartDate.Day - 1) - holidaysTillDate;

                    averageMTDGoal = workDaysTillDate > 0 && monthlyTarget.MaxGoalValue.HasValue ? (decimal?)Math.Round(monthlyTarget.MaxGoalValue.Value / workDaysTillDate, 2, MidpointRounding.AwayFromZero) : null;
                }
                return averageMTDGoal;
            }
            return null;
        }

        /// <summary>
        /// Method to calculate daily MTD Goal 
        /// </summary>
        /// <param name="target">Target entity</param>
        /// <param name="monthId">Month</param>
        /// <returns>Daily MTD goal</returns>
        public decimal? GetDailyMTDGoal(Target target, int monthId)
        {
            decimal? dailyTargetValue = null;

            // Get monthly target
            var monthlyTarget = target.MonthlyTargets.FirstOrDefault(
            x => x.Month == monthId);

            // check whether cascaded metrics tracking method is rolled up targets
            bool isRolledUpTarget = IsRolledUpTargetsForMonth(target, monthId);
            if (monthlyTarget.DailyTargets != null && monthlyTarget.DailyTargets.Any())
            {
                dailyTargetValue = isRolledUpTarget ?
                            monthlyTarget.DailyTargets.Where(x => x.RolledUpGoalValue.HasValue).OrderByDescending(x => x.Day).FirstOrDefault()?.RolledUpGoalValue :
                            monthlyTarget.DailyTargets.Where(x => x.MaxGoalValue.HasValue).OrderByDescending(x => x.Day).FirstOrDefault()?.MaxGoalValue;
            }
            else
            {
                var calendarYear = target.CalendarYear;
                int requestedYear = CalendarUtility.GetYearOfTheMonth(calendarYear, monthId);
                var lastWorkDay = holidayCalculator.GetLastWorkingDayOfMonthForTarget(target, monthId, requestedYear);
                dailyTargetValue = GetDailyGoal(target, lastWorkDay);

            }

            return dailyTargetValue;
        }

        /// <summary>
        /// Gets the last working day of month for target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>The last working day</returns>
        public DateTime GetLastWorkingDayOfMonthForTarget(Target target, int month, int requestedYear)
        {
            return holidayCalculator.GetLastWorkingDayOfMonthForTarget(target, month, requestedYear);
        }

        /// <summary>
        /// Distribute Monthly Goal value evenly among work days
        /// </summary>
        /// <param name="dailyTargetList"></param>
        /// <param name="metricId"></param>
        /// <param name="goalValue"></param>
        /// <returns></returns>
        public List<DailyTargetItem> DistributeMonthlyGoalValueAmongWorkdaysEvenly(List<DailyTargetItem> dailyTargetList, int dataTypeId, decimal goalValue)
        {

            var distributedTargetList = new List<DailyTargetItem>();
            switch (dataTypeId)
            {
                case Constants.DataTypeWholeNumber:
                    distributedTargetList = DistributeMonthlyGoalAsWholeNumbers(dailyTargetList, (int)goalValue);
                    break;
                case Constants.DataTypeDecimalNumber:
                    distributedTargetList = DistributeMonthlyGoalAsDecimal(dailyTargetList, goalValue);
                    break;
                case Constants.DataTypePercentage:
                    distributedTargetList = DistributeMonthlyGoalAsDecimal(dailyTargetList, goalValue);
                    break;
                case Constants.DataTypeAmount:
                    distributedTargetList = DistributeMonthlyGoalAsDecimal(dailyTargetList, goalValue);
                    break;
                default:
                    distributedTargetList = DistributeMonthlyGoalAsDecimal(dailyTargetList, goalValue);
                    break;
            }
            return distributedTargetList;
        }

        /// <summary>
        /// Distribute DailyRate Goal value evenly among work days
        /// </summary>
        /// <param name="dailyTargetList"></param>
        /// <param name="metricId"></param>
        /// <param name="goalValue"></param>
        /// <returns></returns>
        public List<DailyTargetItem> DistributeDailyRateAmongWorkDays(List<DailyTargetItem> dailyTargetList, int dataTypeId, decimal dailyRate)
        {
            var distributedTargetList = dailyTargetList;
            switch (dataTypeId)
            {
                case Constants.DataTypeWholeNumber:
                    distributedTargetList.Where(x => !(x.IsHoliday || x.IsOutofRange)).ToList().ForEach(target =>
                    {
                        target.GoalValue = (int)dailyRate;
                    });
                    break;
                default:
                    distributedTargetList.Where(x => !(x.IsHoliday || x.IsOutofRange)).ToList().ForEach(target =>
                    {
                        target.GoalValue = dailyRate;
                    });
                    break;
            }
            return distributedTargetList;
        }

        /// <summary>
        /// if
        /// </summary>
        /// <param name="effectiveStartDate"></param>
        /// <param name="effectiveEndDate"></param>
        /// <param name="selectedDate"></param>
        /// <returns></returns>
        public bool CheckIfSelectedDateIsOutOfDateRange(DateTime startDate, DateTime endDate, DateTime selectedDate)
        {
            if (selectedDate.Date < startDate.Date || selectedDate.Date > endDate.Date)
                return true;
            else
                return false;

        }
        #endregion
    }
}
