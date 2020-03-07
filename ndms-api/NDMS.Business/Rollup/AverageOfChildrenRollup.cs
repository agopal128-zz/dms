using NDMS.Business.Common;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Rollup
{
    /// <summary>
    /// Implements "Average Of Children" roll up method
    /// </summary>
    internal class AverageOfChildrenRollup : IRollupStrategy
    {
        #region Field(s)
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Target repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dailyActualRepository">Daily actual repository</param>
        /// <param name="monthlyActualRepository">Monthly actual repository</param>
        /// <param name="targetRepository">Target repository</param>
        public AverageOfChildrenRollup(IBaseRepository<DailyActual> dailyActualRepository,
                                       IBaseRepository<MonthlyActual> monthlyActualRepository,
                                       IBaseRepository<Target> targetRepository)
        {
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.targetRepository = targetRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Performs roll up of actuals towards the parent for a set of child scorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be 
        /// rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        public decimal? CalculateRollup(RollupInfo rollupInfo, List<int> childTargetIds)
        {
            decimal? sumOfActuals = null;
            int childTargetCount = 0;

            if (rollupInfo.ParentTargetId != null)
            {
                // Calculate the roll up value recursively for the parent by traversing down
                CalculateRollupValueRecursively(rollupInfo, rollupInfo.ParentTargetId.Value,
                    ref sumOfActuals, ref childTargetCount);

                // Find the average of children
                if (sumOfActuals != null && childTargetCount > 0)
                {
                    // Add to the cumulative figure
                    return sumOfActuals / childTargetCount;
                }
            }

            return null;
        }

        /// <summary>
        /// Performs roll up of targets towards the parent for a set of child scorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be 
        /// rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        public decimal? CalculateRollupTarget(RollupInfo rollupInfo, List<int> childTargetIds)
        {
            decimal? sumOfTargets = null;
            int childTargetCount = 0;

            if (rollupInfo.ParentTargetId != null)
            {
                // Calculate the roll up value recursively for the parent by traversing down
                CalculateRollupTargetValueRecursively(rollupInfo, rollupInfo.ParentTargetId.Value,
                    ref sumOfTargets, ref childTargetCount);

                // Find the average of children
                if (sumOfTargets != null && childTargetCount > 0)
                {
                    // Add to the cumulative figure
                    return sumOfTargets / childTargetCount;
                }
            }

            return null;
        }

        /// <summary>
        /// Traverse the hierarchy down, calculates sum of actuals and count of children which
        /// can be considered for calculating average
        /// </summary>
        /// <param name="rollupInfo">Rollup info</param>
        /// <param name="parentTargetId">Parent target</param>
        /// <param name="sumOfActuals">Sum of actuals which is needed for calculating average</param>
        /// <param name="childTargetCount">Number of child targets which needs to be
        /// considered while calculating actual</param>
        private void CalculateRollupValueRecursively(RollupInfo rollupInfo, int parentTargetId,
            ref decimal? sumOfActuals, ref int childTargetCount)
        {
            // Retrieve all child targets
            var childTargets = targetRepository.GetAll().Where(x =>
                x.ParentTargetId == parentTargetId
                && x.IsActive).Select(x =>
                 new { Id = x.Id, IsCascaded = x.IsCascaded }).ToList();

            // Iterate all child targets and sum up the actual values and count
            foreach (var target in childTargets)
            {
                // While calculating average in a level, we should avoid 
                // scorecards(basically targets) which are cascaded further 
                // down(Or consider only scorecards where actuals can be entered directly)
                if (target.IsCascaded == false)
                {
                    if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                    {
                        var childDailyActual = dailyActualRepository.GetAll().Where(x =>
                         x.TargetId == target.Id &&
                         x.Date == rollupInfo.ActualEntry.Date).FirstOrDefault()?.ActualValue;
                        if (childDailyActual != null)
                        {
                            ++childTargetCount;
                            if (sumOfActuals == null)
                            {
                                sumOfActuals = childDailyActual;
                            }
                            else
                            {
                                sumOfActuals += childDailyActual;
                            }
                        }
                    }
                    else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                    {
                        var childMonthlyActual = monthlyActualRepository.GetAll().Where(x =>
                         x.TargetId == target.Id &&
                         x.Month == rollupInfo.ActualEntry.Date.Month).
                            FirstOrDefault()?.ActualValue;
                        if (childMonthlyActual != null)
                        {
                            ++childTargetCount;
                            if (sumOfActuals == null)
                            {
                                sumOfActuals = childMonthlyActual;
                            }
                            else
                            {
                                sumOfActuals += childMonthlyActual;
                            }
                        }
                    }
                }
            }

            // Traverse down recursively
            foreach (var childTarget in childTargets)
            {
                CalculateRollupValueRecursively(rollupInfo, childTarget.Id, ref sumOfActuals, 
                    ref childTargetCount);
            }
        }

        /// <summary>
        /// Traverse the hierarchy down, calculates sum of targets and count of children which
        /// can be considered for calculating average
        /// </summary>
        /// <param name="rollupInfo">Rollup info</param>
        /// <param name="parentTargetId">Parent target</param>
        /// <param name="sumOfTargets">Sum of actuals which is needed for calculating average</param>
        /// <param name="childTargetCount">Number of child targets which needs to be
        /// considered while calculating actual</param>
        private void CalculateRollupTargetValueRecursively(RollupInfo rollupInfo, int parentTargetId,
            ref decimal? sumOfTargets, ref int childTargetCount)
        {
            // Retrieve all child targets
            var childTargets = targetRepository.GetAll().Where(x =>
                x.ParentTargetId == parentTargetId
                && x.IsActive).Select(x =>
                 new { Id = x.Id, IsCascaded = x.IsCascaded, MonthlyTargets = x.MonthlyTargets}).ToList();
            foreach (var childTarget in childTargets)
            {
                if (!childTarget.IsCascaded)
                {
                    var childMonthlyTarget = childTarget?.MonthlyTargets?.FirstOrDefault(x => x.Month == rollupInfo.TargetEntryDate.Month);
                    if (childMonthlyTarget != null)
                    {
                        if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                        {
                            var childDailyTarget = childMonthlyTarget.DailyTargets.FirstOrDefault(x => x.Day == rollupInfo.TargetEntryDate.Day)?.MaxGoalValue;
                            if (childDailyTarget != null)
                            {
                                ++childTargetCount;
                                if (sumOfTargets == null)
                                {
                                    sumOfTargets = childDailyTarget;
                                }
                                else
                                {
                                    sumOfTargets += childDailyTarget;
                                }
                            }
                        }
                        else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                        {
                            ++childTargetCount;
                            if (sumOfTargets == null)
                            {
                                sumOfTargets = childMonthlyTarget.MaxGoalValue;
                            }
                            else
                            {
                                sumOfTargets += childMonthlyTarget.MaxGoalValue;
                            }
                        }
                    }
                }
            }

            // Traverse down recursively
            foreach (var childTarget in childTargets)
            {
                CalculateRollupTargetValueRecursively(rollupInfo, childTarget.Id, ref sumOfTargets,
                    ref childTargetCount);
            }
        }

        #endregion
    }
}
