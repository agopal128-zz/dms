using System;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to accept request to check whether a metric set can be cascaded to children
    /// </summary>
    public class MetricCascadeRequest
    {
        public int? TargetId { get; set; }

        public int ScorecardId { get; set; }

        public int KPIId { get; set; }

        public int MetricId { get; set; }

        public int CalendarYearId { get; set; }

        public int? TargetEntryMethodId { get; set; }

        public MetricType MetricType { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime EffectiveEndDate { get; set; }

        public int? RollUpMethodId { get; set; }
    }
}
