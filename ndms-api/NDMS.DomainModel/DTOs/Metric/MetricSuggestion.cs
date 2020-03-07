namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent metric typeahead
    /// </summary>
    public class MetricSuggestion
    {
        /// <summary>
        /// Identifier of Metric
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of Metric
        /// </summary>
        public string Name { get; set; }
    }
}
