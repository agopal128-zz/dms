using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System.Collections.Generic;
using System.Web.Http;

namespace NDMS.Apis.Controllers
{
    [RoutePrefix("api/OrganizationalData")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole)]
    public class OrganizationalDataController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// IOrganizationalDataManager reference
        /// </summary>
        private readonly IOrganizationalDataManager organizationalDataManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="organizationalDataManager">IOrganizationalDataManager reference</param>
        public OrganizationalDataController(IOrganizationalDataManager organizationalDataManager)
        {
            this.organizationalDataManager = organizationalDataManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Method to retrieve Business Segments, Productlines, Processes, Departments 
        /// and KPIs to fill dropdown in Add ScoreCard screen
        /// </summary>
        /// <param name="parentScorecardId">ID of parent scorecard</param>
        /// <returns>
        /// response object containing list of Business Segments, Productlines, Processes, Departments 
        /// and KPIs
        /// </returns>
        [Route("GetScorecardTemplateData/{parentScorecardId?}")]
        public IHttpActionResult GetScorecardTemplateData(int? parentScorecardId = null)
        {
            ApiResponse<ScorecardTemplateData> response = new ApiResponse<ScorecardTemplateData>();
            response.Data = organizationalDataManager.GetScorecardTemplateData(parentScorecardId);
            if (parentScorecardId.HasValue && response.Data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        /// <summary>
        /// Method to retrieve Business Segments, Productlines, Processes, Departments 
        /// and KPIs to fill dropdown in Add Metric Mapping
        /// </summary>
        /// <returns>
        /// response object containing list of Business Segments, Productlines, Processes, Departments 
        /// and KPIs
        /// </returns>
        [Route("GetMetricMappingTemplateData")]
        public IHttpActionResult GetMetricMappingTemplateData()
        {
            ApiResponse<MetricMappingTemplateData> response =
                new ApiResponse<MetricMappingTemplateData>();
            response.Data = organizationalDataManager.GetMetricMappingTemplateData();
            return Ok(response);
        }

        /// <summary>
        /// Method to get business segments
        /// </summary>
        /// <returns>api response with business segment list</returns>
        [Route("GetBusinessSegments")]
        public IHttpActionResult GetBusinessSegments()
        {
            ApiResponse<IEnumerable<BusinessSegmentItem>> response =
                new ApiResponse<IEnumerable<BusinessSegmentItem>>();
            response.Data = organizationalDataManager.GetAllBusinessSegments();
            return Ok(response);
        }

        /// <summary>
        /// Method to get divisions
        /// </summary>
        /// <returns>api response with division list</returns>
        [Route("GetDivisions")]
        public IHttpActionResult GetDivisions()
        {
            ApiResponse<IEnumerable<DivisionItem>> response =
                new ApiResponse<IEnumerable<DivisionItem>>();
            response.Data = organizationalDataManager.GetAllDivisions();
            return Ok(response);
        }

        /// <summary>
        /// Method to get departments
        /// </summary>
        /// <returns>api response with department list</returns>
        [Route("GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            ApiResponse<IEnumerable<DepartmentItem>> response =
                new ApiResponse<IEnumerable<DepartmentItem>>();
            response.Data = organizationalDataManager.GetAllDepartments();
            return Ok(response);
        }


        /// <summary>
        /// Method to get product lines
        /// </summary>
        /// <returns>api response with product lines</returns>
        [Route("GetProductLines")]
        public IHttpActionResult GetProductLines()
        {
            ApiResponse<IEnumerable<ProductLineItem>> response =
                new ApiResponse<IEnumerable<ProductLineItem>>();
            response.Data = organizationalDataManager.GetAllProductLines();
            return Ok(response);
        }

        /// <summary>
        /// Method to get facilities
        /// </summary>
        /// <returns>api response with facility list</returns>
        [Route("GetFacilities")]
        public IHttpActionResult GetFacilities()
        {
            ApiResponse<IEnumerable<FacilityItem>> response =
                new ApiResponse<IEnumerable<FacilityItem>>();
            response.Data = organizationalDataManager.GetAllFacilities();
            return Ok(response);
        }

        /// <summary>
        /// Method to get processes
        /// </summary>
        /// <returns>api response with processes list</returns>
        [Route("GetProcesses")]
        public IHttpActionResult GetProcesses()
        {
            ApiResponse<IEnumerable<ProcessItem>> response =
                new ApiResponse<IEnumerable<ProcessItem>>();
            response.Data = organizationalDataManager.GetAllProcesses();
            return Ok(response);
        }

        /// <summary>
        /// Api to add or update business segment
        /// </summary>
        /// <param name="businessSegmentRequest">list of business segment object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateBusinessSegments")]
        public IHttpActionResult AddOrUpdateBusinessSegments(List<BusinessSegmentItem> businessSegmentRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateBusinessSegments(businessSegmentRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to add or update divisions 
        /// </summary>
        /// <param name="divisionRequest">list of divisions object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateDivisions")]
        public IHttpActionResult AddOrUpdateDivisions(List<DivisionItem> divisionRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateDivisions(divisionRequest, Username);
            return Ok();
        }
               
        /// <summary>
        /// Api to add or update facility
        /// </summary>
        /// <param name="facilityRequest">list of facility object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateFacilities")]
        public IHttpActionResult AddOrUpdateFacilities(List<FacilityItem> facilityRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateFacilities(facilityRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to add or update product line
        /// </summary>
        /// <param name="productLineRequest">list of  product line object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateProductLines")]
        public IHttpActionResult AddOrUpdateProductLines(List<ProductLineItem> productLineRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateProductLines(productLineRequest, Username);
            return Ok();
        }
                
        /// <summary>
        /// Api to add or update department
        /// </summary>
        /// <param name="departmentRequest">list of  department object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateDepartments")]
        public IHttpActionResult AddOrUpdateDepartments(List<DepartmentItem> departmentRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateDepartments(departmentRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to add or update process
        /// </summary>
        /// <param name="processRequest">list of  process object</param>
        /// <returns>http response</returns>
        [Route("AddOrUpdateProcess")]
        public IHttpActionResult AddOrUpdateProcess(List<ProcessItem> processRequest)
        {
            CheckModelState();
            organizationalDataManager.AddOrUpdateProcess(processRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Method to get organizational data
        /// </summary>
        /// <returns>api response with organizational data</returns>
        [Route("GetOrganizationalData")]
        public IHttpActionResult GetOrganizationalData()
        {
            ApiResponse<IEnumerable<OrganizationalDataItem>> response =
                new ApiResponse<IEnumerable<OrganizationalDataItem>>();
            response.Data = organizationalDataManager.GetOrganizationalData();
            return Ok(response);
        }
        #endregion
    }
}
