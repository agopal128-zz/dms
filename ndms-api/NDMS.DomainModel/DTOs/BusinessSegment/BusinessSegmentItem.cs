using NDMS.DomainModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for Business Segment
    /// </summary>
    public class BusinessSegmentItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the business segment
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the business segment
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.BusinessSegmentNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.BusinessSegmentNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active business segment
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
