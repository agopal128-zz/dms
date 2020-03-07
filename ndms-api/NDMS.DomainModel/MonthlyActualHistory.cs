using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents History of Monthly Actuals entity
    /// </summary>
    public class MonthlyActualHistory : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Monthly Actual identifier
        /// </summary>
        public int MonthlyActualId { get; set; }

        /// <summary>
        /// Target identifier
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Month to which actual is entered
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// actual value entered
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// status when comparing actual and goal value
        /// </summary>
        public ActualStatus Status { get; set; }

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
        /// Monthly Actual navigational property
        /// </summary>
        public virtual MonthlyActual MonthlyActual { get; set; }
        #endregion
    }
}
