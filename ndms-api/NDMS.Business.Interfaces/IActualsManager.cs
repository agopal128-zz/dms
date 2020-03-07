using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines methods which deals with managing actuals
    /// </summary>
    public interface IActualsManager : IDisposable
    {
        /// <summary>
        /// Add daily or monthly actuals
        /// </summary>
        /// <param name="actualRequest">actual request</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>New actual entry's Id</returns>
        int AddActual(ActualItem actualRequest, string userName);

        /// <summary>
        /// Update daily or monthly actuals
        /// </summary>
        /// <param name="actualRequest">actual request</param>
        /// <param name="userName">logged in user name</param>
        void UpdateActual(ActualItem actualRequest, string userName);

        /// <summary>
        /// Marks a day as holiday in the system
        /// </summary>
        /// <param name="targetId">Target Id corresponding</param>
        /// <param name="date">Date which needs to be marked as holiday</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>New actual entry's Id</returns>
        int MarkHolidayOrWorkday(int targetId, DateTime date, bool isWorkday, string userName);

        /// <summary>
        /// Populates the daily target if not already exist or target found on holiday.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="userName">Name of the user.</param>
        void PopulateDailyTargets(Target target, int month, string userName);
    }
}
