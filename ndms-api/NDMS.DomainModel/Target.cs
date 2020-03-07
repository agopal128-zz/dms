using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a entity for setting/managing targets
    /// </summary>
    public class Target : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Id of the corresponding Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Id of the KPI(Foreign key attribute)
        /// </summary>
        public int KPIId { get; set; }

        /// <summary>
        /// Id of the Metric(Foreign key attribute)
        /// </summary>
        public int MetricId { get; set; }

        /// <summary>
        /// Type of Metric
        /// </summary>
        public MetricType MetricType { get; set; }

        /// <summary>
        /// Flag which says whether child is cascading target from parent
        /// </summary>
        public bool CascadeFromParent { get; set; }

        /// <summary>
        /// Identifier of Parent Target(Foreign Key Attribute)
        /// </summary>
        public int? ParentTargetId { get; set; }

        /// <summary>
        /// Flag which says whether the target is cascaded to children
        /// </summary>
        public bool IsCascaded { get; set; }

        /// <summary>
        ///  Flag which says whether the stretch goal is enabled or not
        /// </summary>
        public bool IsStretchGoalEnabled { get; set; }

        /// <summary>
        ///  Year Identifier to which target is set
        /// </summary>
        public int CalendarYearId { get; set; }

        /// <summary>
        ///  Effective start date
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        ///  Effective end date
        /// </summary>
        public DateTime EffectiveEndDate { get; set; }

        /// <summary>
        /// Target entry method of this target.
        /// </summary>
        public int? TargetEntryMethodId { get; set; }

        ///<summary>
        /// MTD Performance Tracking Method
        /// </summary>
        public int? MTDPerformanceTrackingMethodId { get; set; }

        /// <summary>
        /// Cascaded Metrics Tracking Method
        /// </summary>
        public int? CascadedMetricsTrackingMethodId { get; set; }

        /// <summary>
        ///  Rollup method of this target
        /// </summary>
        public int? RollUpMethodId { get; set; }

        /// <summary>
        ///  Annual Target
        /// </summary>
        public decimal? AnnualTarget { get; set; }

        /// <summary>
        /// Flag which says whether the target is copied from last year
        /// </summary>
        public bool IsCopiedMetric { get; set; }

        ///<summary>
        /// Flag which identifies if a metric is deleted or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// User ID of the created user
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// modified date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// User ID of the modified user
        /// </summary>
        public int LastModifiedBy { get; set; }

        /// <summary>
        /// Score card Navigational Property
        /// </summary>
        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// KPI Navigational Property
        /// </summary>
        public virtual KPI KPI { get; set; }

        /// <summary>
        /// Metric Navigational Property
        /// </summary>
        public virtual Metric Metric { get; set; }

        /// <summary>
        /// Year Navigational Property
        /// </summary>
        public virtual Year CalendarYear { get; set; }

        /// <summary>
        /// Target Entry Method Navigational Property.
        /// </summary>
        public virtual TargetEntryMethod TargetEntryMethod { get; set; }

        /// <summary>
        /// Rollup Method Navigational Property
        /// </summary>
        public virtual RollupMethod RollupMethod { get; set; }

        /// <summary>
        /// Tracking method Navigational Property
        /// </summary>
        public virtual TrackingMethod TrackingMethod { get; set; }

        /// <summary>
        /// Graph Plotting Method Navigational Property
        /// </summary>
        public virtual GraphPlottingMethod GraphPlottingMethod { get; set; }

        /// <summary>
        ///  Tracking method Id of this target
        /// </summary>
        public int TrackingMethodId { get; set; }

        /// <summary>
        ///  Graph Plotting Method Id of this target
        /// </summary>
        public int? GraphPlottingMethodId { get; set; }

        /// <summary>
        /// Parent Target Navigational Property
        /// </summary>
        public virtual Target ParentTarget { get; set; }

        /// <summary>
        /// Collection of Monthly Targets
        /// </summary>
        public virtual ICollection<MonthlyTarget> MonthlyTargets { get; set; }

        /// <summary>
        /// History of annual targets
        /// </summary>
        public virtual ICollection<TargetHistory> TargetHistory { get; set; }

        /// <summary>
        /// Child Scorecard Targets
        /// </summary>
        public virtual ICollection<Target> ChildScorecardTargets { get; set; }
        #endregion
    }
}
