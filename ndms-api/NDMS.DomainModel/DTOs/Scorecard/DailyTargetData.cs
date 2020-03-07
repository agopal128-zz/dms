using System;

namespace NDMS.DomainModel.DTOs
{
    public class DailyTargetData
    {
        /// <summary>
        /// Goal Value for a day
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// Stretch Goal Value
        /// </summary>
        public decimal? StretchGoalValue { get; set; }
    }
}
