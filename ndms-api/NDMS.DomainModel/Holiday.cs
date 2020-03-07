using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Entity to represent holiday in a calendar year
    /// </summary>
    public class Holiday : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Identifier of the Year (Foreign Key Attribute)
        /// </summary>
        public int YearId { get; set; }

        /// <summary>
        /// Date which is marked Holiday
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Holiday Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigational Property for Year
        /// </summary>
        public virtual Year Year { get; set; }
        #endregion
    }
}
