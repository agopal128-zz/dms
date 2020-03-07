using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Helpers
{
    /// <summary>
    /// Defines a utility class which defines various utility operations needed in Actual module
    /// </summary>
    public static class ActualUtility
    {
        #region Public Method(s)
        /// <summary>
        /// Round the actual item to two decimal places
        /// </summary>
        /// <param name="actualItem">Input actual item to round</param>
        public static void RoundActualItem(ActualItem actualItem)
        {
            // Round the actual value to two decimal places
            if (actualItem.ActualValue != null)
            {
                actualItem.ActualValue = decimal.Round(actualItem.ActualValue.Value,
                    2, MidpointRounding.AwayFromZero);
            }

            // Round the goal value to two decimal places
            if (actualItem.GoalValue != null)
            {
                actualItem.GoalValue = decimal.Round(actualItem.GoalValue.Value,
                    2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Round the actual item to two decimal places
        /// </summary>
        /// <param name="actualItem">Input actual item to round</param>
        public static void RoundActualStatusRequest(ActualStatusRequest actualItem)
        {
            // Round the actual value to two decimal places
            actualItem.ActualValue = decimal.Round(actualItem.ActualValue, 2, 
                MidpointRounding.AwayFromZero);

            if (actualItem.GoalValue.HasValue)
            {
                // Round the goal value to two decimal places
                actualItem.GoalValue = decimal.Round(actualItem.GoalValue.Value, 2,
                    MidpointRounding.AwayFromZero);
            }
        }
        #endregion
    }
}
