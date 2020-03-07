using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines methods needed to retrieve and update scorecard data
    /// </summary>
    public interface IScorecardManager : IDisposable
    {
        /// <summary>
        /// Retrieves the data belongs to a score with all KPI's and metrics associated for an year
        /// depending on the tracking method associated
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="yearId">Identifier of the year for which we need score card data</param>
        /// <param name="month">Month for which we need score card data</param>
        /// <returns>
        /// Scorecard data for the current month/year
        /// </returns>
        ScorecardData GetScorecardData(int scorecardId, int yearId, int month);

        /// <summary>
        /// Retrieves the metric data belongs to a KPI of a particular scorecard for
        /// an year and month
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="kpiId">KPI Identifier</param>
        /// <param name="yearId">Calendar year Identifier</param>
        /// <param name="month">Month</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// Requested KPI data for a scorecard
        /// </returns>
        ScorecardKPIData GetScorecardKPIData(int scorecardId, int kpiId, int yearId,
            int month);

        /// <summary>
        /// Retrieves the secondary metric data belongs to a perticular KPI of a particular scorecard for 
        /// an year and month
        /// </summary>
        /// <param name="kpiId">KPI Identifier</param>
        /// <param name="scorecardId">Target Identifier</param>
        /// <param name="yearId">Calendar year Identifier</param>
        /// <param name="month">Month</param>
        /// <returns>Requested secondary metric data for a selected KPI</returns>
        SecondaryMetricData GetScorecardKPISecondaryMetricData(int kpiId, int targetId, int yearId, int month);


        /// <summary>
        ///  Method to retrieve daily goal of a day in a month
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>Daily goal value</returns>
        decimal? GetDailyGoal(int targetId, int month, int day);

        /// <summary>
        /// Retrieve fiscal month statuses of the primary metric of a KPI for an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>Fiscal month status list</returns>
        IEnumerable<FiscalMonthStatus> GetFiscalMonthStatusForKPI(int scorecardId, int kpiId,
            int yearId);

        /// <summary>
        /// Retrieve fiscal month status of a scorecard considering only primary metrics for 
        /// all KPI's
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <returns>Fiscal month status list for all KPI's</returns>
        IEnumerable<KpiFiscalMonthStatus> GetFiscalMonthStatusForScorecard(int scorecardId,
            int yearId);

        /// <summary>
        /// Method to get monthly target 
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="month">month</param>
        /// <returns>monthly target value</returns>
        decimal? GetMonthlyGoal(int targetId, int month);

        /// <summary>
        /// Method to return scorecard hierarchy with status corresponding to selected scorecard kpi
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="month">month</param>
        /// <param name="yearId">year id</param>
        /// <returns>scorecard hierarchy</returns>
        ScorecardDrilldownNode GetDrillDownHierarchy(int scorecardId, int kpiId,
            int month, int yearId);

        /// <summary>
        /// Method to return scorecard hierarchy with status corresponding to selected scorecard kpi
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">Kpi Id</param>
        /// <param name="date">The date.</param>
        /// <returns>
        /// scorecard hierarchy
        /// </returns>
        ScorecardDrilldownNode GetDrillDownHierarchy(int scorecardId, int kpiId, DateTime date);

        /// <summary>
        /// Gets the basic scorecard data like scorecard name and kpi owners.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <returns>The scorecard data</returns>
        ScorecardData GetBasicScorecardData(int scorecardId, int yearId, int month);

        /// <summary>
        /// Method to retrieve scorecard kpis
        /// </summary>
        /// <param name="scorecardId">identifier of scorecard</param>
        /// <param name="yearId">identifier of calendar year</param>
        /// <returns>List of kpis</returns>
        List<KPIItem> GetScorecardKPIs(int scorecardId, int yearId);

        // <summary>
        /// <summary>
        /// Gets the scorecard KPIs of given month.
        /// </summary>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <param name="month">The month.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <returns>List of KPIs</returns>
        List<KPIItem> GetScorecardKPIs(int scorecardId, int month, int yearId);

    }
}
