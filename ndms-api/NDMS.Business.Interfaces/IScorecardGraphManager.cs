using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    public interface IScorecardGraphManager : IDisposable
    {
        /// <summary>
        /// Retrieves the primary metric graph data for a KPI of a particular scorecard for 
        /// an year and month
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="kpiId">KPI Identifier</param>
        /// <param name="yearId">Calendar year Identifier</param>
        /// <param name="month">Month</param>
        /// <returns>Requested KPI data for a scorecard</returns>
        KPIGraphData GetScorecardKPIGraphData(int scorecardId, int kpiId, int yearId,
            int month);

        /// <summary>
        /// Retrieve graph data of the primary metric of all KPIs in a scorecard
        /// for a month in an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="yearId">Identifier of the year</param>
        /// <param name="month">Month</param>
        /// <returns>Graph data</returns>
        IEnumerable<KPIGraphData> GetScorecardGraphData(int scorecardId,
            int yearId, int month);

        /// <summary>
        /// Gets the scorecard kpi graph data for selected metric.
        /// </summary>
        /// <param name="metricTargetId">The metric target identifier.</param>
        /// <param name="kpiId">The kpi identifier.</param>
        /// <param name="yearId">The year identifier.</param>
        /// <param name="month">The month.</param>
        /// <returns>
        /// The graph data
        /// </returns>
        KPIGraphData GetScorecardMetricKPIGraphData(int metricTargetId, int kpiId, int yearId, int month);
    }
}
