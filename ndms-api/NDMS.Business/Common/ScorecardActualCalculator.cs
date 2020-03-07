using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System;
using System.Linq;
using NDMS.DomainModel.DTOs;

namespace NDMS.Business.Common
{
   public class ScorecardActualCalculator
    {
        #region Field(s)
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;
    
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dailyActualRepository">Daily Actual Repository</param>
        public ScorecardActualCalculator(IBaseRepository<DailyActual> dailyActualRepository)
        {
            if (dailyActualRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.dailyActualRepository = dailyActualRepository;
        }
        #endregion


        #region Public Method(s)
        /// <summary>
        /// Calculates the cumulative actual till a selected day
        /// </summary>
        /// <param name="targetId">Target Id</param>
        /// <param name="selectedDate">selected date</param>
        /// <param name="selectedDayActual">actual for selected day</param>
        /// <returns>cumulative actual</returns>
        public virtual decimal? CalculateCumulativeActual(int targetId, DateTime selectedDate,
            decimal? selectedDayActual)
        {
            var dailyActualsTillSelDate = dailyActualRepository.GetAll()
                    .Where(x => x.TargetId == targetId && x.Date.Month == selectedDate.Month &&
                    x.Date < selectedDate && x.Status != ActualStatus.Holiday).Sum(x => x.ActualValue);
            return (dailyActualsTillSelDate.HasValue) ?
                (dailyActualsTillSelDate.Value + selectedDayActual) : selectedDayActual;
        }
        #endregion
    }
}
