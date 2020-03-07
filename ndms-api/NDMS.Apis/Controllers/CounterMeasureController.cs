using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// Web API Controller to deal with CounterMeasure 
    /// </summary>
    [RoutePrefix("api/CounterMeasure")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
    public class CounterMeasureController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// ICounterMeasureManager reference
        /// </summary>
        private readonly ICounterMeasureManager counterMeasureManager;

        /// <summary>
        /// IUserManager reference
        /// </summary>
        private readonly Business.Interfaces.IUserManager userManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="counterMeasureManager">ICounterMeasureManager reference</param>
        /// <param name="userManager">IUserManager reference</param>
        public CounterMeasureController(ICounterMeasureManager counterMeasureManager,
            Business.Interfaces.IUserManager userManager)
        {
            this.counterMeasureManager = counterMeasureManager;
            this.userManager = userManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Api to list all counter measure for the selected year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="isShowClosed">Flag to show closed counter measures</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetCounterMeasures/{scorecardId}/{kpiId}/{isShowClosed}")]
        [ResponseType(typeof(IEnumerable<CounterMeasureItem>))]
        [AllowAnonymous]
        public IHttpActionResult GetCounterMeasures(int scorecardId, int kpiId, bool isShowClosed)
        {
            ApiResponse<IEnumerable<CounterMeasureItem>> response =
                                new ApiResponse<IEnumerable<CounterMeasureItem>>();
            response.Data = counterMeasureManager.GetCounterMeasures(scorecardId, kpiId, isShowClosed);
            return Ok(response);
        }

        /// <summary>
        /// Api to list all counter measure status
        /// </summary>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetAllCounterMeasureStatus")]
        [ResponseType(typeof(IEnumerable<CounterMeasureStatusItem>))]
        [AllowAnonymous]
        public IHttpActionResult GetAllCounterMeasureStatus()
        {
            ApiResponse<IEnumerable<CounterMeasureStatusItem>> response =
                                new ApiResponse<IEnumerable<CounterMeasureStatusItem>>();
            response.Data = counterMeasureManager.GetAllCounterMeasureStatus();
            return Ok(response);
        }

        /// <summary>
        /// Api to check whether target is achieved or not by comparing goal and actuals based on 
        /// goal type and return status and outstanding counter measure details if any 
        /// </summary>
        /// <param name="request">Input Request</param>
        /// <returns></returns>
        [Route("GetActualandCounterMeasureStatus")]
        [ResponseType(typeof(ActualandCounterMeasureStatus))]
        public IHttpActionResult GetActualandCounterMeasureStatus(
            [FromUri]ActualStatusRequest request)
        {
            ApiResponse<ActualandCounterMeasureStatus> response =
                                   new ApiResponse<ActualandCounterMeasureStatus>();
            response.Data = counterMeasureManager.GetActualandCounterMeasureStatus(request);
            return Ok(response);
        }

        /// <summary>
        /// Api to get counter measure details
        /// </summary>
        /// <param name="counterMeasureId">counter measure id</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetCounterMeasure/{counterMeasureId}")]
        [ResponseType(typeof(CounterMeasureItem))]
        public IHttpActionResult GetCounterMeasure(int counterMeasureId)
        {
            ApiResponse<CounterMeasureItem> response = new ApiResponse<CounterMeasureItem>();
            response.Data = counterMeasureManager.GetCounterMeasure(counterMeasureId);
            return Ok(response);
        }

        /// <summary>
        /// Api to add new counter measure when target is not met
        /// </summary>
        /// <param name="counterMeasureRequest">counter measure object</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("AddCounterMeasure")]
        public IHttpActionResult PostCounterMeasure(CounterMeasureAddRequest counterMeasureRequest)
        {
            CheckModelState();
            //if user is kpi owner not an admin, check if he is the kpi owner of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, counterMeasureRequest.ScorecardId.Value);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, counterMeasureRequest.ScorecardId.Value);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }

            counterMeasureManager.AddCounterMeasure(counterMeasureRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to update an existing counter measure 
        /// </summary>
        /// <param name="counterMeasureRequest">counter measure object</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("EditCounterMeasure")]
        public IHttpActionResult PutCounterMeasure(CounterMeasureEditRequest counterMeasureRequest)
        {
            CheckModelState();

            //if user is kpi owner not an admin, check if he is the kpi owner of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, counterMeasureRequest.ScorecardId.Value);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, counterMeasureRequest.ScorecardId.Value);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }

            counterMeasureManager.EditCounterMeasure(counterMeasureRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Method to retrieve metrics belonging to a kpi in a scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year Id</param>
        /// <param name="month">Month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardKPIMetrics/{scorecardId}/{kpiId}/{yearId}/{month}")]
        [ResponseType(typeof(IEnumerable<MetricItem>))]
        [AllowAnonymous]
        public IHttpActionResult GetScorecardKPIMetrics(int scorecardId, int kpiId, int month,
            int yearId)
        {
            ApiResponse<IEnumerable<MetricItem>> response =
                                new ApiResponse<IEnumerable<MetricItem>>();
            response.Data = counterMeasureManager.GetScorecardKPIMetrics(
                scorecardId, kpiId, month, yearId);
            return Ok(response);
        }

        /// <summary>
        /// Api to list all counter measure priority
        /// </summary>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetAllCounterMeasurePriority")]
        [ResponseType(typeof(IEnumerable<CounterMeasurePriorityItem>))]
        [AllowAnonymous]
        public IHttpActionResult GetAllCounterMeasurePriority()
        {
            ApiResponse<IEnumerable<CounterMeasurePriorityItem>> response =
                                new ApiResponse<IEnumerable<CounterMeasurePriorityItem>>();
            response.Data = counterMeasureManager.GetAllCounterMeasurePriority();
            return Ok(response);
        }
        #endregion
    }
}
