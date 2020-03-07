using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Represents a suggestion for AD user
    /// </summary>
    public class ADUserSuggestion
    {
        #region Propertie(s)
        /// <summary>
        /// Account name of the user
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; }
        #endregion
    }
}
