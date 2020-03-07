using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    /// <summary>
    /// Contains methods to implement a role manager for NDMS
    /// </summary>
    public interface IRoleManager : IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Retrieves the roles associated with a user
        /// </summary>
        /// <param name="username">Input user name</param>
        /// <returns>Roles associated with the user</returns>
        List<string> GetUserRoles(string username);
        #endregion
    }
}
