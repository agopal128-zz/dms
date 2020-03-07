using System;
namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to accept request to generate Daily targets
    /// </summary>
    public class GenerateDailyTargetsRequest
    {
        public int ScorecardId { get; set; }
        public int YearId { get; set; }
        public int MonthId { get; set; }
        public int MetricId { get; set; }
        public int TargetEntryMethodId { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public int? ExistingMonthlyTargetId { get; set; }
        public decimal? MonthlyGoalValue { get; set; }
        public decimal? DailyRateValue { get; set; }

    }
}
