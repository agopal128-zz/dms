using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class ScorecardDrilldownNode
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
        /// Drilldown level
        /// </summary>
        public int? DrillDownLevel { get; set; }

        //Flag which says is the scorecard node to be viewed as root node
        public bool IsRootNode { get; set; }

        //Flag which says which scorecard node is active
        public bool IsActive { get; set; }

        //Flag which says which scorecard node to be expanded based on drillDown level
        public bool ExpandTillDrilldownLevel { get; set; }

        /// <summary>
        /// Sort Order in which Scorecard node is listed
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Child Scorecards
        /// </summary>
        public IEnumerable<ScorecardDrilldownNode> Children { get; set; }

        //Flag which represents whether the scorecard have the permission to view scorecard
        public bool CanViewScorecard { get; set; }

        //Drill down status
        public ScorecardStatus ScorecardStatus { get; set; }

        /// <summary>
        /// Flag to show cascaded primary metric
        /// </summary>
        public bool HasPrimaryCascaded { get; set; }

        /// <summary>
        /// Flag to show cascaded secondary metrics
        /// </summary>
        public bool HasSecondaryCascaded { get; set; }

        /// <summary>
        /// Flag to represent cascaded primary metric status
        /// </summary>
        public ActualStatus CascadedPrimaryMetricStatus { get; set; }

        /// <summary>
        /// Flag to represent cascaded secondary metrics status
        /// </summary>
        public ActualStatus CascadedSecondaryMetricStatus { get; set; }
    }
}
