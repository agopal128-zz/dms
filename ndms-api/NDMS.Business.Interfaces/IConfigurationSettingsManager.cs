using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDMS.DomainModel.DTOs;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines Configuration Settings interface
    /// </summary>
    public interface IConfigurationSettingsManager : IDisposable
    {
        /// <summary>
        /// Retrieve Configuration Data
        /// </summary>       
        /// <returns>Configuration Data</returns>
        ConfigurationSettingsItem Get();

        /// <summary>
        /// Method to return current date and year Id
        /// </summary>
        /// <returns>Current date details</returns>
        CurrentDateDetails GetCurrentDateAndYearId();

        /// <summary>
        /// Retrieves the list of months
        /// </summary>
        /// <returns>List of months</returns>
        IEnumerable<MonthItem> GetAllYearMonthsList();
    }
}
