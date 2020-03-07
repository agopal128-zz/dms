namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent monthly actual and goal
    /// </summary>
    public class MonthlyGraphData
    {
        /// <summary>
        /// Month 
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Goal Value
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// Stretch Goal Value
        /// </summary>
        public decimal? StretchGoalValue { get; set; }

        /// <summary>
        /// Actual Value
        /// </summary>
        public decimal? ActualValue { get; set; }
    }
}
