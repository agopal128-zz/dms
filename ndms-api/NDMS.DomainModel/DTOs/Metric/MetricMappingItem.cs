using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class MetricMappingItem
    {
        /// <summary>
        /// Identifier of Metric Mapping
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Identifier of Selected KPI
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingKPIEmpty)]
        public int? KPIId { get; set; }

        /// <summary>
        /// Name of Selected KPI
        /// </summary>
        public string KPIName { get; set; }

        /// <summary>
        /// Identifier of Selected Metric
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingMetricEmpty)]
        public int? MetricId { get; set; }

        /// <summary>
        /// Name of Selected Metric
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// Identifier of Selected Business segment
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingBusinessSegmentEmpty)]
        public int? BusinessSegmentId { get; set; }

        /// <summary>
        /// Name of Selected Business Segment
        /// </summary>
        public string BusinessSegmentName { get; set; }

        /// <summary>
        /// Identifier of Selected Division
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingDivisionEmpty)]
        public int? DivisionId { get; set; }

        /// <summary>
        /// Name of Selected Division
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Identifier of Selected Facility
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingFacilityEmpty)]
        public int? FacilityId { get; set; }

        /// <summary>
        /// Name of Selected Facility
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Identifier of Selected ProductLine
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingProductLineEmpty)]
        public int? ProductLineId { get; set; }

        /// <summary>
        /// Name of Selected ProductLine
        /// </summary>
        public string ProductLineName { get; set; }

        /// <summary>
        /// Identifier of Selected Department
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingDepartmentEmpty)]
        public int? DepartmentId { get; set; }

        /// <summary>
        /// Name of Selected Department
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Identifier of Selected Process
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricMappingProcessEmpty)]
        public int? ProcessId { get; set; }

        /// <summary>
        /// Name of Selected Process
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Flag which says whether mapping is active or not
        /// </summary>
        public bool IsActive { get; set; }
    }
}
