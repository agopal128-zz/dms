using System;

namespace NDMS.DomainModel
{
    public class ScorecardWorkdayPattern : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardId { get; set; }

        ///<summary>
        /// Flag which identifies if sunday is a workday or not
        /// </summary>
        public bool IsSunday { get; set; }

        ///<summary>
        /// Flag which identifies if monday is a workday or not
        /// </summary>
        public bool IsMonday { get; set; }

        ///<summary>
        /// Flag which identifies if tuesday is a workday or not
        /// </summary>
        public bool IsTuesday { get; set; }

        ///<summary>
        /// Flag which identifies if wednesday is a workday or not
        /// </summary>
        public bool IsWednesday { get; set; }

        ///<summary>
        /// Flag which identifies if thursday is a workday or not
        /// </summary>
        public bool IsThursday { get; set; }

        ///<summary>
        /// Flag which identifies if friday is a workday or not
        /// </summary>
        public bool IsFriday { get; set; }

        ///<summary>
        /// Flag which identifies if saturday is a workday or not
        /// </summary>
        public bool IsSaturday { get; set; }

        /// <summary>
        /// Effective start of the workday pattern
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        /// Effective end of the workday pattern
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

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
        /// Scorecard Navigational Property
        /// </summary>
        public virtual Scorecard Scorecard { get; set; }

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
