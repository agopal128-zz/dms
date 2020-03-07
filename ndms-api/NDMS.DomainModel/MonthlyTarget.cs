using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a entity for Monthly Target
    /// </summary>
    public class MonthlyTarget : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Target identifier
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Target month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Minimum of monthly target value
        /// </summary>
        public decimal? DailyRate { get; set; }

        /// <summary>
        /// Maximum of monthly target value
        /// </summary>
        public decimal? MaxGoalValue { get; set; }

        /// <summary>
        /// Rolled up target value
        /// </summary>
        public decimal? RolledUpGoalValue { get; set; }

        /// <summary>
        /// Targeted stretch value
        /// </summary>
        public decimal? StretchGoalValue { get; set; }

        /// <summary>
        /// Flag to decide which goal to be used for the month
        /// </summary>
        public bool IsRolledUpGoal { get; set; }

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
        /// Target navigational property
        /// </summary>
        public virtual Target Target { get; set; }

        /// <summary>
        /// Collection of daily targets
        /// </summary>
        public virtual ICollection<DailyTarget> DailyTargets { get; set; }

        // <summary>
        /// History of monthly targets
        /// </summary>
        public virtual ICollection<MonthlyTargetHistory> MonthlyTargetHistory { get; set; }
        #endregion
    }
}
