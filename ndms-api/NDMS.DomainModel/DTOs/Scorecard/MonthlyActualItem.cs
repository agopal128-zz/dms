namespace NDMS.DomainModel.DTOs
{
    public class MonthlyActualItem
    {
        /// <summary>
        /// Identifier of the monthly actual entry
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Month of the Year
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Actual Value for the Month
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Status for the Month
        /// </summary>
        public ActualStatus Status { get; set; }
    }
}
