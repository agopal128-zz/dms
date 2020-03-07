using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    ///  Represents a goal type entity
    /// </summary>
    public class GoalType : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the Goal Type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether goal type is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Metrics which are part of Goal Type
        /// </summary>
        public virtual ICollection<Metric> Metrics { get; set; }
        #endregion
    }
}
