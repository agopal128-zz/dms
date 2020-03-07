using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for Metric Goal Type
    /// </summary>
    public class GoalTypeItem
    {
        #region Propertie(s)
        /// <summary>
        /// Identifier of Goal Type
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MetricGoalTypeEmpty)]
        public int? Id { get; set; }

        /// <summary>
        /// Name of Goal Type
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
