using NDMS.DomainModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent a monthly Target for a metric
    /// </summary>
    public class MonthlyTargetItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of monthly target
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Target month
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MonthlyTargetEmpty)]
        public MonthItem Month { get; set; }

        /// <summary>
        /// Targeted monthly value
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// Targeted stretch value
        /// </summary>
        public decimal? StretchGoalValue { get; set; }

        /// <summary>
        /// Daily Rate to be set for each day
        /// </summary>
        public decimal? DailyRateValue { get; set; }

        /// <summary>
        /// Rolled up goal for each day
        /// </summary>
        public decimal? RolledupGoalValue { get; set; }

        ///<summary>
        /// Flag to check if any Rolled up daily target is available
        /// </summary>
        public bool HasRolledUpDailyTarget { get; set; }

        /// <summary>
        /// Is the monthly goal value or Daily Rate updated manually
        /// </summary>
        public bool IsManualTarget { get; set; }

        ///<summary>
        /// Flag to note if there are manual Daily Target entry
        /// </summary>
        public bool HasManualTarget { get; set; }

        ///<summary>
        /// Flag to check if any DailyTargets are present
        /// </summary>
        public bool HasDailyTarget { get; set; }

        ///<summary>
        ///Flag to know if the month is past
        /// </summary>
        public bool IsPastMonth { get; set; } 
        /// <summary>
        /// Collection of daily targets
        /// </summary>
        public IEnumerable<DailyTargetItem> DailyTargets { get; set; }
        #endregion
    }
}
