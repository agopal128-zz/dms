using NDMS.DomainModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class HolidayPatternItem
    {
        #region Properties
        /// <summary>
        /// Identifier of the Pattern
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the Holiday Pattern
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.HolidayPatternNameEmpty)]
        [MaxLength(85, ErrorMessage = ValidationMessages.HolidayPatternNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether holiday Pattern is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Current Date and Time
        /// </summary>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Collection of Holidays associated with the pattern
        /// </summary>
        public List<DateTime> Holidays { get; set; }

        #endregion
    }
}
