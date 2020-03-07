using System;

namespace NDMS.Security
{
    /// <summary>
    /// Defines methods related to authenticating a user to NDMS
    /// </summary>
    public interface IAuthenticationService : IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Performs authentication with the credentials supplied
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True, if authenticated successfully</returns>
        bool Authenticate(string username, string password);
        #endregion
    }
}
