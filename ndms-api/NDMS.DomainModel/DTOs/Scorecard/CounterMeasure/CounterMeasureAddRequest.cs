using NDMS.DomainModel.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class CounterMeasureAddRequest : CounterMeasureRequest
    {
        /// <summary>
        /// Id of the KPI(Foreign key attribute)
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureKPIEmpty)]
        public int? KPIId { get; set; }

        /// <summary>
        /// Id of the Metric(Foreign key attribute)
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureMetricEmpty)]
        public int? MetricId { get; set; }

        /// <summary>
        /// Issue
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureIssueEmpty)]
        [MaxLength(300, ErrorMessage = ValidationMessages.CounterMeasureIssueMaxLength)]
        public string Issue { get; set; }
    }
}
