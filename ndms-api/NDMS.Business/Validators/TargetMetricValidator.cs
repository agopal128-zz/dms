using NDMS.Business.Common;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Validators
{
    public class TargetMetricValidator
    {
        #region Field(s)
        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Scorecard repository
        /// </summary>
        private IBaseRepository<Scorecard> scorecardRepository;


        private IBaseRepository<Metric> metricRepository;

        #endregion

        #region Private Methods

        /// <summary>
        /// method to get count of metrics within the target start date
        /// </summary>
        /// <param name="target">target entry</param>
        /// <returns>count of active metrics</returns>
        private int GetMetricCountWithinTargetStartDate(TargetItem target)
        {
            int metricCount = 0;
            //get count of all active metrics within target start date
            metricCount = targetRepository.GetAll().Count(x =>
                                    x.ScorecardId == target.ScorecardId &&
                                    x.KPIId == target.KPIId && x.IsActive &&
                                    x.MetricType == target.MetricType &&
                                    x.CalendarYearId == target.CalendarYearId &&
                                    (target.EffectiveStartDate >= x.EffectiveStartDate &&
                                    target.EffectiveStartDate <= x.EffectiveEndDate) &&
                                    (target.Id.HasValue ? x.Id != target.Id.Value : x.Id == x.Id));

            return metricCount;
        }

        private int GetMetricCountWithinTargetDateRanges(TargetItem target)
        {
            int metricCount = 0;
            metricCount = targetRepository.GetAll().Count(x =>
                                    x.ScorecardId == target.ScorecardId &&
                                    x.KPIId == target.KPIId && x.IsActive &&
                                    x.MetricType == target.MetricType &&
                                    x.CalendarYearId == target.CalendarYearId &&
                                    (target.EffectiveStartDate <= x.EffectiveStartDate &&
                                     target.EffectiveEndDate >= x.EffectiveEndDate) &&
                                    (target.Id.HasValue ? x.Id != target.Id.Value : x.Id == x.Id));

            return metricCount;
        }
        /// <summary>
        /// method to get count of metrics within the target end date
        /// </summary>
        /// <param name="target">target entry</param>
        /// <returns>count of active metrics</returns>
        private int GetMetricCountWithinTargetEndDate(TargetItem target)
        {
            int metricCount = 0;
            //get count of all active metrics within target end date
            metricCount = targetRepository.GetAll().Count(x =>
                                    x.ScorecardId == target.ScorecardId &&
                                    x.KPIId == target.KPIId && x.IsActive &&
                                    x.MetricType == target.MetricType &&
                                    x.CalendarYearId == target.CalendarYearId &&
                                    (target.EffectiveEndDate >= x.EffectiveStartDate &&
                                     target.EffectiveEndDate <= x.EffectiveEndDate) &&
                                     (target.Id.HasValue ? x.Id != target.Id.Value : x.Id == x.Id));

            return metricCount;
        }

        /// <summary>
        /// method to get count of metrics active in target start of the month
        /// </summary>
        /// <param name="target">target entry</param>
        /// <returns>count of active metrics</returns>
        private int GetCountOfMetricsActiveInTargetStartMonth(TargetItem target)
        {
            //count of metrics active in selected target start month 
            return targetRepository.GetAll().Count(x =>
                                 x.ScorecardId == target.ScorecardId &&
                                 x.KPIId == target.KPIId && x.IsActive &&
                                 x.MetricType == target.MetricType &&
                                 (x.EffectiveEndDate.Month == target.EffectiveStartDate.Month ||
                                 x.EffectiveStartDate.Month == target.EffectiveStartDate.Month) &&
                                 x.CalendarYearId == target.CalendarYearId &&
                                 ((target.Id.HasValue) ? (x.Id != target.Id.Value) : (x.Id == x.Id)));
        }

        /// <summary>
        /// method to get count of metrics active in target end of the month
        /// </summary>
        /// <param name="target">target entry</param>
        /// <returns>count of active metrics</returns>
        private int GetCountOfMetricsActiveInTargetEndMonth(TargetItem target)
        {
            //count of metrics active in selected target end month 
            return targetRepository.GetAll().Count(x =>
                                 x.ScorecardId == target.ScorecardId &&
                                 x.KPIId == target.KPIId && x.IsActive &&
                                 x.MetricType == target.MetricType &&
                                 (x.EffectiveEndDate.Month == target.EffectiveEndDate.Month ||
                                 x.EffectiveStartDate.Month == target.EffectiveEndDate.Month) &&
                                 x.CalendarYearId == target.CalendarYearId &&
                                 ((target.Id.HasValue) ? (x.Id != target.Id.Value) : (x.Id == x.Id)));
        }


        /// <summary>
        /// Method to get sum of targets of Child Scorecards for a month
        /// </summary>
        /// <param name="cascadedTargetId">target to be added/updated</param>
        /// <param name="parentTargetId">parent target</param>
        /// <param name="monthlyTarget">monthly target</param>
        /// <returns></returns>
        private decimal GetSumOfChildScorecardTarget(int? cascadedTargetId, int parentTargetId,
            MonthlyTargetItem monthlyTarget)
        {
            decimal sumOfChildScorecardsMonthlyTarget = 0;
            //get all child scorecard targets having cascade from parent enabled
            var siblingTargets = targetRepository.GetAll().Where(x =>
                                        x.ParentTargetId == parentTargetId
                                        && x.CascadeFromParent && x.IsActive &&
                                        (cascadedTargetId.HasValue ? x.Id != cascadedTargetId.Value : x.Id == x.Id))
                                        .ToList();

            //iterating through child scorecard 
            foreach (var child in siblingTargets)
            {
                var monthlyTargetOfChild = child.MonthlyTargets
                    .FirstOrDefault(x => x.Month == monthlyTarget.Month.Id &&
                    x.MaxGoalValue.HasValue);
                if (monthlyTargetOfChild != null)
                {
                    sumOfChildScorecardsMonthlyTarget = sumOfChildScorecardsMonthlyTarget
                        + monthlyTargetOfChild.MaxGoalValue.Value;
                }
            }
            sumOfChildScorecardsMonthlyTarget = sumOfChildScorecardsMonthlyTarget
                    + monthlyTarget.GoalValue.Value;

            return sumOfChildScorecardsMonthlyTarget;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="targetRepository">Target Repository</param>
        /// <param name="scorecardRepository">Scorecard Repository</param>
        public TargetMetricValidator(IBaseRepository<Target> targetRepository,
            IBaseRepository<Scorecard> scorecardRepository,
            IBaseRepository<Metric> metricRepository)
        {
            if (targetRepository == null || scorecardRepository == null || metricRepository == null)
            {
                throw new ArgumentNullException("Repository",
                    "The given parameter cannot be null.");
            }
            this.targetRepository = targetRepository;
            this.scorecardRepository = scorecardRepository;
            this.metricRepository = metricRepository;
        }
        #endregion
        /// <summary>
        /// method to validate whether a primary metric already exists in the given time period
        /// </summary>
        /// <param name="target">target entry</param>
        public virtual void ValidatePrimaryMetricCount(TargetItem target)
        {
            int maxPrimaryMetricCount = Convert.ToInt32(ConfigurationManager.
                AppSettings[AppSettingsKeys.KPIPrimaryMetricCount]);

            //getting count of primary metric added for the given kpi for the selected
            // target start date and end date and restrict the entry if count greater than or 
            //equal to max primary metric count
            int metricCountWithInTargetStartDate = GetMetricCountWithinTargetStartDate(target);
            int metricCountWithInTargetEndDate = GetMetricCountWithinTargetEndDate(target);
            int metricCountWithInTargetDateRange = GetMetricCountWithinTargetDateRanges(target);

            if (metricCountWithInTargetStartDate >= maxPrimaryMetricCount ||
                metricCountWithInTargetDateRange >= maxPrimaryMetricCount ||
                metricCountWithInTargetEndDate >= maxPrimaryMetricCount)
            {
                var errorMessage = string.Format(
                                Constants.PrimaryMetricMaxCountErrorMessage, maxPrimaryMetricCount);
                throw new NDMSBusinessException(errorMessage);
            }

            //get the count of metrics active in target start month and target end month and restrict
            //the entry if any of these count is greater than or equal to the max primary metric count
            int metricCountActiveInTargetStartMonth = GetCountOfMetricsActiveInTargetStartMonth(target);
            int metricCountActiveInTargetEndMonth = GetCountOfMetricsActiveInTargetEndMonth(target);
            if (metricCountActiveInTargetStartMonth >= maxPrimaryMetricCount ||
                metricCountActiveInTargetEndMonth >= maxPrimaryMetricCount)
            {
                throw new NDMSBusinessException(Constants.AddTargetMetricErrorMessage);
            }

        }

        /// <summary>
        /// method to validate whether a primary metric already exists in the given time period
        /// </summary>
        /// <param name="target">target entry</param>
        public virtual void ValidateSecondaryMetricCount(TargetItem target)
        {
            int maxSecondaryMetricCount = Convert.ToInt32(ConfigurationManager.
                AppSettings[AppSettingsKeys.KPISecondaryMetricCount]);

            //getting count of primary metric added for the given kpi for the selected
            // target start date and end date and restrict the entry if count greater than or 
            //equal to max secondary metric count
            int metricCountWithInTargetStartDate = GetMetricCountWithinTargetStartDate(target);
            int metricCountWithInTargetEndDate = GetMetricCountWithinTargetEndDate(target);
            int metricCountWithInTargetDateRange = GetMetricCountWithinTargetDateRanges(target);

            if (metricCountWithInTargetStartDate >= maxSecondaryMetricCount ||
                metricCountWithInTargetEndDate >= maxSecondaryMetricCount)
            {
                var errorMessage = string.Format(Constants.SecondaryMetricMaxCountErrorMessage,
                    maxSecondaryMetricCount);
                throw new NDMSBusinessException(errorMessage);
            }
            //get the count of metrics active in target start month and target end month and restrict
            //the entry if any of these count is greater than or equal to the max secondary metric count
            int metricCountActiveInTargetStartMonth = GetCountOfMetricsActiveInTargetStartMonth(target);
            int metricCountActiveInTargetEndMonth = GetCountOfMetricsActiveInTargetEndMonth(target);
            if (metricCountActiveInTargetStartMonth >= maxSecondaryMetricCount ||
                metricCountActiveInTargetEndMonth >= maxSecondaryMetricCount)
            {
                throw new NDMSBusinessException(Constants.AddTargetMetricErrorMessage);
            }
        }

        /// <summary>
        /// method to check whether this metric already exists for the scorecard & KPI
        /// </summary>
        /// <param name="scorecardId">scorecard id</param>
        /// <param name="kpiId">kpi id</param>
        /// <param name="metricId">metric id</param>
        /// <param name="calendarYearId">calendar year id</param>
        public virtual void ValidateMetricForScorecardKPI(TargetItem target)
        {
            bool isMetricExists = targetRepository.GetAll().Any(x =>
                                    x.ScorecardId == target.ScorecardId &&
                                    x.KPIId == target.KPIId && x.IsActive &&
                                    x.MetricId == target.MetricId &&
                                    x.CalendarYearId == target.CalendarYearId &&
                                    ((target.EffectiveStartDate >= x.EffectiveStartDate &&
                                    target.EffectiveStartDate <= x.EffectiveEndDate) ||
                                    (target.EffectiveEndDate >= x.EffectiveStartDate &&
                                    target.EffectiveEndDate <= x.EffectiveEndDate)) &&
                                    (target.Id.HasValue ? x.Id != target.Id.Value : x.Id == x.Id));
            if (isMetricExists)
            {
                throw new NDMSBusinessException(Constants.MetricExistsErrorMessage);
            }
        }

        /// <summary>
        /// Method to validate whether the cascaded metrics is taking values of the parent scorecard
        /// and also check whether the goal entered is less than or equal to the unallocated portion
        /// </summary>
        /// <param name="targetItem">target entry</param>
        public virtual void ValidateCascadedMetric(TargetItem targetItem)
        {
            int? parentScorecardId = scorecardRepository.Get(targetItem.ScorecardId).
                   ParentScorecardId;
            // If this is not a top level score card
            if (parentScorecardId != null)
            {
                var parentTarget = targetRepository.GetAll().Where(x =>
                    x.ScorecardId == parentScorecardId &&
                    x.KPIId == targetItem.KPIId && x.IsActive &&
                    x.MetricId == targetItem.MetricId &&
                    x.CalendarYearId == targetItem.CalendarYearId &&
                    x.EffectiveStartDate <= targetItem.EffectiveEndDate &&
                    x.EffectiveEndDate >= targetItem.EffectiveStartDate).FirstOrDefault();

                if (parentTarget == null)
                {
                    throw new NDMSBusinessException(Constants.TargetCannotBeCascadedErrorMessage);
                }
                else
                {
                    //Checks whether the cascaded metrics Effective Dates, Tracking Method,  
                    //Graph Plotting method, MTD Performance Tracking Method and StrechGoal is derived from parent scorecard
                    if (targetItem.EffectiveStartDate < parentTarget.EffectiveStartDate
                        || targetItem.EffectiveEndDate > parentTarget.EffectiveEndDate)
                    {
                        throw new NDMSBusinessException(Constants.CascadedTargetEffectiveDatesErrorMessage);
                    }
                    if (parentTarget.IsStretchGoalEnabled != targetItem.IsStretchGoalEnabled)
                    {
                        throw new NDMSBusinessException(Constants.CascadedTargetStretchGoalErrorMessage);
                    }
                    if (parentTarget.TrackingMethodId != targetItem.TrackingMethodId)
                    {
                        throw new NDMSBusinessException(Constants.CascadedTargetTrackingMethodErrorMessage);
                    }
                    if (parentTarget.GraphPlottingMethodId != targetItem.GraphPlottingMethodId)
                    {
                        throw new NDMSBusinessException(Constants.CascadedTargetGraphPlottingMethodErrorMessage);
                    }
                    if (parentTarget.MTDPerformanceTrackingMethodId != targetItem.MTDPerformanceTrackingMethodId)
                    {
                        throw new NDMSBusinessException(Constants.CascadedTargetMTDPerformanceTrackingErrorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Validate whether the selected metric is applicable for cumulative plotting
        /// </summary>
        /// <param name="target">target entry</param>
        public virtual void ValidateMetricForCumulativePlottingMethod(TargetItem target)
        {
            if (target.GraphPlottingMethodId.HasValue &&
                target.GraphPlottingMethodId == Constants.GraphPlottingMethodCumulative)
            {
                Metric metric = metricRepository.Get(target.MetricId.Value);
                if (metric.DataTypeId == Constants.DataTypePercentage)
                {
                    throw new NDMSBusinessException(Constants.CumulativeDataTypeErrorMessage);
                }
            }
        }
    }
}
