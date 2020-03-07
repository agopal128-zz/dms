using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO of Process
    /// </summary>
    public class ProcessItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the Process
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Process
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ProcessNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.ProcessNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active Process
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
