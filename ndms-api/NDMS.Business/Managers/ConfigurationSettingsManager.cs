using NDMS.Business.Common;
using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Implements IConfigurationSettingsManager
    /// </summary>
    public class ConfigurationSettingsManager : IConfigurationSettingsManager
    {
        #region Field(s)
        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="yearRepository">Year Repository</param>
        public ConfigurationSettingsManager(IBaseRepository<Year> yearRepository)
        {
            if (yearRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.yearRepository = yearRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieve Configuration Data
        /// </summary>       
        /// <returns>Configuration Data</returns>
        public ConfigurationSettingsItem Get()
        {
            return new ConfigurationSettingsItem()
            {
                MaxTeamCount = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.MaxTeamCount]),
                MaxKPIOwnerCount = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.MaxKPIOwnerCount]),
                KPIPrimaryMetricCount = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.KPIPrimaryMetricCount]),
                KPISecondaryMetricCount = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.KPISecondaryMetricCount]),
                AutoRefreshDuration = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.AutoRefreshDuration]),
                SessionTimeout = Convert.ToInt32(
                    ConfigurationManager.AppSettings[AppSettingsKeys.SessionTimeout])
            };
        }

        /// <summary>
        /// Method to return current date and year Id
        /// </summary>
        /// <returns>Current date details</returns>
        public CurrentDateDetails GetCurrentDateAndYearId()
        {
            CurrentDateDetails curDateDetails = null;
            DateTime curDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            var curYear = yearRepository.GetAll().Where(x => x.StartDate <= curDate &&
                x.EndDate >= curDate).FirstOrDefault();
            if (curYear != null)
            {
                curDateDetails = new CurrentDateDetails();
                curDateDetails.CurrentDate = curDate;
                curDateDetails.YearId = curYear.Id;
            }
            return curDateDetails;
        }

        /// <summary>
        /// Retrieves the list of months
        /// </summary>
        /// <returns>List of months </returns>
        public IEnumerable<MonthItem> GetAllYearMonthsList()
        {
            List<MonthItem> monthsList = new List<MonthItem>();
            var yearList = yearRepository.GetAll().ToList();

            foreach(var year in yearList)
            {
                List<MonthItem> yearMonths = CalendarUtility.GetMonthsBetweenDates
                    (year.StartDate, year.EndDate).ToList();
                yearMonths.ForEach(x => x.YearId = year.Id);
                monthsList.AddRange(yearMonths);
            }
                       
            return monthsList.OrderBy(x => x.Year).ThenBy(x => x.Id);
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
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    yearRepository = null;
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
