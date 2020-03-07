using System;

namespace NDMS.DomainModel
{
    public class Recordable : BaseEntity
    {
        #region Properties
        
        /// <value>
        /// The scorecard id.
        /// </value>
        public int ScorecardId { get; set; }
        
        /// <value>
        /// The recordable date.
        /// </value>
        public DateTime RecordableDate { get; set; }

        /// <summary>
        /// Flag which says whether recordable is manually entered or not
        /// </summary>
        public bool IsManual { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// User ID of the created user
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// modified date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// User ID of the modified user
        /// </summary>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Flag which says whether recordable is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The scorecard navigation property
        /// </summary>
        public virtual Scorecard Scorecard { get; set; }

        #endregion
    }
}
