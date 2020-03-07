namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent status of month
    /// </summary>
    public class FiscalMonthStatus
    {
        /// <summary>
        /// Month
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// status of the month
        /// </summary>
        public ActualStatus Status { get; set; }
    }
}
