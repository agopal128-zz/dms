using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO representing a node in Scorecard Hierarchy
    /// </summary>
    public class ScorecardNode
    {
        /// <summary>
        /// Identifier of the scorecard
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the scorecard
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of days without recordables
        /// </summary>
        public double? DaysWithoutRecordables { get; set; }

        /// <summary>
        /// Sort Order in which Scorecard node is shown as parent's child
        /// </summary>
        public int SortOrder { get; set;}

        /// <summary>
        /// Drilldown level
        /// </summary>
        public int? DrillDownLevel { get; set; }

        /// <summary>
        /// Child Scorecards
        /// </summary>
        public IEnumerable<ScorecardNode> Children { get; set; }
       
        // flag which represents whether the scorecard is Active/Inactive
        public bool IsActive { get; set; }

        //Flag which represents whether the scorecard have the permission to manage target
        public bool CanManageTarget { get; set; }

        //Flag which represents whether the scorecard have the permission to add scorecard
        public bool CanAddScorecard { get; set; }

        //Flag which represents whether the scorecard have the permission to edit scorecard
        public bool CanEditScorecard { get; set; }

        //Flag which represents whether the scorecard have the permission to view scorecard
        public bool CanViewScorecard { get; set; }

        //Flag which represents whether the scorecard have the permission to view targets
        public bool CanViewTargets { get; set; }

        //Flag which says if the scorecard order can be changed by the User
        public bool CanReOrder { get; set; }

        //Flag which says is the scorecard node to be viewed as root node
        public bool IsRootNode { get; set; }

        //Flag which says which scorecard node to be expanded based on drillDown level
        public bool ExpandTillDrilldownLevel { get; set; }

        //Drill down status
        public ScorecardStatus ScorecardStatus { get; set; }

    }
}
