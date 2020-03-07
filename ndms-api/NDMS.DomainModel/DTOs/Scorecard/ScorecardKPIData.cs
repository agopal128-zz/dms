using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class ScorecardKPIData
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScorecardKPIData()
        {
            this.SecondaryMetricsData = new List<SecondaryMetricData>();
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Identifier of KPI
        /// </summary>
        public int KpiId { get; set; }

        /// <summary>
        /// Name of KPI
        /// </summary>
        public string KpiName { get; set; }

        /// <summary>
        /// List of secondary metric data associated with the KPI
        /// </summary>
        public List<SecondaryMetricData> SecondaryMetricsData { get; set; }

        /// <summary>
        /// Primary metric data associated with the KPI
        /// </summary>
        public PrimaryMetricData PrimaryMetricData { get; set; }

        /// <summary>
        /// Count of counter measures associated with the KPI's metrics
        /// </summary>
        public int CounterMeasureCount { get; set; }
        #endregion
    }
}
