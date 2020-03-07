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
    /// API Controller for Targets module
    /// </summary>
    [RoutePrefix("api/Target")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole)]
    public class TargetsController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// ITargetManager reference
        /// </summary>
        private readonly ITargetManager targetManager;

        ///<summary>
        ///IUserManager reference
        /// </summary>
        private readonly Business.Interfaces.IUserManager userManager;

        /// <summary>
        /// IScorecardManager reference
        /// </summary>
        private readonly IScorecardManager scorecardManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetManager">ITargetManager reference</param>
        /// <param name="userManager">IUserManager reference</param>
        public TargetsController(ITargetManager targetManager,
            Business.Interfaces.IUserManager userManager,
            IScorecardManager scorecardManager)
        {
            this.targetManager = targetManager;
            this.userManager = userManager;
            this.scorecardManager = scorecardManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Api to retrieve initial data to load targets screen (year, roll up method list)
        /// and bool to check whether bowling chart is applicable or not
        /// </summary>
        /// <param name="scorecardId">Identifier of scorecard</param>
        /// <param name="canEdit">Flag to know if page is rendered in viewmode or edit mode.</param>
        /// <returns>object with year and roll up method list</returns>
        [Route("GetTargetsInitialData/{scorecardId}/{canEdit}")]
        [ResponseType(typeof(TargetTemplateData))]
        public IHttpActionResult GetTargetsInitialData(int scorecardId, bool canEdit)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var isUserKPIOwnerofScorecard = userManager.IsUserKPIOwnerOfScorecard(Username, scorecardId);
                var isUserKPIOwnerOfParentScorecard = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId);
                if (canEdit)
                {
                    if(!isUserKPIOwnerOfParentScorecard)
                    {
                        return Unauthorized();
                    }
                }
                else 
                {
                    var canUserViewTargets = isUserKPIOwnerofScorecard || isUserKPIOwnerOfParentScorecard;
                    if (!canUserViewTargets)
                    {
                        return Unauthorized();
                    }
                }
            }
            ApiResponse<TargetTemplateData> response = new ApiResponse<TargetTemplateData>();
            response.Data = targetManager.GetTargetsInitialData(scorecardId);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve scorecard Kpi's
        /// </summary>
        /// <param name="scorecardId">Identifier of scorecard</param>
        /// <param name="yearId">Identifier of calendar year</param>
        /// <returns>List of kpis and its associated targets</returns>
        [Route("GetScorecardKPIs/{scorecardId}/{yearId}")]
        [ResponseType(typeof(List<KPIItem>))]
        public IHttpActionResult GetScorecardKPIs(int scorecardId, int yearId)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var isUserKPIOwnerofScorecard = userManager.IsUserKPIOwnerOfScorecard(Username, scorecardId);
                var isuserKPIOwnerOfParentScorecard = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId);
                var canUserViewTargets = isUserKPIOwnerofScorecard || isuserKPIOwnerOfParentScorecard;
                if (!canUserViewTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<List<KPIItem>> response = new ApiResponse<List<KPIItem>>();
            response.Data = scorecardManager.GetScorecardKPIs(scorecardId, yearId);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all targets for a scorecard and KPI belongs to an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>List of target entries</returns>
        [Route("GetTargetsForScorecardAndKPI/{scorecardId}/{kpiId}/{yearId}")]
        [ResponseType(typeof(IEnumerable<TargetItem>))]
        public IHttpActionResult GetTargetsForScorecardAndKPI(int scorecardId, int kpiId,
            int yearId)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var isUserKPIOwnerofScorecard = userManager.IsUserKPIOwnerOfScorecard(Username, scorecardId);
                var isUserKPIOwnerOfParentScorecard = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId);
                var canUserViewTargets = isUserKPIOwnerofScorecard || isUserKPIOwnerOfParentScorecard;
                if (!canUserViewTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<IEnumerable<TargetItem>> response = new
                ApiResponse<IEnumerable<TargetItem>>();
            response.Data = targetManager.GetTargetsForScorecardAndKPI(scorecardId, kpiId,
                yearId);
            return Ok(response);
        }

        /// <summary>
        /// API to retrieve daily targets for a month
        /// </summary>
        /// <param name="monthlyTargetId">Monthly Target Id</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetDailyTargets/{monthlyTargetId}")]
        [ResponseType(typeof(IEnumerable<DailyTargetItem>))]
        public IHttpActionResult GetDailyTargets(int monthlyTargetId)
        {
            ApiResponse<IEnumerable<DailyTargetItem>> response = new
                ApiResponse<IEnumerable<DailyTargetItem>>();
            response.Data = targetManager.GetDailyTargets(monthlyTargetId);
            return Ok(response);
        }

        /// <summary>
        /// Gets the list of available workdays of the month with monthly goal distributed evenly if applicable
        /// </summary>
        /// <param name="request"></param>
        /// object containing scorecardId, MetricId, YearId, MonthId, Effective Dates, Goal value/DailyRate
        /// <returns>List of system generated daily Target </returns>
        [HttpGet]
        [Route("GenerateDailyTargets")]
        [ResponseType(typeof(IEnumerable<DailyTargetItem>))]
        public IHttpActionResult GenerateDailyTargets([FromUri] GenerateDailyTargetsRequest request)
        {
            ApiResponse<IEnumerable<DailyTargetItem>> response = new
                ApiResponse<IEnumerable<DailyTargetItem>>();
            response.Data = targetManager.GenerateDailyTargetsList(request);
            return Ok(response);
        }


        /// <summary>
        /// Method to retrieve all metrics associated with selected KPI 
        /// having organization data same as that of selected scorecard
        /// </summary>
        /// <param name="kpiId">KPI id</param>
        /// <param name="scorecardId">Scorecard id</param>
        /// <returns>Metric list</returns>
        [Route("GetMetrics/{kpiId}/{scorecardId}")]
        [ResponseType(typeof(List<MetricItem>))]
        public IHttpActionResult GetMetrics(int kpiId, int scorecardId)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserEditTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId);
                if (!canUserEditTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<IEnumerable<MetricItem>> response = new ApiResponse<
                IEnumerable<MetricItem>>();
            response.Data = targetManager.GetMetrics(kpiId, scorecardId);
            return Ok(response);
        }

        /// <summary>
        /// Method to check whether parent scorecard has the same metric set for the KPI selected 
        /// by child scorecard
        /// </summary>
        /// <param name="request">
        /// object containing parent scorecard id and the selected metric id,
        /// kpi id and metric type of child scorecard
        /// </param>
        /// <returns>Target details if target can be cascaded</returns>
        [Route("GetCascadedMetricDetails")]
        [ResponseType(typeof(CascadedParentTargetItem))]
        public IHttpActionResult GetCascadedMetricDetails([FromUri] MetricCascadeRequest request)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserEditTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, request.ScorecardId); 
                if (!canUserEditTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<CascadedParentTargetItem> response = new ApiResponse<
                CascadedParentTargetItem>();
            response.Data = targetManager.GetCascadedMetricDetails(request);
            return Ok(response);
        }

        /// <summary>
        /// Returns month's list for a calendar year in ascending order
        /// </summary>
        /// <param name="yearId">Calendar year id</param>
        /// <returns>Months list</returns>
        [Route("GetMonthsListForCalendarYear/{yearId}")]
        [ResponseType(typeof(IEnumerable<MonthItem>))]
        public IHttpActionResult GetMonthsListForCalendarYear(int yearId)
        {
            ApiResponse<IEnumerable<MonthItem>> response = new ApiResponse<
               IEnumerable<MonthItem>>();
            response.Data = targetManager.GetMonthsListForCalendarYear(yearId);
            if (response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// API to add a new primary target which belongs to a scorecard and KPI
        /// </summary>
        /// <param name="targetRequest">Target entry</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("AddMetricTarget")]
        public IHttpActionResult PostMetricTarget(TargetItem targetRequest)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserEditTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, targetRequest.ScorecardId);
                if (!canUserEditTargets)
                {
                    return Unauthorized();
                }
            }
            CheckModelState();
            targetManager.AddMetricTarget(targetRequest, Username);
            return Ok();
        }

        /// <summary>
        /// API to Update target for a primary metric which belongs to a scorecard and KPI
        /// </summary>
        /// <param name="targetRequest">target entry to be updated</param>
        /// <returns>http response</returns>
        [Route("EditMetricTarget")]
        public IHttpActionResult PutMetricTarget(TargetItem targetRequest)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserEditTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, targetRequest.ScorecardId);
                if (!canUserEditTargets)
                {
                    return Unauthorized();
                }
            }
            CheckModelState();
            targetManager.EditMetricTarget(targetRequest, Username);
            return Ok();
        }

        /// <summary>
        /// API to check whether the targets for cascaded metric set on a score card is cascaded 
        /// completely to child score cards
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="metricId">Metric Id</param>
        /// <param name="calendarYearId">Calendar year Id</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("IsMetricTargetsCascadedCompletely/{scorecardId}/{kpiId}/{metricId}/{calendarYearId}")]
        [ResponseType(typeof(bool))]
        [HttpGet]
        public IHttpActionResult IsMetricTargetsCascadedCompletely(int scorecardId, int kpiId,
              int metricId, int calendarYearId)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Data = targetManager.IsMetricTargetsCascadedCompletely(
                scorecardId, kpiId, metricId, calendarYearId);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the maximum allowed monthly goal for a new/existing target which is
        /// going to be/being cascaded
        /// </summary>
        /// <param name="parentTargetId">Parent target Id</param>
        /// <param name="rollUpMethodId">Selected rollup method id</param>
        /// <param name="childTargetId">Child target Id(if child target is existing)</param>
        /// <param name="targetEntryMethodId">The target entry method identifier.</param>
        /// <returns>
        /// Maximum allowed monthly goals
        /// </returns>
        [Route("GetMaximumAllowedMonthlyGoals/{parentTargetId}/{rollUpMethodId}/{targetEntryMethodId}/{childTargetId?}")]
        [ResponseType(typeof(IEnumerable<MonthAndTarget>))]
        [HttpGet]
        public IHttpActionResult GetMaximumAllowedMonthlyGoals([FromUri] int 
            parentTargetId, [FromUri] int rollUpMethodId, 
            [FromUri] int? childTargetId = null, [FromUri] int? targetEntryMethodId = null)
        {
            ApiResponse<IEnumerable<MonthAndTarget>> response = new
                ApiResponse<IEnumerable<MonthAndTarget>>();
            response.Data = targetManager.GetMaximumAllowedMonthlyGoals(parentTargetId, 
                childTargetId, rollUpMethodId, targetEntryMethodId);
            return Ok(response);
        }

        ///<summary>
        /// Deletes a specific metric target if the metric has no actuals entered. This also deletes the 
        /// target from history
        ///</summary>
        /// <param name="targetId">target Id</param>
        /// <param name="scorecardId">scorecard Id</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("DeleteMetricTarget/{scorecardId}/{targetId}")]
        [ResponseType(typeof(bool))]
        [HttpDelete]
        public IHttpActionResult DeleteMetricTarget(int scorecardId, int targetId)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserDeleteTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId); ;
                if (!canUserDeleteTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Data = targetManager.DeleteMetricTarget(targetId, Username);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all targets for a scorecard belongs to an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>List of target entries</returns>
        [Route("GetTargetsForScorecard/{scorecardId}/{yearId}")]
        [ResponseType(typeof(IEnumerable<CopiedKPIItem>))]
        public IHttpActionResult GetTargetsForScorecard(int scorecardId,  int yearId)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserCopyTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId); ;
                if (!canUserCopyTargets)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<IEnumerable<CopiedKPIItem>> response = new
                ApiResponse<IEnumerable<CopiedKPIItem>>();
            response.Data = targetManager.GetSelectedYearTargetsForScorecard(scorecardId, yearId);
            return Ok(response);
        }

        /// <summary>
        /// Copies the targets.
        /// </summary>
        /// <param name="scorecardId">the scorecard Id</param>
        /// <param name="targetRequest">The target request.</param>
        /// <returns>
        /// true is copied, otherwise false
        /// </returns>
        [Route("CopyTargets/{scorecardId}")]
        public IHttpActionResult PostCopyTargets([FromUri]int scorecardId,[FromBody] CopiedScorecardItem targetRequest)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canUserCopyTargets = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardId);
                if (!canUserCopyTargets)
                {
                    return Unauthorized();
                }
            }
            targetManager.CopyTargets(targetRequest, Username);
            return Ok();
        }

        [Route("GetRolledUpTargets/{targetId}/{targetEntryMethodId}/{mtdPerformanceTrackingId}")]
        [ResponseType(typeof(IEnumerable<MonthAndTarget>))]
        public IHttpActionResult GetRolledUpTargets(int targetId, int targetEntryMethodId, int mtdPerformanceTrackingId)
        {
            ApiResponse<IEnumerable<MonthAndTarget>> response = new
                ApiResponse<IEnumerable<MonthAndTarget>>();
            response.Data = targetManager.GetRolledupGoals(targetId, targetEntryMethodId, mtdPerformanceTrackingId);
            return Ok(response);
        }

        #endregion

    }
}
