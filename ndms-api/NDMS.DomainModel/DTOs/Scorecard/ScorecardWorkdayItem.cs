using System;

namespace NDMS.DomainModel
{
    public class ScorecardWorkdayItem
    {
        /// <summary>
        /// The marked date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The flag to check workday
        /// </summary>
        public bool IsWorkday { get; set; }
    }
}
