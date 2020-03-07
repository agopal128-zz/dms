using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class CopiedScorecardItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Selected Year
        /// </summary>
        public int CalendarYearId { get; set; }

        /// <summary>
        /// List of copied targets
        /// </summary>
        public List<CopiedTargetItem> Targets { get; set; }
        #endregion
    }
}
