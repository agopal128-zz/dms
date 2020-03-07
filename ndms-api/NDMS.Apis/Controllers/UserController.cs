using NDMS.Apis.Models;
using NDMS.DomainModel.DTOs;
using NDMS.Security;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace NDMS.Apis.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseController
    {
        #region Field(s)
        /// <summary>
        /// IUserManager reference
        /// </summary>
        private readonly Business.Interfaces.IUserManager userManager;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="userManager">IUserManager reference</param>
        public UserController(Business.Interfaces.IUserManager userManager)
        {
            this.userManager = userManager;
        }
        #endregion

        #region Action Method(s)
        /// <summary>
        /// Get all NDMS users whose first name or last name starts with the 
        /// input string mentioned.
        /// </summary>
        /// <param name="name">Input name to match</param>
        /// <returns>
        /// List of NDMS users whose first name or last name starts with the input string
        /// </returns>
        [HttpGet]
        [Route("GetNDMSUsersWithLastName/{name}")]
        [ResponseType(typeof(NDMSUserSuggestion))]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," +
            NDMSSecurityConstants.KPIOwnerRole)]
        public IHttpActionResult GetNDMSUsersWithLastName(string name)
        {
            ApiResponse<IEnumerable<NDMSUserSuggestion>> response = new
                ApiResponse<IEnumerable<NDMSUserSuggestion>>();
            response.Data = userManager.GetNDMSUsersWithLastName(name);
            return Ok(response);
        }

        /// <summary>
        /// Get all AD users whose account name or last name starts with the input string mentioned.
        /// </summary>
        /// <param name="name">Input name to match</param>
        /// <returns>
        /// List of AD users whose first name or last name starts with the input string
        /// </returns>
        [HttpGet]
        [Route("GetADUsersWithLastNameOrAccountName/{name?}")]
        [ResponseType(typeof(ADUserSuggestion))]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," +
            NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
        public IHttpActionResult GetADUsersWithLastNameOrAccountName(string name)
        {
            ApiResponse<IEnumerable<ADUserSuggestion>> response = new
                ApiResponse<IEnumerable<ADUserSuggestion>>();
            response.Data = userManager.GetADUsersWithLastNameOrAccountName(name);
            return Ok(response);
        }


        /// <summary>
        /// Api to retrieve scorecard id and check whether any targets are available for 
        /// the current logged in user
        /// </summary>
        /// <returns>HTTP response which conveys the status of the operation</returns>
        [Route("GetScorecardIdAndTargetStatus")]
        [ResponseType(typeof(ScorecardTargetStatusData))]
        [Authorize(Roles = NDMSSecurityConstants.AdminRole + "," +
            NDMSSecurityConstants.KPIOwnerRole + "," + NDMSSecurityConstants.TeamMemberRole)]
        public IHttpActionResult GetScorecardIdAndTargetStatus()
        {
            ApiResponse<ScorecardTargetStatusData> response = new
                ApiResponse<ScorecardTargetStatusData>();
            response.Data = userManager.GetScorecardIdAndTargetStatus(Username);
            if (response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }      

        #endregion
    }
}
