using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NDMS.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NDMS.Apis.Providers
{
    /// <summary>
    /// Implements an OAuth provider for the API
    /// </summary>
    public class NDMSApiOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region Public Method(s)
        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", 
        /// and that the correct credentials for that client are present on the request.
        /// </summary>
        /// <param name="context">Context information</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". 
        /// This occurs when the user has provided name and password credentials directly into the 
        /// client application's user interface, and the client application is using those 
        /// to acquire an "access_token" and optional "refresh_token"
        /// </summary>
        /// <param name="context">Context information</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (IAuthenticationService authService =
                AuthenticationServiceProvider.GetAuthenticationService("NOV"))
            {
                bool authResult = authService.Authenticate(context.UserName, context.Password);

                if (authResult == false)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            string usernameWithoutDomain = context.UserName.Substring(
                context.UserName.LastIndexOf(@"\") + 1);

            string fullName;
            using (IUserManager userMgr = UserManagerProvider.GetUserManager("NOV"))
            {
                fullName = userMgr.GetFullName(usernameWithoutDomain);
            }

            List<string> roles;
            using (IRoleManager roleManager = RoleManagerProvider.GetRoleManager("NOV"))
            {
                roles = roleManager.GetUserRoles(usernameWithoutDomain);
            }

            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, usernameWithoutDomain));
            foreach (var role in roles)
            {
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            AuthenticationProperties properties = CreateProperties(usernameWithoutDomain, 
                fullName, roles);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Create Authentication properties
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="fullName">Full user name</param>
        /// <param name="roles">Roles associated</param>
        /// <returns>Auth propertries created</returns>
        public static AuthenticationProperties CreateProperties(string username,
            string fullName, List<string> roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "user", username },
                { "fullname", fullName},
                { "role", string.Join(",", roles) }
            };
            return new AuthenticationProperties(data);
        }
        #endregion
    }
}