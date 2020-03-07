using NDMS.DomainModel;

namespace NDMS.Business.Common
{
    /// <summary>
    /// Class to compare targets and actuals
    /// </summary>
    internal class TargetActualComparer
    {
        /// <summary>
        /// method to return status by comparing goal and actual value based on goal type
        /// </summary>
        /// <param name="goalValue"></param>
        /// <param name="actualValue"></param>
        /// <param name="goalTypeId"></param>
        /// <returns></returns>
        public static ActualStatus GetActualStatus(decimal? goalValue,
            decimal? actualValue, int goalTypeId)
        {
            ActualStatus status = new ActualStatus();
            if (goalValue.HasValue)
            {
                switch (goalTypeId)
                {
                    case Constants.GoalTypeEqualTo:
                        status = (actualValue == goalValue) ? ActualStatus.Achieved
                                : ActualStatus.NotAchieved;
                        break;
                    case Constants.GoalTypeGreaterThanOrEqualTo:
                        status = (actualValue >= goalValue) ? ActualStatus.Achieved
                           : ActualStatus.NotAchieved;
                        break;
                    case Constants.GoalTypeLessThanOrEqualTo:
                        status = (actualValue <= goalValue) ? ActualStatus.Achieved
                           : ActualStatus.NotAchieved;
                        break;
                }
            }
            else
            {
                status = ActualStatus.NotEntered;
            }
            return status;
        }
    }
}
