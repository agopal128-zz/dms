using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Represents a DTO which holds the necessary data for a primary metric
    /// </summary>
    public class PrimaryMetricData
    {
        /// <summary>
        /// Identifier of Metric
        /// </summary>
        public int MetricId { get; set; }

        /// <summary>
        /// Name of Metric
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Metric data type Id
        /// </summary>
        public int MetricDataTypeId { get; set; }

        /// <summary>
        /// Tracking Method Id of the Metric
        /// </summary>
        public int TrackingMethodId { get; set; }

        /// <summary>
        /// Target Entry Method Id of the Metric
        /// </summary>
        public int TargetEntryMethodId { get; set; }

        /// <summary>
        /// Target Identifier
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Flag which says whether the metric has been cascaded further down or not
        /// </summary>
        public bool IsCascaded { get; set; }

        /// <summary>
        /// Goal Value Per Month
        /// </summary>
        public decimal? MonthlyGoalValue { get; set; }

        /// <summary>
        /// Stretch Goal Value Per Month
        /// </summary>
        public decimal? MonthlyStretchGoalValue { get; set; }

        /// <summary>
        /// Daily Actuals in case Tracking Method is Daily
        /// </summary>
        public IEnumerable<DailyActualItem> DailyActuals { get; set; }

        /// <summary>
        /// Monthly Actuals in case Tracking Method is Monthly
        /// </summary>
        public IEnumerable<MonthlyActualItem> MonthlyActuals { get; set; }

        /// <summary>
        /// Daily Rate
        /// </summary>
        public decimal? DailyRate { get; set; }

        /// <summary>
        /// Graph Plotting Method Id
        /// </summary>
        public int GraphPlottingMethodId { get; set; }
    }
}
