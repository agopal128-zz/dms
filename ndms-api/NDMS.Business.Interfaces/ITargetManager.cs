using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Interface which defines all methods for set/manage targets feature
    /// </summary>
    public interface ITargetManager : IDisposable
    {
        /// <summary>
        /// Method to retrieve targets initial data (Years and roll up Methods)
        /// and bool to check whether bowling chart is applicable or not
        /// </summary>
        /// <param name="scorecardId">identifier of scorecard</param>
        /// <returns>
        /// object with year and roll up method list
        /// </returns>
        TargetTemplateData GetTargetsInitialData(int scorecardId);        

        /// <summary>
        /// Retrieves all targets for a scorecard and KPI belongs to an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>List of target entries</returns>
        IEnumerable<TargetItem> GetTargetsForScorecardAndKPI(int scorecardId, int kpiId,
            int yearId);

        /// <summary>
        /// Retrieves all daily targets for a scorecard, KPI and metric belongs to an year
        /// </summary>
        /// <param name="monthlyTargetId">Monthly Target Id</param>
        /// <returns>List of target entries</returns>
        IEnumerable<DailyTargetItem> GetDailyTargets(int monthlyTargetId);

        /// <summary>
        /// Gets the list of available workdays of the month with monthly goal distributed evenly if applicable
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <param name="yearId"></param>
        /// <param name="monthId"></param>
        /// <param name="goalValue"></param>
        /// <returns>List of system generated daily Target </returns>
        IEnumerable<DailyTargetItem> GenerateDailyTargetsList(GenerateDailyTargetsRequest request);

        /// <summary>
        /// Method to retrieve all metrics associated with selected KPI 
        /// having organization data same as that of selected scorecard
        /// </summary>
        /// <param name="kpiId">selected kpi id</param>
        /// <param name="scorecardId">selected scorecard id</param>
        /// <returns>metric list</returns>
        List<MetricItem> GetMetrics(int kpiId, int scorecardId);

        /// <summary>
        /// Method to check whether parent scorecard has the same metric set for the KPI selected 
        /// by child scorecard
        /// </summary>
        /// <param name="request">
        /// object containing parent scorecard id and the selected metric id,
        /// KPI Id and metric type of child scorecard
        /// </param>
        /// <returns>
        /// Cascading parent target details if parent has the same metric set else returns null
        /// which indicates the parent cannot cascade the targets to children
        /// </returns>
        CascadedParentTargetItem GetCascadedMetricDetails(MetricCascadeRequest request);

        /// <summary>
        /// Returns month's list for a calendar year in ascending order
        /// </summary>
        /// <param name="yearId">Calendar year ID</param>
        /// <returns>List of months in ascending order</returns>
        IEnumerable<MonthItem> GetMonthsListForCalendarYear(int yearId);

        /// <summary>
        /// Adds a new target for a primary/secondary metric which belongs to a scorecard and
        /// KPI
        /// </summary>
        /// <param name="targetItem">New target entry</param>
        /// <param name="userName">logged in user name</param>
        void AddMetricTarget(TargetItem targetItem, string userName);

        /// <summary>
        /// Update target for a primary/secondary metric which belongs to a scorecard and
        /// KPI
        /// </summary>
        /// <param name="targetItem"> target entry to be updated</param>
        /// <param name="userName">logged in user name</param>
        void EditMetricTarget(TargetItem targetItem, string userName);

        /// <summary>
        /// method to check whether the targets for cascaded metric set on a score card is cascaded 
        /// completely to child score cards
        /// </summary>
        /// <param name="scorecardId">scorecard id</param>
        /// <param name="kpiId">kpi id</param>
        /// <param name="metricId">metric id</param>
        /// <param name="calendarYearId">calendar year id</param>
        /// <returns>flag which says whether the target of scorecard is fully allocated or not
        /// </returns>
        bool IsMetricTargetsCascadedCompletely(int scorecardId, int kpiId, int metricId,
                                               int calendarYearId);

        /// <summary>
        /// Retrieves the maximum allowed monthly goal for a new/existing target which is 
        /// going to be/being cascaded
        /// </summary>
        /// <param name="parentTargetId">Parent target Id</param>
        /// <param name="childTargetId">Child target Id(if child target is already created)</param>
        /// <param name="selectedRollUpMethodId">Selected rollup method id</param>
        /// <returns>Maximum allowed monthly goals</returns>
        IEnumerable<MonthAndTarget> GetMaximumAllowedMonthlyGoals(int parentTargetId, 
            int?childTargetId, int? selectedRollUpMethodId, int? selectedTargetEntryMethodId);

        /// <summary>
        /// Deletes a specific Metric target if the metric does not have any actuals associated.
        /// </summary>
        /// <param name="userName">logged in user name</param>
        /// <param name="targetId">target id</param>
        /// <returns>Status of the delete operation</returns>
        bool DeleteMetricTarget(int targetId,string userName);

        /// <summary>
        /// Retrieves all selected year targets for a scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Year id</param>
        /// <returns>List of target entries</returns>
        IEnumerable<CopiedKPIItem> GetSelectedYearTargetsForScorecard(int scorecardId, int yearId);        

        /// <summary>
        /// Copies the targets in a scorecard.
        /// </summary>
        /// <param name="scorecardTarget">The scorecard targets for copy.</param>
        /// <param name="userName">Name of the user.</param>
        void CopyTargets(CopiedScorecardItem scorecardTarget, string userName);

        /// <summary>
        /// Gets the rolled up goals.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        IEnumerable<MonthAndTarget> GetRolledupGoals(int targetId, int targetEntryMethodId, int mtdPerformanceTrackingId);
    }
}
