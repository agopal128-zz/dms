using NDMS.Apis.Models;
using NDMS.Business.Interfaces;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System.Web.Http;
using System.Web.Http.Description;

namespace NDMS.Apis.Controllers
{
    /// <summary>
    /// Web API Controller for scorecard admin features
    /// </summary>
    [RoutePrefix("api/ScorecardAdmin")]
    [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole)]
    public class ScorecardAdminController : BaseController
    {
        #region Field(s)
        
        /// <summary>
        /// IScorecardAdminManager reference
        /// </summary>
        private readonly IScorecardAdminManager scorecardAdminMgr;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly Business.Interfaces.IUserManager userManager;

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="scorecardAdminManager">IScorecardAdminManager reference</param>
        /// <param name="userManager">The user manager.</param>
        public ScorecardAdminController(IScorecardAdminManager scorecardAdminManager, Business.Interfaces.IUserManager userManager)
        {
            this.scorecardAdminMgr = scorecardAdminManager;
            this.userManager = userManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Retrieves scorecard by id
        /// </summary>
        /// <param name="id">Identifier of the score card</param>
        /// <returns>scorecard object</returns>
        [Route("{id}")]        
        public IHttpActionResult Get(int id)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canEditScorecard = userManager.IsUserKPIOwnerOfParentScorecard(Username, id);
                if (!canEditScorecard)
                {
                    return Unauthorized();
                }
            }
            ApiResponse<ScorecardItem> response = new ApiResponse<ScorecardItem>();
            response.Data = scorecardAdminMgr.GetScorecard(id);
            if (response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Api to add scorecard
        /// </summary>
        /// <param name="scorecardRequest">Scorecard add request</param>
        /// <returns>New scorecard's id</returns>
        [ResponseType(typeof(int))]
        [Route("AddScorecard")]    
        [HttpPost]    
        public IHttpActionResult Add(ScorecardItem scorecardRequest)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canAddScorecard = userManager.IsUserKPIOwnerOfScorecard(Username, scorecardRequest.ParentScorecardId.Value);
                if (!canAddScorecard)
                {
                    return Unauthorized();
                }
            }

            CheckModelState();
            ApiResponse<int> response = new ApiResponse<int>();
            response.Data = scorecardAdminMgr.AddScorecard(scorecardRequest, Username);
            return Ok(response);
        }

        /// <summary>
        /// Api to update a scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard object</param>
        /// <returns>scorecard id</returns>
        [Route("UpdateScorecard")]
        [HttpPut]
        public IHttpActionResult Put(ScorecardItem scorecardRequest)
        {
            if (!User.IsInRole(NDMSSecurityConstants.AdminRole))
            {
                var canEditScorecard = userManager.IsUserKPIOwnerOfParentScorecard(Username, scorecardRequest.Id.Value);
                if (!canEditScorecard)
                {
                    return Unauthorized();
                }
            }

            CheckModelState();
            if (!scorecardAdminMgr.UpdateScorecard(scorecardRequest, Username))
            {
                return NotFound();
            }
            return Ok();
        }

        ///<summary>
        ///API to change Sort Order
        ///</summary>
        [Route("ChangeSortOrder")]
        [HttpPut]
        [OverrideAuthorization]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole)]
        public IHttpActionResult SwapSortOrder([FromBody]SwapScorecardSortOrderRequest swapRequest)
        {
            if (!scorecardAdminMgr.SwapScorecardSortOrders(swapRequest, Username))
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        /// Api to display all the Scorecards in a hierarchy mode
        /// </summary>
        /// <param name="topScorecardId"></param>
        /// <param name="selectedScorecardId"></param>
        /// <returns> Scorecard Hierarchy</returns>
        [Route("GetScorecardHierarchy/{topScorecardId}/{selectedScorecardId}")]
        [OverrideAuthorization]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
        public IHttpActionResult GetScorecardHierarchy(int? topScorecardId, int? selectedScorecardId)
        {
            ApiResponse<ScorecardNode> response = new ApiResponse<ScorecardNode>();
            response.Data = scorecardAdminMgr.GetScorecardHierarchy(Username, topScorecardId, selectedScorecardId);
            return Ok(response);
        }

        ///<summary>
        /// Gets the scorecards of logged in user.
        /// </summary>
        ///<returns>The list of available templates</returns>
        [Route("GetHierarchyDropdownList")]
        [OverrideAuthorization]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," + NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
        [ResponseType(typeof(HierarchyMenuDTO))]
        public IHttpActionResult GetHierarchyDropdownList()
        {
            ApiResponse<HierarchyMenuDTO> response = new ApiResponse<HierarchyMenuDTO>();
            response.Data = scorecardAdminMgr.GetHierarchyDropdownList(Username);
            return Ok(response);
        }

        #endregion

    }
}
