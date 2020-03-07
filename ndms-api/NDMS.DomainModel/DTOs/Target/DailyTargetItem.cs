namespace NDMS.DomainModel.DTOs
{
    public class DailyTargetItem
    {
        /// <summary>
        /// Identifier of daily target
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Day of the month
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// days targeted goal value
        /// </summary>
        public decimal? GoalValue { get; set; }
        /// <summary>
        /// Rolled up goal value for the day
        /// </summary>
        public decimal? RolledUpGoalValue { get; set; }

        /// <summary>
        /// Flag to mark holiday
        /// </summary>
        public bool IsHoliday { get; set; }

        ///<summary>
        /// Flag to mark dates outside target effective date range
        /// </summary>
        public bool IsOutofRange { get; set; }

        ///<summary>
        ///Flag to mark id the day is manually updated
        /// </summary>
        public bool IsManual { get; set; }


    }
}
