using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a entity for Daily Target
    /// </summary>
    public class DailyTarget : BaseEntity
    {
        #region Propertie(s)
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
        /// History of Daily Targets
        /// </summary>
        public virtual ICollection<DailyTargetHistory> DailyTargetHistory { get; set; }
        #endregion
    }
}
