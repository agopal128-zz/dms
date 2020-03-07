using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Details of the current date
    /// </summary>
    public class CurrentDateDetails
    {
        #region Propertie(s)
        /// <summary>
        /// Current date
        /// </summary>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Current year identifier
        /// </summary>
        public int YearId { get; set; }
        #endregion
    }
}
