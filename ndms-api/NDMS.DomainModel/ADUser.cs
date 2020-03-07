using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents an Active Directory User
    /// </summary>
    public class ADUser
    {
        #region Propertie(s)
        /// <summary>
        /// Windows Account name
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// E-mail of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Last location ID
        /// </summary>
        public int? LastLocationID { get; set; }
        #endregion
    }
}
