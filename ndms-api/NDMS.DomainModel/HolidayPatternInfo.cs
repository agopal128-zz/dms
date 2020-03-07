using System;

namespace NDMS.DomainModel
{
    public class HolidayPatternInfo : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Holiday Pattern(Foreign key attribute)
        /// </summary>
        public int HolidayPatternId { get; set; }

        /// <summary>
        /// Date of the holiday
        /// </summary>
        public DateTime Date { get; set; }

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

        ///<summary>
        /// Flag which identifies if a holiday pattern is deleted or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Holiday Pattern Navigational Property
        /// </summary>
        public virtual HolidayPattern HolidayPattern { get; set; }

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
