using NDMS.Business.Common;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Converters
{
    /// <summary>
    /// Contains various converters needed for Targets module
    /// </summary>
    internal static class TargetConverters
    {
        /// <summary>
        /// Convert Target data structure to TargetHistory
        /// </summary>
        /// <param name="target">Input Target DS</param>
        /// <returns>TargetHistory DS</returns>
        public static TargetHistory ConvertTargetToTargetHistory(Target target)
        {
            return new TargetHistory()
            {
                Target = target,
                ScorecardId = target.ScorecardId,
                KPIId = target.KPIId,
                MetricId = target.MetricId,
                MetricType = MetricType.Primary,
                EffectiveStartDate = target.EffectiveStartDate,
                EffectiveEndDate = target.EffectiveEndDate,
                IsCascadeFromParent = target.CascadeFromParent,
                ParentTargetId = target.ParentTargetId,
                IsStretchGoalEnabled = target.IsStretchGoalEnabled,
                RollUpMethodId = target.RollUpMethodId,
                TrackingMethodId = target.TrackingMethodId,
                TargetEntryMethodId = target.TargetEntryMethodId,
                GraphPlottingMethodId = target.GraphPlottingMethodId,
                MTDPerformanceTrackingMethodId = target.MTDPerformanceTrackingMethodId,
                CascadedMetricsTrackingMethodId = target.CascadedMetricsTrackingMethodId,
                AnnualTarget = target.AnnualTarget,
                IsCascaded = target.IsCascaded,
                IsActive = target.IsActive,
                CalendarYearId = target.CalendarYearId,
                IsCopiedMetric = target.IsCopiedMetric,
                CreatedOn = target.CreatedOn,
                LastModifiedOn = target.LastModifiedOn,
                CreatedBy = target.CreatedBy,
                LastModifiedBy = target.LastModifiedBy
            };
        }

        /// <summary>
        /// Convert MonthlyTarget data structure to MonthlyTargetHistory
        /// </summary>
        /// <param name="monthlyTarget">Input MonthlyTarget DS</param>
        /// <returns>MonthlyTarget DS</returns>
        public static MonthlyTargetHistory ConvertMonthlyTargetToMonthlyTargetHistory(
            MonthlyTarget monthlyTarget)
        {
            return new MonthlyTargetHistory()
            {
                MonthlyTarget = monthlyTarget,
                Target = monthlyTarget.Target,
                Month = monthlyTarget.Month,
                DailyRate = monthlyTarget.DailyRate,
                MaxGoalValue = monthlyTarget.MaxGoalValue,
                StretchGoalValue = monthlyTarget.StretchGoalValue,
                RolledUpGoalValue = monthlyTarget.RolledUpGoalValue,
                CreatedOn = monthlyTarget.CreatedOn,
                LastModifiedOn = monthlyTarget.LastModifiedOn,
                CreatedBy = monthlyTarget.CreatedBy,
                LastModifiedBy = monthlyTarget.LastModifiedBy
            };
        }

        /// <summary>
        /// Convert DailyTarget data structure to DailyTargetHistory
        /// </summary>
        /// <param name="dailyTarget">Input DailyTarget DS</param>
        /// <returns>DailyTargetHistory DS</returns>
        public static DailyTargetHistory ConvertDailyTargetToDailyTargetHistory(
            DailyTarget dailyTarget)
        {
            return new DailyTargetHistory()
            {
                DailyTarget = dailyTarget,
                MonthlyTarget = dailyTarget.MonthlyTarget,
                Day = dailyTarget.Day,                
                MaxGoalValue = dailyTarget.MaxGoalValue,
                RolledUpGoalValue = dailyTarget.RolledUpGoalValue,
                IsManual = dailyTarget.IsManual,
                CreatedOn = dailyTarget.CreatedOn,
                LastModifiedOn = dailyTarget.LastModifiedOn,
                CreatedBy = dailyTarget.CreatedBy,
                LastModifiedBy = dailyTarget.LastModifiedBy
            };
        }

        /// <summary>
        /// Method to convert Target item DTO to Target entity
        /// </summary>
        /// <param name="targetItem">Target item DTO to be converted</param>
        /// <param name="Parent target Id">Identifier of the parent target</param>
        /// <param name="loggedInUserId">Logged In UserId</param>
        /// <returns>Target entity</returns>
        public static Target ConvertTargetItemDTOToTarget(TargetItem targetItem,
            int? parentTargetId, int loggedInUserId)
        {

            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var target = new Target()
            {
                ScorecardId = targetItem.ScorecardId,
                KPIId = targetItem.KPIId.Value,
                MetricId = targetItem.MetricId.Value,
                MetricType = targetItem.MetricType,
                EffectiveStartDate = targetItem.EffectiveStartDate,
                EffectiveEndDate = targetItem.EffectiveEndDate,
                CascadeFromParent = targetItem.CascadeFromParent,
                ParentTargetId = parentTargetId,
                IsStretchGoalEnabled = targetItem.IsStretchGoalEnabled ?? false,
                TrackingMethodId = targetItem.TrackingMethodId ?? Constants.TrackingMethodDaily,
                TargetEntryMethodId = targetItem.TargetEntryMethodId,
                GraphPlottingMethodId = targetItem.GraphPlottingMethodId,
                MTDPerformanceTrackingMethodId = targetItem.MTDPerformanceTrackingMethodId,
                CascadedMetricsTrackingMethodId = targetItem.CascadedMetricsTrackingMethodId,
                AnnualTarget = targetItem.AnnualTarget,
                IsCascaded = false,
                IsActive = true,
                IsCopiedMetric = false,
                CalendarYearId = targetItem.CalendarYearId.Value,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,      //Need to update with logged in user in future
                LastModifiedBy = loggedInUserId, //Need to update with logged in user in future
            };

            target.TargetHistory = new List<TargetHistory>() {
                ConvertTargetToTargetHistory(target)
            };

            // Iterate monthly targets and insert
            List<MonthlyTarget> monthlyTargets = new List<MonthlyTarget>();
            foreach (var monthlyTarget in targetItem.MonthlyTargets)
            {
                if (monthlyTarget.GoalValue != null || monthlyTarget.DailyRateValue != null)
                {
                    MonthlyTarget mthlyTarget = ConvertMonthlyTargetItemToMonthlyTargetEntity
                        (monthlyTarget, loggedInUserId);
                    mthlyTarget.MonthlyTargetHistory = new List<MonthlyTargetHistory>(){
                        ConvertMonthlyTargetToMonthlyTargetHistory(mthlyTarget)
                    };

                    mthlyTarget.DailyTargets = new List<DailyTarget>();
                    if (monthlyTarget.DailyTargets != null)
                    {
                        foreach (var dailyTargetItem in monthlyTarget.DailyTargets)
                        {
                            if (!dailyTargetItem.IsHoliday && !dailyTargetItem.IsOutofRange)
                            {
                                DailyTarget dailyTarget =
                                    ConvertDailyTargetItemToDailyTargetEntity(dailyTargetItem,
                                    mthlyTarget, loggedInUserId);
                                mthlyTarget.DailyTargets.Add(dailyTarget);
                            }
                        }
                    }

                    monthlyTargets.Add(mthlyTarget);
                }
            }
            target.MonthlyTargets = monthlyTargets;
            return target;
        }

        /// <summary>
        /// Method to convert copied Target item DTO to Target entity
        /// </summary>
        /// <param name="targetItem">Target item DTO to be converted</param>
        /// <param name="loggedInUserId">Logged In UserId</param>
        /// <param name="parentTarget">parent target info for cascaded metrics, else null</param>
        /// <returns>
        /// Target entity
        /// </returns>
        public static Target ConvertCurrentYearTargetToNextYearTarget(Target targetItem, int loggedInUserId, Target parentTarget)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            var target = new Target()
            {
                ScorecardId = targetItem.ScorecardId,
                KPIId = targetItem.KPIId,
                MetricId = targetItem.MetricId,
                MetricType = targetItem.MetricType,
                //if cascaded from parent
                CascadeFromParent = parentTarget != null ? true : false,
                ParentTargetId = parentTarget?.ParentTargetId,
                IsStretchGoalEnabled = parentTarget?.IsStretchGoalEnabled ?? targetItem.IsStretchGoalEnabled ,
                TrackingMethodId = parentTarget?.TrackingMethodId ??  targetItem.TrackingMethodId,
                TargetEntryMethodId = parentTarget?.TargetEntryMethodId ?? targetItem.TargetEntryMethodId,
                GraphPlottingMethodId = parentTarget?.GraphPlottingMethodId ?? targetItem.GraphPlottingMethodId,
                MTDPerformanceTrackingMethodId = parentTarget?.MTDPerformanceTrackingMethodId ?? targetItem.MTDPerformanceTrackingMethodId,
                CascadedMetricsTrackingMethodId = parentTarget?.CascadedMetricsTrackingMethodId ?? targetItem.CascadedMetricsTrackingMethodId,
                IsCascaded = false,
                IsCopiedMetric = true,
                IsActive = true,
                CreatedOn = curTimestamp,
                LastModifiedOn = curTimestamp,
                CreatedBy = loggedInUserId,      
                LastModifiedBy = loggedInUserId, 
            };            

            target.TargetHistory = new List<TargetHistory>() {
                ConvertTargetToTargetHistory(target)
            };

            return target;
        }

        /// <summary>
        /// Method to convert Target entity to target item DTO
        /// </summary>
        /// <param name="target">target entity to be converted</param>
        /// <param name="monthsList">List of months in the calendar year</param>
        /// <returns>target item DTO</returns>
        public static TargetItem ConvertTargetToTargetItemDTO(Target target,
            List<MonthItem> monthsList)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var targetItem = new TargetItem()
            {
                Id = target.Id,
                KPIId = target.KPIId,
                ScorecardId = target.ScorecardId,
                CalendarYearId = target.CalendarYearId,
                MetricId = target.Metric.Id,
                MetricName = target.Metric.Name,
                MetricType = target.MetricType,
                MetricDataTypeId = target.Metric.DataTypeId,
                EffectiveStartDate = target.EffectiveStartDate,
                EffectiveEndDate = target.EffectiveEndDate,
                CascadeFromParent = target.CascadeFromParent,
                IsCascaded = target.IsCascaded,
                TargetEntryMethodId = target.TargetEntryMethodId,
                IsStretchGoalEnabled = target.IsStretchGoalEnabled,
                GraphPlottingMethodId = target.GraphPlottingMethodId,
                MTDPerformanceTrackingMethodId = target.MTDPerformanceTrackingMethodId,
                CascadedMetricsTrackingMethodId = target.CascadedMetricsTrackingMethodId,
                TrackingMethodId = target.TrackingMethodId,
                AnnualTarget = target.AnnualTarget,
                MonthlyTargets = target.MonthlyTargets
                   .Select(m => ConvertMonthlyTargetToMonthlyTargetItemDTO(m,
                   monthsList.FirstOrDefault(x => x.Id == m.Month))).ToList()
            };

            // Fill the remaining months with empty goal values
            foreach (var monthItem in monthsList)
            {
                if (!targetItem.MonthlyTargets.Any(x => x.Month.Id == monthItem.Id))
                {
                    targetItem.MonthlyTargets.Add(new MonthlyTargetItem()
                    {
                        Month = monthItem,
                        IsPastMonth = (monthItem.Id < (currentDate.Month - 1) && monthItem.Year == currentDate.Year) || monthItem.Year < currentDate.Year
                    });
                }
            }

            // Sort the monthly targets by year and month. 
            targetItem.MonthlyTargets = targetItem.MonthlyTargets.OrderBy(x => x.Month.Year).
                ThenBy(x => x.Month.Id).ToList();
            return targetItem;
        }


        /// <summary>
        /// Method to convert Target entity to target item DTO excluding targets
        /// </summary>
        /// <param name="target">target entity to be converted</param>
        /// <param name="monthsList">List of months in the calendar year</param>
        /// <returns>target item DTO</returns>
        public static CopiedTargetItem ConvertTargetToCopyTargetItemDTO(Target target)
        {
            return new CopiedTargetItem()
            {
                Id = target.Id,
                KPIId = target.KPIId,
                ScorecardId = target.ScorecardId,
                CalendarYearId = target.CalendarYearId,
                MetricId = target.Metric.Id,
                MetricName = target.Metric.Name,
                MetricType = target.MetricType,
                MetricDataTypeId = target.Metric.DataTypeId,
                TargetEntryMethodId = target.TargetEntryMethodId,
                IsStretchGoalEnabled = target.IsStretchGoalEnabled,
                GraphPlottingMethodId = target.GraphPlottingMethodId,
                TrackingMethodId = target.TrackingMethodId,
                MTDPerformanceTrackingMethodId = target.MTDPerformanceTrackingMethodId,
                CascadedMetricsTrackingMethodId = target.CascadedMetricsTrackingMethodId
            };
        }
        /// <summary>
        /// Method to convert daily target entity to daily target item DTO
        /// </summary>
        /// <param name="dailyTarget">daily target entity to be converted</param>
        /// <returns>Daily target item DTO</returns>
        public static DailyTargetItem ConvertDailyTargetToDailyTargetItemDTO(DailyTarget dailyTarget)
        {
            var dailyTargetItem = new DailyTargetItem()
            {
                Id = dailyTarget.Id,
                Day = dailyTarget.Day,
                GoalValue = dailyTarget.MaxGoalValue,
                RolledUpGoalValue = dailyTarget.RolledUpGoalValue,                
                IsManual = dailyTarget.IsManual
            };
            return dailyTargetItem;
        }

        /// <summary>
        /// Method to convert monthly target entity to monthly target item DTO
        /// </summary>
        /// <param name="monthlyTarget">MonthlyTarget Entity</param>
        /// <param name="monthItem">MonthItem DTO</param>
        /// <returns></returns>
        public static MonthlyTargetItem ConvertMonthlyTargetToMonthlyTargetItemDTO 
                (MonthlyTarget monthlyTarget, MonthItem monthItem)
        {
            var currentDate = TimeZoneUtility.GetCurrentTimestamp();

            var monthlyTargetItem = new MonthlyTargetItem()
            {
                Id = monthlyTarget.Id,
                Month = monthItem,
                GoalValue = monthlyTarget.MaxGoalValue,
                DailyRateValue = monthlyTarget.DailyRate,
                StretchGoalValue = monthlyTarget.StretchGoalValue,
                HasManualTarget = monthlyTarget.DailyTargets.Any(x => x.IsManual),
                HasRolledUpDailyTarget = monthlyTarget.DailyTargets.Any(x => x.RolledUpGoalValue != null),
                HasDailyTarget = monthlyTarget.DailyTargets.Any(x=> x.MaxGoalValue != null),
                IsPastMonth = (monthItem.Id < (currentDate.Month - 1) && monthItem.Year == currentDate.Year) || monthItem.Year < currentDate.Year,
                RolledupGoalValue = monthlyTarget.RolledUpGoalValue

            };
            return monthlyTargetItem;
        }

        /// <summary>
        /// Convert MonthlyTargetItem data structure to MonthlyTarget
        /// </summary>
        /// <param name="monthlyTarget">Input MonthlyTargetItem</param>
        /// <param name="loggedInUserId">Logged In UserId</param>
        /// <returns>MonthlyTarget DS</returns>
        public static MonthlyTarget ConvertMonthlyTargetItemToMonthlyTargetEntity(
            MonthlyTargetItem monthlyTarget, int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            MonthlyTarget mthlyTarget = new MonthlyTarget();
            mthlyTarget.Month = monthlyTarget.Month.Id;
            mthlyTarget.MaxGoalValue = monthlyTarget.GoalValue;
            mthlyTarget.DailyRate = monthlyTarget.DailyRateValue;
            mthlyTarget.StretchGoalValue = monthlyTarget.StretchGoalValue;
            mthlyTarget.CreatedOn = curTimestamp;
            mthlyTarget.LastModifiedOn = curTimestamp;
            mthlyTarget.CreatedBy = loggedInUserId;      //Need to update with logged in user in future
            mthlyTarget.LastModifiedBy = loggedInUserId;  //Need to update with logged in user in future
            return mthlyTarget;
        }

        /// <summary>
        /// Convert DailyTargetItem DS to DailyTarget
        /// </summary>
        /// <param name="dailyTarget">Input DailyTargetItem</param>
        /// <param name="monthlyTarget">Corresponding monthly target</param>
        /// <param name="loggedInUserId">Logged In UserId</param>
        /// <returns>DailyTarget DS</returns>
        public static DailyTarget ConvertDailyTargetItemToDailyTargetEntity(
            DailyTargetItem dailyTarget,
            MonthlyTarget monthlyTarget,
            int loggedInUserId)
        {
            DateTime curTimestamp = TimeZoneUtility.GetCurrentTimestamp();
            DailyTarget daily = new DailyTarget();
            daily.MonthlyTarget = monthlyTarget;
            daily.Day = dailyTarget.Day;
            daily.MaxGoalValue = dailyTarget.GoalValue;
            daily.IsManual = dailyTarget.IsManual;
            daily.CreatedOn = curTimestamp;
            daily.LastModifiedOn = curTimestamp;
            daily.CreatedBy = loggedInUserId;     //Need to update with logged in user in future
            daily.LastModifiedBy = loggedInUserId; //Need to update with logged in user in future
            daily.DailyTargetHistory = new List<DailyTargetHistory>()
              {
                 ConvertDailyTargetToDailyTargetHistory(daily)
              };
            return daily;
        }

        /// <summary>
        /// mapping metric database entity to metric item DTO
        /// </summary>
        /// <param name="metric">metric entity</param>
        /// <returns>metric dto object</returns>
        public static MetricItem ConvertMetricToMetricItemDTO(Metric metric)
        {
            return new MetricItem()
            {
                Id = metric.Id,
                Name = metric.Name,
                GoalType = (metric.GoalType != null) ? new GoalTypeItem()
                {
                    Id = metric.GoalType.Id,
                    Name = metric.GoalType.Name
                } : null,
                DataType = (metric.DataType != null) ? new DataTypeItem()
                {
                    Id = metric.DataType.Id,
                    Name = metric.DataType.Name
                } : null
            };
        }        
    }
}
