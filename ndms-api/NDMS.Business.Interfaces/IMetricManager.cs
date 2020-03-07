using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines Metric interface
    /// </summary>
    public interface IMetricManager : IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Method to retrieve all metrics
        /// </summary>
        /// <returns>metric list</returns>
        IEnumerable<MetricItem> GetAll();

        /// <summary>
        /// Retrieve selected metric
        /// </summary>
        /// <param name="metricId">selected metric id</param>
        /// <returns>metric item</returns>
        MetricItem Get(int metricId);

        /// <summary>
        /// Retrieves all active goal types and data types 
        /// </summary>
        /// <returns>object with goal types and data types</returns>
        MetricTemplateData GetMetricTemplateData();

        /// <summary>
        /// Method to retrieve all assigned metrics
        /// </summary>
        /// <returns>metric mapping list</returns>
        IEnumerable<MetricMappingItem> GetAllMetricMappings();

        /// <summary>
        /// Get all metrics whose name starts with the input string mentioned.
        /// </summary>
        /// <param name="name">Input name to match</param>
        /// <returns>List of metrics whose name starts with the input string</returns>
        IEnumerable<MetricSuggestion> GetMetricsWithName(string name);

        /// <summary>
        /// Add or update metric mapping to database
        /// </summary>
        /// <param name="metricMappingRequest">list of metric mapping object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>added metric mapping id</returns>
        void AddorUpdateMetricMapping(IEnumerable<MetricMappingItem> metricMappingRequest,
            string userName);

        /// <summary>
        /// Add or Update metric 
        /// </summary>
        /// <param name="metricList">list of metric object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddorUpdateMetric(IEnumerable<MetricItem> metricList, string userName);
        #endregion
    }
}
