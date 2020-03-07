using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for a Metric DataType entity
    /// </summary>
    public class DataTypeItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of DataType
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricDataTypeEmpty)]
        public int? Id { get; set; }

        /// <summary>
        /// Name of the data type
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
