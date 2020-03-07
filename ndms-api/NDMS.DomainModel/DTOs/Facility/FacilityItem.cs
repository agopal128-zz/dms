using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO of Facility
    /// </summary>
    public class FacilityItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the Facility
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Facility
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.FacilityNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.FacilityMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active Facility
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
