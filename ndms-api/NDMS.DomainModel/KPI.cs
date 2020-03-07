using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a Key Performance Indicator in NDMS system
    /// </summary>
    public class KPI : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the KPI
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether KPI is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// List of Scorecards which are having this KPI
        /// </summary>
        public virtual ICollection<Scorecard> Scorecards { get; set; }
        #endregion
    }
}
