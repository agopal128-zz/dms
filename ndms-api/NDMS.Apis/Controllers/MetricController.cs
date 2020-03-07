using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System.Collections.Generic;
using System.Web.Http;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// Api Controller for Metric
    /// </summary>
    [RoutePrefix("api/Metric")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole)]
    public class MetricController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// IMetricManager reference
        /// </summary>
        private readonly IMetricManager metricManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="metricManager">IMetricManager reference</param>
        public MetricController(IMetricManager metricManager)
        {
            this.metricManager = metricManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Method to retrieve all active metrics
        /// </summary>
        /// <returns>metric list</returns>
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            ApiResponse<IEnumerable<MetricItem>> response =
                new ApiResponse<IEnumerable<MetricItem>>();
            response.Data = metricManager.GetAll();
            return Ok(response);
        }

        /// <summary>
        /// Retrieve selected metric
        /// </summary>
        /// <param name="id">selected metric id</param>
        /// <returns>metric item</returns>
        [Route("Get/{id}")]
        public IHttpActionResult Get(int id)
        {
            ApiResponse<MetricItem> response = new ApiResponse<MetricItem>();
            response.Data = metricManager.Get(id);
            if (response.Data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        /// <summary>
        /// Api to retrieve all active goal types and data types 
        /// to fill dropdown in Add/Edit Metric screen
        /// </summary>
        /// <returns>
        /// response object containing list of goal types and data types  
        /// and KPIs
        /// </returns>
        [Route("GetMetricTemplateData")]
        public IHttpActionResult GetMetricTemplateData()
        {
            ApiResponse<MetricTemplateData> response = new ApiResponse<MetricTemplateData>();
            response.Data = metricManager.GetMetricTemplateData();
            return Ok(response);
        }

        /// <summary>
        /// Method to retrieve all assigned metrics
        /// </summary>
        /// <returns>metric mapping list</returns>
        [Route("GetAllMetricMappings")]
        public IHttpActionResult GetAllMetricMappings()
        {
            ApiResponse<IEnumerable<MetricMappingItem>> response =
                new ApiResponse<IEnumerable<MetricMappingItem>>();
            response.Data = metricManager.GetAllMetricMappings();
            return Ok(response);
        }

        /// <summary>
        /// Get all metrics whose name starts with the input string mentioned.
        /// </summary>
        /// <param name="name">Input name to match</param>
        /// <returns>List of metrics whose name starts with the input string</returns>
        [HttpGet]
        [Route("GetMetricsWithName/{name}")]
        public IHttpActionResult GetMetricsWithName(string name)
        {
            ApiResponse<IEnumerable<MetricSuggestion>> response =
                new ApiResponse<IEnumerable<MetricSuggestion>>();
            response.Data = metricManager.GetMetricsWithName(name);
            return Ok(response);
        }

        /// <summary>
        /// Api to add or update metric mapping
        /// </summary>
        /// <param name="metricMappingRequest">list of metric mapping object</param>
        /// <returns>http response</returns>
        [Route("AddorUpdateMetricMapping")]
        public IHttpActionResult AddorUpdateMetricMapping(
            List<MetricMappingItem> metricMappingRequest)
        {
            CheckModelState();
            metricManager.AddorUpdateMetricMapping(metricMappingRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to add or update metric 
        /// </summary>
        /// <param name="metricRequest">list of metric object</param>
        /// <returns>http response</returns>
        [Route("AddorUpdateMetric")]
        public IHttpActionResult AddorUpdateMetric(List<MetricItem> metricRequest)
        {
            CheckModelState();
            metricManager.AddorUpdateMetric(metricRequest, Username);
            return Ok();
        }
        #endregion
    }
}
