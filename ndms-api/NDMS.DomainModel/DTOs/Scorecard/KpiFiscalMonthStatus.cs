using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent fiscal month status of a KPI for an year
    /// </summary>
    public class KpiFiscalMonthStatus
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public KpiFiscalMonthStatus()
        {
            this.FiscalMonthStatusList = new List<FiscalMonthStatus>();
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Identifier of the KPI
        /// </summary>
        public int KpiId { get; set; }

        /// <summary>
        /// Fiscal month status list
        /// </summary>
        public List<FiscalMonthStatus> FiscalMonthStatusList { get; private set; }
        #endregion
    }
}
