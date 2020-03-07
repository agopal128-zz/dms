using System;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to represent Year entity
    /// </summary>
    public class YearItem
    {
        /// <summary>
        /// Year Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the Year
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start Date of a Year
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of a Year
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
