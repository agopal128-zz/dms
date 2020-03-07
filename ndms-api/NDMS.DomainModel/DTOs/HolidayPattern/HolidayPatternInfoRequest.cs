using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NDMS.DomainModel.DTOs
{
    public class HolidayPatternInfoRequest
    {
        #region Properties

        /// <summary>
        /// Identifier of the Pattern
        /// </summary>
        [Required]
        public int HolidayPatternId { get; set; }

        ///<summary>
        ///List of Holidays Associated
        /// </summary>
        public List<DateTime> Holidays { get; set; }
        #endregion
    }
}
