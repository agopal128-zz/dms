using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System;
using System.Linq;

/// <summary>
/// To manipulate Workday Pattern
/// </summary>
namespace NDMS.Business.Common
{
    public class WorkdayCalculator
    {
        #region Feild(s)

        private IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository;

        #endregion

        #region Constructor

        public WorkdayCalculator(IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository)
        {
            if(scorecardWorkdayPatternRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }

            this.scorecardWorkdayPatternRepository = scorecardWorkdayPatternRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the scorecard workday pattern for scorecard with a given date range.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="monthStartDate">The month start date.</param>
        /// <param name="monthEndDate">The month end date.</param>
        /// <returns></returns>
        public ScorecardWorkdayPattern GetScorecardWorkdayPatternForScorecardWithDateRange(int scorecardId, DateTime monthStartDate, DateTime monthEndDate)
        {
            return scorecardWorkdayPatternRepository.GetAll()
                                .Where(x => x.ScorecardId == scorecardId && x.EffectiveStartDate <= monthStartDate &&
                                (x.EffectiveEndDate == null || x.EffectiveEndDate >= monthEndDate)).FirstOrDefault();            
        }

        /// <summary>
        ///Method to check whether workday pattern is available for a scorecard.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        public bool CheckIfWorkdayPatternAvailableForScorecard(int scorecardId)
        {
            bool isWorkdayPatternAvailable = false;
            var workdayPattern= scorecardWorkdayPatternRepository.GetAll()
                                .Where(x => x.ScorecardId == scorecardId).FirstOrDefault();
            if (workdayPattern != null)
            {
                isWorkdayPatternAvailable = true;
            }
            else
            {
                isWorkdayPatternAvailable = false;
            }
            return isWorkdayPatternAvailable;
        }

        /// <summary>
        /// Determines whether the specified day is workday.
        /// </summary>
        /// <param name="workdayPattern">The workday pattern.</param>
        /// <param name="monthDate">The month date.</param>
        /// <returns>
        ///   <c>true</c> if the specified day is workday; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWorkday(ScorecardWorkdayPattern workdayPattern, DateTime monthDate)
        {
            switch (monthDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return workdayPattern.IsSunday;
                case DayOfWeek.Monday:
                    return workdayPattern.IsMonday;
                case DayOfWeek.Tuesday:
                    return workdayPattern.IsTuesday;
                case DayOfWeek.Wednesday:
                    return workdayPattern.IsWednesday;
                case DayOfWeek.Thursday:
                    return workdayPattern.IsThursday;
                case DayOfWeek.Friday:
                    return workdayPattern.IsFriday;
                case DayOfWeek.Saturday:
                    return workdayPattern.IsSaturday;
                default:
                    return false;
            }

        }

        #endregion
    }
}
