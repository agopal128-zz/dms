using System;

namespace NDMS.DomainModel.DTOs
{
    public class RollupTargetItem
    {
        /// <summary>
        /// The target identifier.
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// The target entry date.
        /// </summary>
        public DateTime TargetEntryDate { get; set; }

        /// <summary>
        /// The target roll up value.
        /// </summary>
        public decimal? RollUpValue { get; set; }

        /// <summary>
        /// The target goal value.
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// Flag to check holiday
        /// </summary>
        public bool IsHoliday { get; set; }
    }
}
