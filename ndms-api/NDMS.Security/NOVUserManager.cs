using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    public class NOVUserManager:IUserManager
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
        // ~NOVRoleManager() {
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
        public NOVUserManager(IBaseRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Get full name of the user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Full name of the user</returns>
        public string GetFullName(string username)
        {
            string fullname = string.Empty;
            var userInfo = userRepository.GetAll().Where(x => 
                x.AccountName == username && x.IsActive).FirstOrDefault();
            if (userInfo != null)
            {
                fullname = userInfo.FirstName + " " + userInfo.LastName;
            }
            return fullname;
        }
        #endregion
    }
}
