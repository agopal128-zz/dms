using System;
using NDMS.DomainModel.DTOs;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{   
    /// <summary>
    /// Defines Holiday Pattern Interface
    /// </summary>
    public interface IHolidayPatternManager : IDisposable
    {
        #region Methods
        /// <summary>
        /// Method to retreive all Holiday Patterns.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HolidayPatternItem> GetAll();

        /// <summary>
        /// Method to retrieve selected Holiday Pattern
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        HolidayPatternItem GetHolidayPattern(int id);

        /// <summary>
        /// Method to add or update holiday Pattern Object
        /// </summary>
        /// <param name="holidayPattern"></param>
        void AddOrUpdateHolidayPattern(HolidayPatternItem holidayPattern, string userName);

        /// <summary>
        /// Method to update Holidays mapping
        /// </summary>
        /// <param name="request"></param>
        /// <param name="username"></param>
        void UpdateHolidayPatternMapping(HolidayPatternInfoRequest request, string username);

        /// <summary>
        /// Copies the holidays associated with one holiday pattern to a new Item
        /// </summary>
        /// <param name="holidayPatternId"></param>
        /// <param name="username"></param>
        void CopyHolidayPatternInfo(int holidayPatternId, string username);


        #endregion
    }
}
