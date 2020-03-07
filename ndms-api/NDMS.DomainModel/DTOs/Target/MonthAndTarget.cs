namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Represents a month and it's target
    /// </summary>
    public class MonthAndTarget
    {
        /// <summary>
        /// Month
        /// </summary>
        public MonthItem Month { get; set; }

        /// <summary>
        /// Target value for the month
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Target entry method for the month
        /// </summary>
        public int? TargetEntryMethodId { get; set; }
    }
}
