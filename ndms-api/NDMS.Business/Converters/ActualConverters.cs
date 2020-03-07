using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Business.Converters
{
    /// <summary>
    /// Contains various data structure converters needed for Actuals module
    /// </summary>
    internal static class ActualConverters
    {
        #region Public Method(s)
        /// <summary>
        /// Convert Daily Actual to Daily Actual History
        /// </summary>
        /// <param name="dailyActual">Daily Actual</param>
        /// <returns>Daily Actual History</returns>
        public static DailyActualHistory ConvertDailyActualToDailyActualHistory(
            DailyActual dailyActual)
        {
            var dailyActualHistory = new DailyActualHistory()
            {
                DailyActualId = dailyActual.Id,
                Date = dailyActual.Date,
                GoalValue = dailyActual.GoalValue,
                ActualValue = dailyActual.ActualValue,
                Status = dailyActual.Status,
                TargetId = dailyActual.TargetId,
                CreatedOn = dailyActual.CreatedOn,
                LastModifiedOn = dailyActual.LastModifiedOn,
                CreatedBy = dailyActual.CreatedBy,
                LastModifiedBy = dailyActual.LastModifiedBy
            };

            return dailyActualHistory;
        }

        /// <summary>
        /// Convert Monthly Actual to Monthly Actual History
        /// </summary>
        /// <param name="monthlyActual">Monthly Actual</param>
        /// <returns>Monthly Actual History</returns>
        public static MonthlyActualHistory ConvertMonthlyActualToMonthlyActualHistory
            (MonthlyActual monthlyActual)
        {
            var monthlyActualHistory = new MonthlyActualHistory()
            {
                MonthlyActualId = monthlyActual.Id,
                Month = monthlyActual.Month,
                ActualValue = monthlyActual.ActualValue,
                Status = monthlyActual.Status,
                TargetId = monthlyActual.TargetId,
                CreatedOn = monthlyActual.CreatedOn,
                LastModifiedOn = monthlyActual.LastModifiedOn,
                CreatedBy = monthlyActual.CreatedBy,
                LastModifiedBy = monthlyActual.LastModifiedBy
            };

            return monthlyActualHistory;
        }
        #endregion

    }
}
