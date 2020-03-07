using System;

namespace NDMS.DomainModel
{
    public class DailyTargetHistory : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Daily Target identifier
        /// </summary>
        public int DailyTargetId { get; set; }

        /// <summary>
        /// Monthly Target identifier
        /// </summary>
        public int MonthlyTargetId { get; set; }

        /// <summary>
        /// Target day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Maximum of daily target value
        /// </summary>
        public decimal? MaxGoalValue { get; set; }

        /// <summary>
        /// Rolled up daily target value
        /// </summary>
        public decimal? RolledUpGoalValue { get; set; }

        /// <summary>
        /// Flag to identify manual entry
        /// </summary>
        public bool IsManual { get; set; }

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
        /// Monthly Target navigational property
        /// </summary>
        public virtual MonthlyTarget MonthlyTarget { get; set; }

        /// <summary>
        /// Daily Target navigational property
        /// </summary>
        public virtual DailyTarget DailyTarget { get; set; }
        #endregion
    }
}
