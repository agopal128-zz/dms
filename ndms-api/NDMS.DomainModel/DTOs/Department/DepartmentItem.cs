using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for Department
    /// </summary>
    public class DepartmentItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the Department
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Department
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.DepartmentNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.DepartmentNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active Department
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
