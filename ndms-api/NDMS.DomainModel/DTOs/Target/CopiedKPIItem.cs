using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class CopiedKPIItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Selected Year
        /// </summary>
        public int CalendarYearId { get; set; }

        /// <summary>
        /// Gets or sets the name of the kpi.
        /// </summary>
        public string KpiName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// List of copied targets
        /// </summary>
        public List<CopiedTargetItem> Targets { get; set; }
        
        #endregion
    }
}
