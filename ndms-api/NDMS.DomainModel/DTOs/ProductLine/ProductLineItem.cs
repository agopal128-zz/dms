using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO of ProductLine
    /// </summary>
    public class ProductLineItem
    {
        #region Propertie(s)
        /// <summary>
        /// ID of the ProductLine
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the ProductLine
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ProductLineNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.ProductLineNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag for active ProductLine
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
    }
}
