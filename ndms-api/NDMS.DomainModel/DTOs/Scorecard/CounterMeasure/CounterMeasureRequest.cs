using NDMS.DomainModel.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class CounterMeasureRequest
    {
        /// <summary>
        /// Id of the corresponding Scorecard(Foreign key attribute)
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureScorecardEmpty)]
        public int? ScorecardId { get; set; }

        /// <summary>
        /// Action for the Counter Measure
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureActionEmpty)]
        [MaxLength(300, ErrorMessage = ValidationMessages.CounterMeasureActionMaxLength)]
        public string Action { get; set; }

        /// <summary>
        /// Due Date of Counter Measure
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureDueDateEmpty)]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Assigned User Account
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureAssigneeEmpty)]
        public string AssignedTo { get; set; }

        /// <summary>
        /// Id of Counter Measure Status(Foreign key attribute)
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureStatusEmpty)]
        public int? CounterMeasureStatusId { get; set; }

        /// <summary>
        /// Id of Counter Measure Priority(Foreign key attribute)
        /// </summary>
        public int? CounterMeasurePriorityId { get; set; }

        /// <summary>
        /// Counter Measure Comment
        /// </summary>
        [MaxLength(4000, ErrorMessage = ValidationMessages.CounterMeasureCommentMaxLength)]
        public string Comment { get; set; }
    }
}
