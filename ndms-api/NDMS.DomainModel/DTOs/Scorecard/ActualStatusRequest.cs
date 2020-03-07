using System;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent request to check actual status
    /// </summary>
    public class ActualStatusRequest
    {
        /// <summary>
        /// Target Identifier
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Goal Value to be compared
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// Actual Value to be compared
        /// </summary>
        public decimal ActualValue { get; set; }
        
        /// <summary>
        /// Selected Date
        /// </summary>
        public DateTime SelectedDate { get; set; }
    }
}
