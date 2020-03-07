using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    public class HolidayPattern : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Name of the holiday
        /// </summary>
        public string Name { get; set; }        

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
        /// CreatedBy User Navigation Property
        /// </summary>
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Last ModifiedBy User Navigation Property
        /// </summary>
        public virtual User LastModifiedByUser { get; set; }

        /// <summary>
        /// The holidays associated with Holiday Pattern.
        /// </summary>
        public virtual ICollection<HolidayPatternInfo> Holidays { get; set; }
        #endregion
    }
}
