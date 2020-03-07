using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.Helpers;
using NDMS.Logger;
using System;
using System.Configuration;
using System.DirectoryServices;

namespace NDMS.Security
{
    /// <summary>
    /// Implements the authentication service for NOV
    /// </summary>
    public class NOVAuthenticationService : IAuthenticationService
    {
        #region Field(s)
        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~NOVAuthenticationService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="userRepository">User Repository</param>
        public NOVAuthenticationService(IBaseRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Performs authentication with the credentials supplied
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>True, if authenticated successfully</returns>
        public bool Authenticate(string username, string password)
        {
            bool authenticated = false;
            try
            {
                string adServer = ConfigurationManager.AppSettings[AppSettingsKeys.ADServer];
                string usernameWithoutDomain = username.Substring(username.LastIndexOf(@"\") + 1);

                // Here is a temporary code which will help us to work without AD during development
                // Below if block needs to be removed when we move it to production.
                if (string.IsNullOrEmpty(adServer))
                {
                    authenticated = true;
                }
                else
                {
                    string activeDirectoryPath = string.Format("LDAP://{0}", adServer);
                    LogManager.Instance.Log(string.Format("The LDAP Url used is {0}",
                        activeDirectoryPath), LogLevelEnum.Information);
                    DirectoryEntry entry = new DirectoryEntry(activeDirectoryPath,
                        username, password);
                    // Bind to the native AdsObject to force authentication.
                    object obj = entry.NativeObject;
                    DirectorySearcher search = new DirectorySearcher(entry);
                    search.Filter = string.Format("(SAMAccountName={0})", usernameWithoutDomain);
                    search.PropertiesToLoad.Add("cn");
                    SearchResult result = search.FindOne();
                    if (result == null)
                    {
                        string msg = "Failed to perform Active Directory authentication";
                        LogManager.Instance.Log(msg, LogLevelEnum.Error);
                        authenticated = false;
                    }
                    else
                    {
                        authenticated = true;
                    }
                }
            }
            catch (Exception exception)
            {
                // Log the exception
                LogManager.Instance.Log(exception.ToString(), LogLevelEnum.Error);
                string msg = "Failed to perform Active Directory authentication";
                LogManager.Instance.Log(msg, LogLevelEnum.Error);
                return false;
            }
            return authenticated;
        }
        #endregion
    }
}
