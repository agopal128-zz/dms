namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent daily actual and goal
    /// </summary>
    public class DailyGraphData
    {
        /// <summary>
        /// day 
        /// </summary>
        public int Day { get; set; }

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
