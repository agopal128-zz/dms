using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for Division
    /// </summary>
    public class DivisionItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the Division
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Division
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.DivisionNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.DivisionNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active Division
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
