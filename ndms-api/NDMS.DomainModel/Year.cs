using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a Year in the system
    /// </summary>
    public class Year : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Year Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start Date of a Year
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of a Year
        /// </summary>
        public DateTime EndDate { get; set; }
        #endregion
    }
}
