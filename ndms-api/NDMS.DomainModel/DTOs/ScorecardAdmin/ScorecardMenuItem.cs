
namespace NDMS.DomainModel.DTOs
{
    public class ScorecardMenuItem
    {
        /// <summary>
        /// Identifier of the scorecard
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the scorecard
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the facility.
        /// </summary>
        public string NamePrefix { get; set; }
        /// <summary>
        /// The parent scorecard Id of the scorecard
        /// </summary>
        public int? ParentScorecardId { get; set; }
        ///<summary>
        /// Root Scorecard Id
        /// </summary>
        public int TopScorecardId { get; set; }
    }
}
