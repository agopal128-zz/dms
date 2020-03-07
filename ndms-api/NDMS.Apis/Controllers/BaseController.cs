using NDMS.Helpers;
using NDMS.Security;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace NDMS.Apis.Controllers
{
    public class BaseController : ApiController
    {
        #region Protected Method(s)
        /// <summary>
        /// Method to validate model state
        /// </summary>
        protected void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(a => a.Errors).Select(b => b.ErrorMessage)
                    .Where(c => !string.IsNullOrEmpty(c)).ToList();
                throw new NDMSBusinessException(errors);
            }
        }

        /// <summary>
        /// Check if the user is a kpi owner but not an admin
        /// </summary>
        /// <returns></returns>
        protected bool IsUserKPIOwnerNotAdmin()
        {
            var userRoles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            return !userRoles.Contains(NDMSSecurityConstants.AdminRole) &&
                userRoles.Contains(NDMSSecurityConstants.KPIOwnerRole);
        }

        /// <summary>
        /// Determines whether [is user kpi owner or team member not admin].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is user kpi owner or team member not admin]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsUserKPIOwnerOrTeamMemberNotAdmin()
        {
            var userRoles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            return !userRoles.Contains(NDMSSecurityConstants.AdminRole) &&
                (userRoles.Contains(NDMSSecurityConstants.KPIOwnerRole) ||
                userRoles.Contains(NDMSSecurityConstants.TeamMemberRole));
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseController()
        {
            var user = HttpContext.Current.User;
            Username = user.Identity.IsAuthenticated ? user.Identity.Name : null;
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Authenticated user name
        /// </summary>
        public string Username
        {
            get;
            private set;
        }
        #endregion
    }
}
