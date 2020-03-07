using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.Interfaces
{
    /// <summary>
    /// Defines a repository for AD Users
    /// </summary>
    public interface IADUserRepository: IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Retrieves ADUser based on Account Name
        /// </summary>
        /// <param name="accountName">Account name to match</param>
        /// <returns>AD user with matching account id if found, otherwise null</returns>
        ADUser Get(string accountName);

        /// <summary>
        /// Retrieves all AD Users
        /// </summary>
        /// <returns>All entities available in the repository</returns>
        IQueryable<ADUser> GetAll();
        #endregion
    }
}
