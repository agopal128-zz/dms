using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for Configuration Settings Item
    /// </summary>
    public class ConfigurationSettingsItem
    {
        #region Propertie(s)
        /// <summary>
        /// Maximum KPI Owner Count
        /// </summary>
        public int MaxKPIOwnerCount { get; set; }

        /// <summary>
        /// Maximum Team Count
        /// </summary>
        public int MaxTeamCount { get; set; }

        /// <summary>
        /// Scorecard Primary Metric Count
        /// </summary>
        public int KPIPrimaryMetricCount { get; set; }

        /// <summary>
        /// Scorecard Secondary Metric Count
        /// </summary>
        public int KPISecondaryMetricCount { get; set; }

        /// <summary>
        /// Duration of session time out in minute
        /// </summary>
        public int SessionTimeout { get; set; }
        
        /// <summary>
        /// Duration of scorecard page auto refresh in minute
        /// </summary>
        public int AutoRefreshDuration { get; set; }
        #endregion

    }
}
