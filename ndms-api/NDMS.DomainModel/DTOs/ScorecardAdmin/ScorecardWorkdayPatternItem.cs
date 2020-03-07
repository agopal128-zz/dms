using System;

namespace NDMS.DomainModel.DTOs
{
    public class ScorecardWorkdayPatternItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Scorecard Workday Pattern
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of Scorecard
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
                
        #endregion
    }
}
