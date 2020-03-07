namespace NDMS.DomainModel.DTOs
{
    public enum DrilldownStatus
    {
        /// <summary>
        /// In case the status is not applicable(Like a future data, actual not entered)
        /// </summary>
        NotApplicable,

        /// <summary>
        /// Actual met the goal value for both primary and secondary metric
        /// </summary>
        Achieved,

        /// <summary>
        /// Actual did not meet the goal value for primary metric
        /// </summary>
        PrimaryNotAchieved,

        /// <summary>
        /// Actual did not meet the goal value for secondary metric
        /// </summary>
        SecondaryNotAchieved,
    }
}
