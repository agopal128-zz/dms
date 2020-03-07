using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a data type entity
    /// </summary>
    public class DataType : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the Data Type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether data type is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Metrics which are part of data type
        /// </summary>
        public virtual ICollection<Metric> Metrics { get; set; }
        #endregion
    }
}
