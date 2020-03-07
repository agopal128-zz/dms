using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class CounterMeasureEditRequest : CounterMeasureRequest
    {
        /// <summary>
        /// Identifier of Counter Measure
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.CounterMeasureIdEmpty)]
        public int? CounterMeasureId { get; set; }
    }
}
