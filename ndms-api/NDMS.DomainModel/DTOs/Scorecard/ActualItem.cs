using NDMS.DomainModel.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class ActualItem
    {
        /// <summary>
        /// Identifier of actual entry
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Id of the corresponding Scorecard(Foreign key attribute)
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ActualScorecardEmpty)]
        public int? ScorecardId { get; set; }

        /// <summary>
        /// Target Identifier
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ActualTargetEmpty)]
        public int TargetId { get; set; }

        /// <summary>
        /// Date to which actual is entered
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Actual Value Entered
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ActualValueEmpty)]
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// Monthly/Daily Goal Value
        /// </summary>
        public decimal? GoalValue { get; set; }
    }
}
