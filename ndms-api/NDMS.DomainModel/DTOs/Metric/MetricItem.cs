using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    ///  DTO for metric entity
    /// </summary>
    public class MetricItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Metric
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the metric
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricNameEmpty)]
        [MaxLength(85, ErrorMessage = ValidationMessages.MetricNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether metric is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Goal type of this metric
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricGoalTypeEmpty)]
        public GoalTypeItem GoalType { get; set; }

        /// <summary>
        /// Data type of this metric
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricDataTypeEmpty)]
        public DataTypeItem DataType { get; set; }
        #endregion
    }
}
