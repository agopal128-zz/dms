using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a roll up method entity
    /// </summary>
    public class RollupMethod : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the Rollup Method
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether the rollup method is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Targets which are part of this rollup method
        /// </summary>
        #endregion
    }
}
