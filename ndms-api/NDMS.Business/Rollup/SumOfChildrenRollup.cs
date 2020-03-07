using NDMS.Business.Common;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Rollup
{
    /// <summary>
    /// Implements "Sum Of Children" roll up method
    /// </summary>
    internal class SumOfChildrenRollup : IRollupStrategy
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
        public SumOfChildrenRollup(IBaseRepository<DailyActual> dailyActualRepository,
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
        /// Performs roll up of actuals towards the parent for a set of child sccorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        public decimal? CalculateRollup(RollupInfo rollupInfo, List<int> childTargetIds)
        {
            decimal? sumOfChilds = null;
            foreach (var targetId in childTargetIds)
            {
                if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                {
                    var childDailyActual = dailyActualRepository.GetAll().Where(x =>
                        x.TargetId == targetId &&
                        x.Date == rollupInfo.ActualEntry.Date).FirstOrDefault()?.ActualValue;
                    if (childDailyActual != null)
                    {
                        if (sumOfChilds == null)
                        {
                            sumOfChilds = childDailyActual;
                        }
                        else
                        {
                            sumOfChilds += childDailyActual;
                        }
                    }
                }
                else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                {
                    var childMonthlyActual = monthlyActualRepository.GetAll().Where(x =>
                       x.TargetId == targetId &&
                       x.Month == rollupInfo.ActualEntry.Date.Month).
                       FirstOrDefault()?.ActualValue;
                    if (childMonthlyActual != null)
                    {
                        if (sumOfChilds == null)
                        {
                            sumOfChilds = childMonthlyActual;
                        }
                        else
                        {
                            sumOfChilds += childMonthlyActual;
                        }
                    }
                }
            }
            return sumOfChilds;
        }

        /// <summary>
        /// Performs roll up of target towards the parent for a set of child sccorecards
        /// </summary>
        /// <param name="rollupInfo">Rollup information</param>
        /// <param name="childTargetIds">Target Id's of scorecards which needs to be rolled up</param>
        /// <returns>Rolled up value towards the parent</returns>
        public decimal? CalculateRollupTarget(RollupInfo rollupInfo, List<int> childTargetIds)
        {
            decimal? sumOfChilds = null;
            foreach (var targetId in childTargetIds)
            {
                var childTarget = targetRepository.Get(targetId);
                var childMonthlyTarget = childTarget?.MonthlyTargets?.FirstOrDefault(x => x.Month == rollupInfo.TargetEntryDate.Month);
                if (childMonthlyTarget != null)
                {
                    if (rollupInfo.TrackingMethodId == Constants.TrackingMethodDaily)
                    {
                        var childDailyTarget = childTarget.IsCascaded ?
                                               childMonthlyTarget.DailyTargets.FirstOrDefault(x => x.Day == rollupInfo.TargetEntryDate.Day)?.RolledUpGoalValue :
                                               childMonthlyTarget.DailyTargets.FirstOrDefault(x => x.Day == rollupInfo.TargetEntryDate.Day)?.MaxGoalValue;
                        if (childDailyTarget != null)
                        {
                            if (sumOfChilds == null)
                            {
                                sumOfChilds = childDailyTarget;
                            }
                            else
                            {
                                sumOfChilds += childDailyTarget;
                            }
                        }
                    }
                    else if (rollupInfo.TrackingMethodId == Constants.TrackingMethodMonthly)
                    {
                        if (sumOfChilds == null)
                        {
                            sumOfChilds = childTarget.IsCascaded ?
                                          childMonthlyTarget.RolledUpGoalValue :
                                          childMonthlyTarget.MaxGoalValue;
                        }
                        else
                        {
                            sumOfChilds += childTarget.IsCascaded ?
                                          childMonthlyTarget.RolledUpGoalValue :
                                          childMonthlyTarget.MaxGoalValue;
                        }
                    }
                }
            }
            return sumOfChilds;
        }
        #endregion
    }
}
