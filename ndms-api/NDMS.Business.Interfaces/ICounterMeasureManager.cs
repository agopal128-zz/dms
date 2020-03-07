using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    public interface ICounterMeasureManager : IDisposable
    {
        /// <summary>
        /// Retrieves all counter measures related to a KPI of a particular scorecard for 
        /// an year
        /// </summary>
        /// <param name="scorecardId">Scorecard Identifier</param>
        /// <param name="kpiId">KPI Identifier</param>
        /// <param name="month">selected month</param>
        /// <returns>All Counter Measures related to the KPI</returns>
        IEnumerable<CounterMeasureItem> GetCounterMeasures(int scorecardId, int kpiId, bool isShowClosed);

        /// <summary>
        /// Retrieves  counter measure details of selected counter measure
        /// </summary>
        /// <param name="counterMeasureId">Counter Measure Id</param>
        /// <returns>Counter Measure DTO</returns>
        CounterMeasureItem GetCounterMeasure(int counterMeasureId);

        /// <summary>
        /// Retrieves all counter measure status
        /// </summary>
        /// <returns>counter measure status list</returns>
        IEnumerable<CounterMeasureStatusItem> GetAllCounterMeasureStatus();

        /// <summary>
        /// Add a new counter measure for a metric which belongs to a score card and KPI
        /// </summary>
        /// <param name="counterMeasureRequest">new counter measure entry</param>
        /// <param name="userName">logged in user name</param>
        void AddCounterMeasure(CounterMeasureAddRequest counterMeasureRequest, string userName);

        /// <summary>
        /// Method to update existing counter measure
        /// </summary>
        /// <param name="counterMeasureRequest">counter measure to be updated</param>
        /// <param name="userName">logged in user name</param>
        void EditCounterMeasure(CounterMeasureEditRequest counterMeasureRequest, string userName);

        /// <summary>
        /// Method to get counter measure count for a KPI which belongs to scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <returns>counter measure count</returns>
        int GetCounterMeasureCount(int scorecardId, int kpiId);

        /// <summary>
        /// Method to check whether target is achieved or not by comparing goal and actuals based on 
        /// goal type and return status and outstanding counter measure status if any 
        /// </summary>
        /// <param name="request">Actual request object</param>
        /// <returns>Actual and counter measure status</returns>
        ActualandCounterMeasureStatus GetActualandCounterMeasureStatus(ActualStatusRequest request);

        /// <summary>
        /// Method to retrieve metrics belonging to a kpi in a scorecard
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="kpiId">KPI Id</param>
        /// <param name="yearId">Year Id</param>
        /// <param name="month">Month</param>
        /// <returns>metric list</returns>
        List<MetricItem> GetScorecardKPIMetrics(int scorecardId, int kpiId, int month,
            int yearId);

        /// <summary>
        /// Gets all counter measure priority.
        /// </summary>
        /// <returns>The counter measure priority list</returns>
        List<CounterMeasurePriorityItem> GetAllCounterMeasurePriority();
    }
}
