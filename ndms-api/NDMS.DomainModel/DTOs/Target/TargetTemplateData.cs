using System;
using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to store targets initial data
    /// </summary>
    public class TargetTemplateData
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the score card
        /// </summary>
        public string ScorecardName { get; set; }

        /// <summary>
        /// List of KPI Owners
        /// </summary>
        public IEnumerable<string> KPIOwners { get; set; }

        /// <summary>
        /// List of Metric types
        /// </summary>
        public IEnumerable<MetricTypeItem> MetricTypes { get; set; }

        /// <summary>
        /// List of Years
        /// </summary>
        public IEnumerable<YearItem> Years { get; set; }

        /// <summary>
        /// List of Roll up Methods
        /// </summary>
        public IEnumerable<RollupMethodItem> RollupMethods { get; set; }

        /// <summary>
        /// List of Tracking Methods
        /// </summary>
        public IEnumerable<TrackingMethodItem> TrackingMethods { get; set; }

        /// <summary>
        /// List of Target Entry Methods
        /// </summary>
        public IEnumerable<TargetEntryMethodItem> TargetEntryMethods { get; set; }

        /// <summary>
        /// List of Graph Plotting Methods
        /// </summary>
        public IEnumerable<GraphPlottingMethodItem> GraphPlottingMethods { get; set; }

        ///<summary>
        /// List of MTD Performance Tracking Methods 
        ///</summary>
        public IEnumerable<MtdPerformanceTrackingMethodItem> MtdTrackingMethods { get; set; }

        ///<summary>
        /// List of cascaded metrics tracking methods 
        ///</summary>
        public IEnumerable<CascadedMetricsTrackingMethodItem> CascadedMetricsTrackingMethods { get; set; }

        /// <summary>
        /// Flag which says whether the bowling chart of scorecard is applicable
        /// </summary>
        public bool IsBowlingChartApplicable { get; set; }

        /// <summary>
        /// Represents current date
        /// </summary>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Represents current month start date
        /// </summary>
        public DateTime CurrentMonthStartDate { get; set; }

        /// <summary>
        /// Default Value for Roll-up Method
        /// </summary>
        public int DefaultRollupMethodId { get; set; }
        #endregion
    }
}
