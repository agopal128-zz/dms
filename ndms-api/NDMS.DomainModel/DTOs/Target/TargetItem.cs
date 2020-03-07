using NDMS.DomainModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent a metric target item
    /// </summary>
    public class TargetItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Target
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Selected scorecard identifier
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetScorecardEmpty)]
        public int ScorecardId { get; set; }

        /// <summary>
        /// Selected KPI
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetKPIEmpty)]
        public int? KPIId { get; set; }

        /// <summary>
        /// Name of Selected KPI 
        /// </summary>
        public string KPIName { get; set; }

        /// <summary>
        /// Identifier of Selected Metric 
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetMetricEmpty)]
        public int? MetricId { get; set; }

        /// <summary>
        /// Name of Selected Metric 
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Selected Metric Type
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetMetriTypeEmpty)]
        public MetricType MetricType { get; set; }

        /// <summary>
        /// Selected Metric Data Type Id
        /// </summary>
        public int MetricDataTypeId { get; set; }

        /// <summary>
        /// Flag which says whether user selected cascade from parent 
        /// </summary>
        public bool CascadeFromParent { get; set; }

        /// <summary>
        /// Flag which says whether the target is cascaded to children
        /// </summary>
        public bool IsCascaded { get; set; }

        /// <summary>
        /// Id of the parent target in case, if the metric is a cascaded one
        /// </summary>
        public int? ParentTargetId { get; set; }

        /// <summary>
        ///  Flag which says whether the stretch goal is enabled or not
        /// </summary>
        public bool? IsStretchGoalEnabled { get; set; }

        /// <summary>
        /// Identifier of Selected Year
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetYearEmpty)]
        public int? CalendarYearId { get; set; }

        /// <summary>
        ///  Effective start date
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetStartDateEmpty)]
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        ///  Effective end date
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetEndDateEmpty)]
        public DateTime EffectiveEndDate { get; set; }

        /// <summary>
        /// Identifier of Selected roll up method
        /// </summary>
        public int? RollupMethodId { get; set; }

        /// <summary>
        /// Identifier of selected tracking method Id
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ActualTrackingMethodEmpty)]
        public int? TrackingMethodId { get; set; }

        /// <summary>
        /// Identifier of selected target entry method if
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetEntryMethodEmpty)]
        public int? TargetEntryMethodId { get; set; }

        ///<summary>
        /// Identifier of selected MTD performance tracking method
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MTDPerformanceTrackingMethodEmpty)]
        public int? MTDPerformanceTrackingMethodId { get; set; }

        ///<summary>
        /// Identifier of selected cascaded metrics tracking method
        /// </summary>
        public int? CascadedMetricsTrackingMethodId { get; set; }

        /// <summary>
        /// Identifier of selected graph plotting method Id
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.TargetGraphPlottingMethodEmpty)]
        public int? GraphPlottingMethodId { get; set; }
        
        ///<summary>
        ///Flag to check if metric is allowed to be deleted
        /// </summary>
        public bool CanDelete { get; set; }

        ///<summary>
        ///Flag to check if metric has monthly and daily target entries
        /// </summary>
        public bool HasMonthlyAndDailyTargets { get; set; }

        /// <summary>
        ///  Annual Target
        /// </summary>
        [RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$",ErrorMessage = ValidationMessages.AnnualTargetInvalid)]
        public decimal? AnnualTarget { get; set; }

        /// <summary>
        /// Collection of Monthly Targets
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MonthlyTargetEmpty)]
        public List<MonthlyTargetItem> MonthlyTargets { get; set; }

        /// <summary>
        /// Maximum allowed monthly goals
        /// </summary>
        public List<MonthAndTarget> MaximumAllowedMonthlyGoals { get; set; }
        #endregion
    }
}
