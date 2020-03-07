using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Helpers
{
    /// <summary>
    /// Defines a utility class which defines various utility operations needed in Target module
    /// </summary>
    public static class TargetUtility
    {
        #region Public Method(s)
        /// <summary>
        /// Round the target item to two decimal places
        /// </summary>
        /// <param name="targetItem">Input target item to round</param>
        public static void RoundTargetItem(TargetItem targetItem)
        {
            // Round the annual target to two decimal places
            if (targetItem.AnnualTarget != null)
            {
                targetItem.AnnualTarget = decimal.Round(targetItem.AnnualTarget.Value,
                    2, MidpointRounding.AwayFromZero);
            }

            // Round the monthly/stretch/daily goal to two decimal places
            foreach (var monthlyTarget in targetItem.MonthlyTargets)
            {
                if (monthlyTarget.GoalValue != null)
                {
                    monthlyTarget.GoalValue = decimal.Round(monthlyTarget.GoalValue.Value,
                        2, MidpointRounding.AwayFromZero);
                }

                if (monthlyTarget.StretchGoalValue != null)
                {
                    monthlyTarget.StretchGoalValue = decimal.Round(
                        monthlyTarget.StretchGoalValue.Value, 2, MidpointRounding.AwayFromZero);
                }

                // Rounding daily targets 
                if (monthlyTarget.DailyTargets != null)
                {
                    foreach (var dailyTarget in monthlyTarget.DailyTargets)
                    {
                        if (dailyTarget.GoalValue != null)
                        {
                            dailyTarget.GoalValue = decimal.Round(dailyTarget.GoalValue.Value, 2,
                                MidpointRounding.AwayFromZero);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
