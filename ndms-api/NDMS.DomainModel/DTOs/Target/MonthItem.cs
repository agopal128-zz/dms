using NDMS.DomainModel.Common;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// Represents a month in a calendar year
    /// </summary>
    public class MonthItem
    {
        /// <summary>
        /// Identifier of the month
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MonthlyTargetEmpty)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the month
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Calendar year Id
        /// </summary>
        public int YearId { get; set; }

        /// <summary>
        /// Calendar year
        /// </summary>
        public int Year { get; set; }
    }
}
