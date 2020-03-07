namespace NDMS.DomainModel.DTOs
{
    public class CopiedTargetItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Target
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Selected scorecard identifier
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Selected KPI
        /// </summary>
        public int? KPIId { get; set; }

        /// <summary>
        /// Name of Selected KPI 
        /// </summary>
        public string KPIName { get; set; }

        /// <summary>
        /// Identifier of Selected Metric 
        /// </summary>
        public int? MetricId { get; set; }

        /// <summary>
        /// Name of Selected Metric 
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Selected Metric Type
        /// </summary>
        public MetricType MetricType { get; set; }

        /// <summary>
        /// Selected Metric Data Type Id
        /// </summary>
        public int MetricDataTypeId { get; set; }
        

        /// <summary>
        ///  Flag which says whether the stretch goal is enabled or not
        /// </summary>
        public bool? IsStretchGoalEnabled { get; set; }

        /// <summary>
        /// Identifier of Selected Year
        /// </summary>
        public int? CalendarYearId { get; set; }        

        
        /// <summary>
        /// Identifier of selected tracking method Id
        /// </summary>
        public int TrackingMethodId { get; set; }

        /// <summary>
        /// Identifier of selected MTD performance Tracking Method
        /// </summary>
        public int? MTDPerformanceTrackingMethodId { get; set; }

        /// <summary>
        /// Identifier of selected cascaded metrics tracking method Id
        /// </summary>
        public int? CascadedMetricsTrackingMethodId { get; set; }

        /// <summary>
        /// Identifier of selected target entry method if
        /// </summary>
        public int? TargetEntryMethodId { get; set; }

        /// <summary>
        /// Identifier of selected graph plotting method Id
        /// </summary>
        public int? GraphPlottingMethodId { get; set; }      

        #endregion
    }
}
