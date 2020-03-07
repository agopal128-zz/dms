using System;
using System.Linq;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System.Collections.Generic;

namespace NDMS.Business.Common
{
    public class HolidayCalculator
    {
        #region fields
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;        

        /// <summary>
        /// The scorecard holiday pattern repository
        /// </summary>
        private IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository;

        /// <summary>
        /// The scorecard workday pattern repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository;

        /// <summary>
        /// The scorecard workday tracker repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository;

        /// <summary>
        /// The workday pattern calculator
        /// </summary>
        private WorkdayCalculator workdayPatternCalculator;
        #endregion

        #region private methods
        /// <summary>
        /// Method to get total holidays between two dates in a month
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="monthStartDate">month start date</param>
        /// <param name="monthEndDate">month end date</param>
        /// <returns>
        /// total number of holidays
        /// </returns>
        private int GetTotalNumberOfHolidaysInaMonth(int targetId, int scorecardId,
            DateTime monthStartDate, DateTime monthEndDate)
        {
            int month = monthStartDate.Month;
            //get the count of marked holidays
            IQueryable<ScorecardWorkdayItem> markedWorkDays = scorecardWorkdayTrackerRepository.GetAll()
                .Where(x => x.ScorecardId == scorecardId && x.Date >= monthStartDate &&
                x.Date <= monthEndDate && x.IsActive).Select(x => new ScorecardWorkdayItem {
                    Date = x.Date,
                    IsWorkday = x.IsWorkDay
                });

            //get days list which are not marked holiday (keeping this for backward compatibility)
            IQueryable<int> days = dailyActualRepository.GetAll()
                .Where(x => x.TargetId == targetId && x.Date >= monthStartDate &&
                x.Date <= monthEndDate).Select(x => x.Date.Day);

            var holidayList = GetScorecardHolidayListInaMonth(scorecardId, monthStartDate, monthEndDate);

            // Get the count of no of holidays between two dates excluding 
            // the days which have actual entry
            int CountOfCalendarHolidays = holidayList.Count(x => x.Date >= monthStartDate && x.Date <= monthEndDate
                && !days.Any(d => d == x.Date.Day) && !markedWorkDays.Any(d => d.Date == x.Date));

            return markedWorkDays.Count(d => !d.IsWorkday) + CountOfCalendarHolidays;
        }

        /// <summary>
        /// Gets the scorecard holiday list in a month.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="monthStartDate">The month start date.</param>
        /// <param name="monthEndDate">The month end date.</param>
        /// <returns></returns>
        private IList<HolidayPatternInfo> GetScorecardHolidayListInaMonth(int scorecardId, DateTime monthStartDate, DateTime monthEndDate)
        {
            IList<HolidayPatternInfo> holidayList = new List<HolidayPatternInfo>();
            var holidayPattern = GetHolidayPatternForScorecardInaMonth(scorecardId, monthStartDate, monthEndDate);
            var workdayPattern = workdayPatternCalculator.GetScorecardWorkdayPatternForScorecardWithDateRange(scorecardId, monthStartDate, monthEndDate);
            if (holidayPattern != null && holidayPattern.Holidays != null && holidayPattern.Holidays.Any())
            {
                holidayList = holidayPattern.Holidays
                                  .Where(x => x.Date >= monthStartDate && x.Date <= monthEndDate && x.IsActive).ToList();
            }

            if (workdayPattern != null)
            {
                for (DateTime startDate = monthStartDate; startDate <= monthEndDate; startDate = startDate.AddDays(1))
                {
                    if (!(workdayPatternCalculator.IsWorkday(workdayPattern, startDate) || holidayList.Any(x => x.Date == startDate)))
                    {
                        holidayList.Add(new HolidayPatternInfo
                        {
                            Date = startDate
                        });
                    }
                }
            }
            else
            {
                // if no workday pattern, set all days as holiday
                for (DateTime startDate = monthStartDate; startDate <= monthEndDate; startDate = startDate.AddDays(1))
                {
                    holidayList.Add(new HolidayPatternInfo
                    {
                        Date = startDate
                    });
                }
            }

            return holidayList;
        }        

        private HolidayPattern GetHolidayPatternForScorecardInaMonth(int scorecardId, DateTime monthStartDate, DateTime monthEndDate)
        {
            var scorecardHolidayPattern = scorecardHolidayPatternRepository.GetAll()
                                         .Where(x => x.ScorecardId == scorecardId &&
                                         x.EffectiveStartDate <= monthStartDate &&
                                         (x.EffectiveEndDate >= monthEndDate || x.EffectiveEndDate == null)).FirstOrDefault();            

            return scorecardHolidayPattern?.HolidayPattern;
        }

        #endregion

        #region Protected Methods

        protected WorkdayCalculator CreateWorkdayPatternCalculator(IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository)
        {
            if(workdayPatternCalculator == null)
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
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        /// <param name="holidayRepository">Holiday repository</param>
        public HolidayCalculator(IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (dailyActualRepository == null || scorecardWorkdayPatternRepository == null
                || scorecardHolidayPatternRepository == null || scorecardWorkdayTrackerRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.dailyActualRepository = dailyActualRepository;
            this.scorecardWorkdayTrackerRepository = scorecardWorkdayTrackerRepository;
            this.scorecardHolidayPatternRepository = scorecardHolidayPatternRepository;
            this.scorecardWorkdayPatternRepository = scorecardWorkdayPatternRepository;
            this.workdayPatternCalculator = CreateWorkdayPatternCalculator(scorecardWorkdayPatternRepository);
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Method to get total holidays between two dates in a month
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="calendarYearId">calendar year id</param>
        /// <param name="monthStartDate">month start date</param>
        /// <param name="monthEndDate">month end date</param>
        /// <returns>total number of holidays</returns>
        public int CountHolidaysBetweenDaysOfMonth(int targetId, int scorecardId,
            DateTime monthStartDate, DateTime monthEndDate)
        {
            return GetTotalNumberOfHolidaysInaMonth(targetId, scorecardId, monthStartDate, monthEndDate);
        }

        /// <summary>
        /// Method to check whether day is a holiday
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>
        /// flag which says whether the day is holiday or not
        /// </returns>
        public bool CheckIfDateIsaHoliday(int scorecardId, DateTime selectedDate)
        {
            var markedHoliday = scorecardWorkdayTrackerRepository.GetAll()
                .FirstOrDefault(x => x.ScorecardId == scorecardId && x.Date == selectedDate && x.IsActive);
            //check if given date is marked holiday
            if (markedHoliday != null)
            {
                return !markedHoliday.IsWorkDay;
            }
            //check if a calendar holiday is unmarked as holiday
            else
            {
                var holidayList = GetScorecardHolidayListInaMonth(scorecardId, selectedDate, selectedDate);
                if (holidayList.Any())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to check whether day in a month of a calendar year is a holiday
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="day">The day.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        public bool CheckIfDateIsaHoliday(int scorecardId, int day, int month, int year)
        {
            DateTime selectedDate = new DateTime(year, month, day);
            var markedHoliday = scorecardWorkdayTrackerRepository.GetAll()
                .FirstOrDefault(x => x.ScorecardId == scorecardId && x.Date == selectedDate && x.IsActive);
            //check if given date is marked holiday
            if (markedHoliday != null)
            {
                return !markedHoliday.IsWorkDay;
            }
            //check if a calendar holiday is unmarked as holiday
            else
            {
                var holidayList = GetScorecardHolidayListInaMonth(scorecardId, selectedDate, selectedDate);
                if (holidayList.Any())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the last working day of month for target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="selectedDate">The selected date.</param>
        /// <returns>The last working day</returns>
        public DateTime GetLastWorkingDayOfMonthForTarget(Target target, int month, int year)
        {
            DateTime effectiveEndDate = new DateTime(year, month,
                        DateTime.DaysInMonth(year, month));

            // if target end date is less than the month end date 
            // then last working day for the month that target will be less than or equal to target end date
            if (effectiveEndDate > target.EffectiveEndDate)
            {
                effectiveEndDate = target.EffectiveEndDate;
            }


            while (CheckIfDateIsaHoliday(target.ScorecardId, effectiveEndDate) && effectiveEndDate.Month == month)
            {
                effectiveEndDate = effectiveEndDate.AddDays(-1);
            }

            return effectiveEndDate;
        }

        /// <summary>
        /// Gets the holiday list for scorecard.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="monthStartDate">The month start date.</param>
        /// <param name="monthEndDate">The month end date.</param>
        /// <returns>The holiday list</returns>
        public IList<HolidayPatternInfo> GetHolidayListForScorecardInaMonth(int scorecardId, DateTime monthStartDate, DateTime monthEndDate)
        {
            return GetScorecardHolidayListInaMonth(scorecardId, monthStartDate, monthEndDate);
        }

        /// <summary>
        /// Checks if pattern is set for scorecard for the month and year.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>True if pattern is set, otherwise false</returns>
        public bool CheckIfPatternIsSetForScorecard(int scorecardId, int month, int year)
        {
            DateTime monthStartDate = new DateTime(year, month, 1);
            DateTime monthEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var holidayPattern = GetHolidayPatternForScorecardInaMonth(scorecardId, monthStartDate, monthEndDate);
            if(holidayPattern == null)
            {
                return false;
            }

            var workdayPattern = workdayPatternCalculator.GetScorecardWorkdayPatternForScorecardWithDateRange(scorecardId, monthStartDate, monthEndDate);
            if(workdayPattern == null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
