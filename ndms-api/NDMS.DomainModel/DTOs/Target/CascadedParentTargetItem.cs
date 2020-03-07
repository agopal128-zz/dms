using System;
using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to capture details of cascaded parent target
    /// </summary>
    public class CascadedParentTargetItem
    {
        /// <summary>
        /// Target id of the parent
        /// </summary>
        public int ParentTargetId { get; set; }

        /// <summary>
        ///  Effective start date
        /// </summary>
        public DateTime EffectiveStartDate { get; set; }

        /// <summary>
        ///  Effective end date
        /// </summary>
        public DateTime EffectiveEndDate { get; set; }

        /// <summary>
        ///  Flag which says whether the stretch goal is enabled or not
        /// </summary>
        public bool IsStretchGoalEnabled { get; set; }

        /// <summary>
        ///  Identifier of roll up method
        /// </summary>
        public int? RollupMethodId { get; set; }

        /// <summary>
        ///  Identifier of tracking method
        /// </summary>
        public int TrackingMethodId { get; set; }

        /// <summary>
        /// Flag which says whether child is cascading target from parent
        /// </summary>
        public bool CascadeFromParent { get; set; }
        
        /// <summary>
        /// Identifier for target entry method
        /// </summary>
        public int? TargetEntryMethodId { get; set; }

        /// <summary>
        ///  Identifier of graph plotting method
        /// </summary>
        public int? GraphPlottingMethodId { get; set; }

        /// <summary>
        /// Identifier of MTD Performance Tracking Method
        /// </summary>
        public int? MTDPerformanceTrackingMethodId { get; set; }

        /// <summary>
        /// Maximum allowed monthly goals
        /// </summary>
        public List<MonthAndTarget> MaximumAllowedMonthlyGoals { get; set; }
    }
}
