using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO required to plot graph for a KPI
    /// </summary>
    public class KPIGraphData
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of KPI
        /// </summary>
        public int KpiId { get; set; }

        /// <summary>
        /// Min value of graph y-axis
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// Min value of graph y-axis
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// List of daily values
        /// </summary>
        public IEnumerable<DailyGraphData> DailyGraphData { get; set; }


        /// <summary>
        /// List of monthly values
        /// </summary>
        public List<MonthlyGraphData> MonthlyGraphData { get; set; }
        #endregion
    }
}
