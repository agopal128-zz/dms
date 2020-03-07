using System.Collections.Generic;
using System.Linq;

namespace NDMS.DomainModel.DTOs
{
    public class HierarchyMenuDTO
    {
        /// <summary>
        /// KPiOwned Scorecards
        /// </summary>
        public IEnumerable<ScorecardMenuItem> KpiOwnerScorecards { get; set; }
        /// <summary>
        /// Team member socrecards
        /// </summary>
        public IEnumerable<ScorecardMenuItem> TeamMemberScorecards { get; set; }
        /// <summary>
        /// Root Scorecards
        /// </summary>
        public IEnumerable<ScorecardMenuItem> RootScorecards { get; set; }

        public bool ScorecardExists
        {
            get
            {
                return (this.KpiOwnerScorecards?.Any()?? false) ||(this.TeamMemberScorecards?.Any()?? false);
            }
        }
    }
}
