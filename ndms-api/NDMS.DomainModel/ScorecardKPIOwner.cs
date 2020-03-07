using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a KPI owner in the system
    /// </summary>
    public class ScorecardKPIOwner : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of the User(Foreign key attribute)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Assigned date
        /// </summary>
        public DateTime AssignedOn { get; set; }

        public DateTime? DeactivatedOn { get; set; }

        /// <summary>
        /// Flag which says whether KPI Owner is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Id of the corresponding Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Score card which the KPI Owner owns
        /// </summary>
        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// KPI Owner Navigation Property
        /// </summary>
        public virtual User User { get; set; }
        #endregion
    }
}
