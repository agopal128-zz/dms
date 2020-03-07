using NDMS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDMS.DomainModel;
using System.Linq.Expressions;

namespace NDMS.DataAccess.Repositories
{
    public class ADUserRepository : IADUserRepository
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public ADUserRepository()
        {
            this.Context = new NDMSDataContext();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="context">Database Context</param>
        public ADUserRepository(NDMSDataContext context)
        {
            this.Context = context;
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// DBContext associated with this repository
        /// </summary>
        public NDMSDataContext Context
        {
            get;
            protected set;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves ADUser based on Account ID
        /// </summary>
        /// <param name="accountName">Account name to match</param>
        /// <returns>AD user with matching account name if found, otherwise null</returns>
        public ADUser Get(string accountName)
        {
            return Context.Set<ADUser>().Where(x => 
                x.AccountName == accountName).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all AD Users
        /// </summary>
        /// <returns>All entities available in the repository</returns>
        public IQueryable<ADUser> GetAll()
        {
            return Context.Set<ADUser>();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
