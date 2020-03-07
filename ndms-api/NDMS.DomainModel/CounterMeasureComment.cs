using System;

namespace NDMS.DomainModel
{
    public class CounterMeasureComment : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Id of Counter Measure(Foreign key attribute)
        /// </summary>
        public int CounterMeasureId { get; set; }

        /// <summary>
        /// Comment for the Counter Measure
        /// </summary>
        public string Comment { get; set; }

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
        /// Counter Measure Navigational Property
        /// </summary>
        public virtual CounterMeasure CounterMeasure { get; set; }

        /// <summary>
        /// CreatedBy User Navigation Property
        /// </summary>
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Last ModifiedBy User Navigation Property
        /// </summary>
        public virtual User LastModifiedByUser { get; set; }
        #endregion
    }
}
