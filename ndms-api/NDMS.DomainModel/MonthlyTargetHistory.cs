using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents history of monthly target
    /// </summary>
    public class MonthlyTargetHistory : BaseEntity
    {
        #region Propertie(s)

        /// <summary>
        /// Monthly Target identifier
        /// </summary>
        public int MonthlyTargetId { get; set; }
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
        /// Monthly Target navigational property
        /// </summary>
        public virtual MonthlyTarget MonthlyTarget { get; set; }
        #endregion
    }
}
