using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    /// <summary>
    /// Defines methods to deal with user database
    /// </summary>
    public interface IUserManager:IDisposable
    {
        /// <summary>
        /// Get full name of the user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Full name of the user</returns>
        string GetFullName(string username);
    }
}
