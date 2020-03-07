using NDMS.DataAccess.Repositories;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    /// <summary>
    /// Factory to return the authentication service requested
    /// </summary>
    public class AuthenticationServiceProvider
    {
        #region Public Method(s)
        /// <summary>
        /// Returns the authentication service requested
        /// </summary>
        /// <param name="authSvcIdentifier">Unique string to identify the auth service</param>
        /// <returns>Requested auth service</returns>
        public static IAuthenticationService GetAuthenticationService(string authSvcIdentifier)
        {
            var userRepo = new BaseRepository<User>();
            switch (authSvcIdentifier)
            {
                case "NOV":
                    {
                        return new NOVAuthenticationService(userRepo);
                    }
                default:
                    {
                        return new NOVAuthenticationService(userRepo);
                    }
            }
        }
        #endregion
    }
}
