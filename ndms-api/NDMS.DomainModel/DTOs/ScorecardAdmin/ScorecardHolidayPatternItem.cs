using System;

namespace NDMS.DomainModel.DTOs
{
   public class ScorecardHolidayPatternItem
    {
        #region Propertie(s)       

        /// <summary>
        /// Identifier of the Holiday Pattern
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of Scorecard
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Id of Scorecard
        /// </summary>
        public int HolidayPatternId { get; set; }

        /// <summary>
        /// Name of the Holiday
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        /// 
        /// <summary>
        /// Effective start of the holiday pattern
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        /// Effective end of the holiday pattern
        /// </summary>
        public DateTime? EffectiveEndDate { get; set; }

        #endregion
    }
}
