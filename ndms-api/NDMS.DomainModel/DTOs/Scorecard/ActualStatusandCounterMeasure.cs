namespace NDMS.DomainModel.DTOs
{
    public class ActualandCounterMeasureStatus
    {
        /// <summary>
        /// Status of the actual given
        /// </summary>
        public ActualStatus ActualStatus { get; set; }

        /// <summary>
        ///Flag representing whether a outstanding counter measure is present
        /// </summary>
        public bool OutstandingCounterMeasureExists { get; set; }
    }
}
