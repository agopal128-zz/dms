namespace NDMS.DomainModel.DTOs
{
    public class MonthToDatePerformanceItem
    {
        /// <summary>
        /// Year for which MTD performance is evaluated 
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Month of the Year
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Actual Value for the Month
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// Goal Value for the month
        /// </summary>
        public decimal? GoalValue { get; set; }

        ///<summary>
        ///Status for the month
        /// </summary>
        public ActualStatus Status { get; set; }
    }
}
