using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents the status of actual with respect to the corresponding goal value
    /// </summary>
    public enum ActualStatus
    {
        /// <summary>
        /// Actual not entered
        /// </summary>
        NotEntered,

        /// <summary>
        /// Actual met the goal value
        /// </summary>
        Achieved,

        /// <summary>
        /// Actual did not meet the goal value
        /// </summary>
        NotAchieved,

        /// <summary>
        /// Marked a particular day as holiday
        /// </summary>
        Holiday,

        /// <summary>
        /// Date not within target effective dates
        /// </summary>
        NotApplicable,

        /// <summary>
        /// Target missing for the day
        /// </summary>
        NoTarget
    }
}
