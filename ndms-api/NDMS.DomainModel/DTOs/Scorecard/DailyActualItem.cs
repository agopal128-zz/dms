namespace NDMS.DomainModel.DTOs
{
    public class DailyActualItem
    {
        /// <summary>
        /// Identifier of the daily actual entry
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Day of the month
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Actual Value for the day
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Status of the actual entry
        /// </summary>
        public ActualStatus Status { get; set; }
    }
}
