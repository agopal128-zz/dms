
namespace NDMS.DomainModel.DTOs
{
    public class ScorecardTargetStatusData
    {
        /// <summary>
        /// identifier of scorecard
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Status for availability of targets
        /// </summary>
        public bool IsTargetAvailable { get; set; }

    }
}
