using System;
using System.Web.Http;
using NDMS.Apis.Models;
using NDMS.DomainModel.DTOs;
using NDMS.Business;
using System.Web.Http.Description;
using System.Collections.Generic;
using NDMS.Business.Interfaces;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// API controller to deal with configuration data and general data look up
    /// </summary>
    [RoutePrefix("api/Configuration")]
    public class ConfigurationController : BaseController
    {
        #region Field(s)
        private readonly IConfigurationSettingsManager cfgSettingsManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="cfgSettingsManager">IConfigurationSettingsManager reference</param>
        public ConfigurationController(IConfigurationSettingsManager cfgSettingsManager)
        {
            this.cfgSettingsManager = cfgSettingsManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Method to retrieve web config values
        /// </summary>
        /// <returns>web config values as dictionary</returns>
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            ApiResponse<ConfigurationSettingsItem> response = new
                ApiResponse<ConfigurationSettingsItem>();
            response.Data = cfgSettingsManager.Get();
            return Ok(response);
        }

        /// <summary>
        /// Api to return current date and year Id 
        /// </summary>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetCurrentDateAndYearId")]
        [ResponseType(typeof(CurrentDateDetails))]
        public IHttpActionResult GetCurrentDateAndYearId()
        {
            ApiResponse<CurrentDateDetails> response = new ApiResponse<CurrentDateDetails>();
            response.Data = cfgSettingsManager.GetCurrentDateAndYearId();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the list of months in year
        /// </summary>
        /// <returns>List of months in year</returns>
        [Route("GetAllYearMonthsList")]
        [ResponseType(typeof(IEnumerable<MonthItem>))]
        public IHttpActionResult GetAllYearMonthsList()
        {
            ApiResponse<IEnumerable<MonthItem>> response = new
                ApiResponse<IEnumerable<MonthItem>>();
            response.Data = cfgSettingsManager.GetAllYearMonthsList();
            return Ok(response);
        }
        #endregion
    }
}
