using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// Web API Controller to deal with Actuals entry 
    /// </summary>
    [RoutePrefix("api/Actuals")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
    public class ActualsController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// IScorecardManager reference
        /// </summary>
        private readonly IScorecardManager scorecardManager;

        /// <summary>
        /// IUserManager reference
        /// </summary>
        private readonly Business.Interfaces.IUserManager userManager;

        /// <summary>
        /// IActualsManager reference
        /// </summary>
        private readonly IActualsManager actualsManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="scorecardManager">IScorecardManager reference</param>
        /// <param name="actualsManager">IActualsManager reference</param>
        /// <param name="userManager">IUserManager reference</param>
        public ActualsController(IScorecardManager scorecardManager,
            IActualsManager actualsManager,
            Business.Interfaces.IUserManager userManager)
        {
            this.scorecardManager = scorecardManager;
            this.actualsManager = actualsManager;
            this.userManager = userManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Api to retrieve daily target of a day in a month
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="day">selected day</param>
        /// <param name="month">selected month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetDailyTarget/{targetId}/{month}/{day}")]
        [ResponseType(typeof(decimal?))]
        public IHttpActionResult GetDailyTarget(int targetId, int month, int day)
        {
            ApiResponse<decimal?> response = new ApiResponse<decimal?>();
            response.Data = scorecardManager.GetDailyGoal(targetId, month, day);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve monthly target of a month
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="month">selected month</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetMonthlyTarget/{targetId}/{month}")]
        [ResponseType(typeof(decimal?))]
        public IHttpActionResult GetMonthlyTarget(int targetId, int month)
        {
            ApiResponse<decimal?> response = new ApiResponse<decimal?>();
            response.Data = scorecardManager.GetMonthlyGoal(targetId, month);
            return Ok(response);
        }

        /// <summary>
        /// Api to check if a User is allowed to enter actuals, Mark holiday or enter counter measure.
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        [Route("IsUserAuthorizedToAlterScorecardEntries/{scorecardId}")]
        [ResponseType(typeof(bool))]
        [HttpGet]
        public IHttpActionResult IsUserAdminOrKpiOwnerOrTeamMemberofScorecard(int scorecardId)
        {
            //if user is kpi owner not an admin, check if he is the kpi owner/team member of the given 
            //scorecard else return unauthorized
            bool isUserKPIOwnerOrTeamMemberofScorecard = false;
            bool isUserAdmin = false;
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, scorecardId);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, scorecardId);
                isUserKPIOwnerOrTeamMemberofScorecard = isUserKPIOwnerOfScorecard || isUserTeamMemberOfScorecard;
            }
            else if (User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                isUserAdmin = true;
            }
            ApiResponse<bool> resonse = new ApiResponse<bool>();
            resonse.Data = isUserAdmin || isUserKPIOwnerOrTeamMemberofScorecard;
            return Ok(resonse);
        }

        /// <summary>
        /// Api to add a new daily/monthly actual value
        /// </summary>
        /// <param name="actualRequest">actual request object</param>
        /// <returns>New actual entry's Id</returns>
        [Route("AddActual")]
        [ResponseType(typeof(int))]
        public IHttpActionResult AddActual(ActualItem actualRequest)
        {
            //if user is kpi owner not an admin, check if he is the kpi owner/team member of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, actualRequest.ScorecardId.Value);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, actualRequest.ScorecardId.Value);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }

            ApiResponse<int> response = new ApiResponse<int>();
            response.Data = actualsManager.AddActual(actualRequest, Username);
            return Ok(response);
        }

        /// <summary>
        /// Api to update a daily/monthly actual value
        /// </summary>
        /// <param name="actualRequest">actual request object</param>
        /// <returns>Status of the actual value against the target entry</returns>
        [Route("EditActual")]
        [ResponseType(typeof(ActualStatus))]
        public IHttpActionResult PutActual(ActualItem actualRequest)
        {
            //if user is kpi owner not an admin, check if he is the kpi owner/team member of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, actualRequest.ScorecardId.Value);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, actualRequest.ScorecardId.Value);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }

            actualsManager.UpdateActual(actualRequest, Username);
            return Ok();
        }

        /// <summary>
        /// Api to mark a day as holiday
        /// </summary>
        /// <param name="targetId">target id of the primary metric</param>
        /// <param name="date">Date which needs to be marked as holiday</param>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <returns>New/updated actual entry's Id</returns>
        [Route("MarkHoliday/{scorecardId}/{targetId}/{date}")]
        [ResponseType(typeof(int))]
        public IHttpActionResult MarkHoliday(int scorecardId, int targetId, DateTime date)
        {
            //if user is kpi owner not an admin, check if he is the kpi owner/team member of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, scorecardId);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, scorecardId);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }

            ApiResponse<int> response = new ApiResponse<int>();
            response.Data = actualsManager.MarkHolidayOrWorkday(targetId, date, false, Username);
            return Ok(response);
        }

        /// <summary>
        /// Api to unmark a day which has been marked as holiday
        /// </summary>
        /// <param name="targetId">target id of the primary metric</param>
        /// <param name="date">Date which needs to be unmarked as holiday</param>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <returns>New/updated actual entry's Id</returns>
        [Route("UnmarkHoliday/{scorecardId}/{targetId}/{date}")]
        public IHttpActionResult UnmarkHoliday(int scorecardId, int targetId, DateTime date)
        {
            //if user is kpi owner not an admin, check if he is the kpi owner/team member of the given 
            //scorecard else return unauthorized
            if (IsUserKPIOwnerOrTeamMemberNotAdmin())
            {
                bool isUserKPIOwnerOfScorecard = userManager.
                     IsUserKPIOwnerOfScorecard(Username, scorecardId);
                bool isUserTeamMemberOfScorecard = userManager.
                     IsUserTeamMemberOfScorecard(Username, scorecardId);
                if (!isUserKPIOwnerOfScorecard && !isUserTeamMemberOfScorecard)
                {
                    return Unauthorized();
                }
            }
           
            ApiResponse<int> response = new ApiResponse<int>();
            response.Data = actualsManager.MarkHolidayOrWorkday(targetId, date, true, Username);
            return Ok(response);
        }
        
        /// <summary>
        /// Api to retrieve scorecard hierarchy with status corresponding to selected scorecard kpi
        /// </summary>
        /// <param name="scorecardId">scorecard id</param>
        /// <param name="kpiId">kpi id</param>
        /// <param name="month">selected month</param>
        /// <param name="yearId">selected year Id</param>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetDrillDownHierarchy/{scorecardId}/{kpiId}/{month}/{yearId}")]
        [ResponseType(typeof(ScorecardDrilldownNode))]
        public IHttpActionResult GetDrillDownHierarchy(int scorecardId, int kpiId, int month,
            int yearId)
        {
            ApiResponse<ScorecardDrilldownNode> response = new
                ApiResponse<ScorecardDrilldownNode>();
            response.Data = scorecardManager.GetDrillDownHierarchy(scorecardId, kpiId,
                month, yearId);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve scorecard hierarchy with status corresponding to selected scorecard kpi for the selected date
        /// </summary>
        /// <param name="scorecardId">scorecard id</param>
        /// <param name="kpiId">kpi id</param>
        /// <param name="date">The date.</param>
        /// <returns>
        /// HTTP response which conveys the status of the operation
        /// </returns>
        [Route("GetDrillDownHierarchyOnDate/{scorecardId}/{kpiId}/{date}")]
        [ResponseType(typeof(ScorecardDrilldownNode))]
        public IHttpActionResult GetDrillDownHierarchyOnDate(int scorecardId, int kpiId, DateTime date)
        {
            ApiResponse<ScorecardDrilldownNode> response = new
                ApiResponse<ScorecardDrilldownNode>();
            response.Data = scorecardManager.GetDrillDownHierarchy(scorecardId, kpiId,date);
            return Ok(response);
        }

        /// <summary>
        /// Api to retrieve scorecard Kpi's
        /// </summary>
        /// <param name="scorecardId">Identifier of scorecard</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">Identifier of calendar year</param>
        /// <returns>
        /// List of kpis and its associated targets
        /// </returns>
        [Route("GetScorecardKPIs/{scorecardId}/{month}/{yearId}")]
        [ResponseType(typeof(List<KPIItem>))]
        public IHttpActionResult GetScorecardKPIs(int scorecardId, int month, int yearId)
        {
            ApiResponse<List<KPIItem>> response = new ApiResponse<List<KPIItem>>();
            response.Data = scorecardManager.GetScorecardKPIs(scorecardId, month, yearId);
            return Ok(response);
        }

        #endregion
    }
}
