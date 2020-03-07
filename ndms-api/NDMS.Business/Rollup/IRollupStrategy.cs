using System.Collections.Generic;

namespace NDMS.Business.Rollup
{
    /// <summary>
    /// Defines a roll up strategy for actual module
    /// </summary>
    public interface IRollupStrategy
    {
        /// <summary>
        /// Performs roll up of actuals towards the parent for a set of child scorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be 
        /// rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        decimal? CalculateRollup(RollupInfo rollupInfo, List<int> childTargetIds);

        /// <summary>
        /// Performs roll up of targets towards the parent for a set of child scorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be 
        /// rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        decimal? CalculateRollupTarget(RollupInfo rollupInfo, List<int> childTargetIds);
    }
}
