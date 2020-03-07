using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DataAccess.Interfaces
{
    /// <summary>
    /// Defines a repository for NDMS Users
    /// </summary>
    public interface INDMSUserRepository : IBaseRepository<User>
    {
        #region Method(s)
        /// <summary>
        /// Method to add AD users to NDMS Users if the user doesn't exists in table and return the
        /// corresponding identifier
        /// </summary>
        /// <param name="adUsers">list of account names of AD Users</param>
        /// <returns>List of User Identifiers</returns>
        List<int> AddADUsersToNDMS(IEnumerable<string> adUsers);
        #endregion
    }
}
