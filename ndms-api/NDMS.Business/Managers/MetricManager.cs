using NDMS.Business.Common;
using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Implements Metric Manager
    /// </summary>
    public class MetricManager : IMetricManager
    {
        #region Field(s)
        /// <summary>
        /// Metric Repository
        /// </summary>
        private IMetricRepository metricRepository;

        /// <summary>
        /// Metric Mapping Repository
        /// </summary>
        private IBaseRepository<MetricMapping> metricMappingRepository;

        /// <summary>
        /// User Repository
        /// </summary>
        private IBaseRepository<User> userRepository;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// mapping metric database entity to metric item DTO
        /// </summary>
        /// <param name="metric">metric entity</param>
        /// <returns>metric dto object</returns>
        private MetricItem ConvertMetricToMetricItemDTO(Metric metric)
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
                } : null,
                IsActive = metric.IsActive
            };
        }

        /// <summary>
        /// Method to add a metric to database
        /// </summary>
        /// <param name="metricItem">metric item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddMetric(MetricItem metricItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            var metric = new Metric()
            {
                Name = metricItem.Name,
                DataTypeId = metricItem.DataType.Id.Value,
                GoalTypeId = metricItem.GoalType.Id.Value,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = metricItem.IsActive
            };
            metricRepository.AddOrUpdate(metric);
            metricRepository.Save();
        }

        /// <summary>
        /// Method to update a metric item
        /// </summary>
        /// <param name="metricItem">Metric item to update</param>
        /// <param name="userName">logged in user name</param>
        private void UpdateMetric(MetricItem metricItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            //get existing metric details
            var existingMetric = metricRepository.Get(metricItem.Id.Value);
            existingMetric.Name = metricItem.Name;
            existingMetric.DataTypeId = metricItem.DataType.Id.Value;
            existingMetric.GoalTypeId = metricItem.GoalType.Id.Value;
            existingMetric.IsActive = metricItem.IsActive;
            existingMetric.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingMetric.LastModifiedBy = loggedInUserId;
            metricRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a metric
        /// </summary>
        /// <param name="metricItem">Metric item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateMetricUpdateRequest(MetricItem metricItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing metric details
            var existingMetric = metricRepository.Get(metricItem.Id.Value);
            if (existingMetric == null)
            {
                string errorMessage = string.Format(Constants.MetricNotFound,
                    metricItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the metric name it self, check if the 
                // new name already exists or not
                if (existingMetric.Name != metricItem.Name)
                {
                    if (metricRepository.GetAll().Any(m => m.Name == metricItem.Name))
                    {
                        string errorMessage = string.Format(Constants.MetricExists,
                            metricItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
                // If targets are set for this metric, only "IsActive" status can be updated
                if (existingMetric.AnnualTargets.Any())
                {
                    if (existingMetric.GoalTypeId != metricItem.GoalType.Id ||
                       existingMetric.DataTypeId != metricItem.DataType.Id)
                    {
                        string errorMessage = string.Format(Constants.MetricUpdateTargetSetErrorMessage,
                            metricItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }

        /// <summary>
        /// Method to add a metric mapping to database
        /// </summary>
        /// <param name="metricMappingItem">metric mapping to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddMetricMapping(MetricMappingItem metricMappingItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;

            var metricMapping = new MetricMapping()
            {
                BusinessSegmentId = metricMappingItem.BusinessSegmentId.Value,
                DivisionId = metricMappingItem.DivisionId.Value,
                FacilityId = metricMappingItem.FacilityId.Value,
                ProductLineId = metricMappingItem.ProductLineId.Value,
                DepartmentId = metricMappingItem.DepartmentId.Value,
                ProcessId = metricMappingItem.ProcessId.Value,
                KPIId = metricMappingItem.KPIId.Value,
                MetricId = metricMappingItem.MetricId.Value,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = true
            };
            metricMappingRepository.AddOrUpdate(metricMapping);
            metricMappingRepository.Save();
        }

        /// <summary>
        /// Method to update a metric mapping
        /// </summary>
        /// <param name="MetricMappingItem">Metric Mapping item to update</param>
        /// <param name="userName">logged in user name</param>
        private void UpdateMetricMapping(MetricMappingItem metricMappingItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            //get existing metric mapping details
            var existingMetricMapping = metricMappingRepository.Get(metricMappingItem.Id.Value);

            existingMetricMapping.BusinessSegmentId = metricMappingItem.BusinessSegmentId.Value;
            existingMetricMapping.DivisionId = metricMappingItem.DivisionId.Value;
            existingMetricMapping.FacilityId = metricMappingItem.FacilityId.Value;
            existingMetricMapping.ProductLineId = metricMappingItem.ProductLineId.Value;
            existingMetricMapping.DepartmentId = metricMappingItem.DepartmentId.Value;
            existingMetricMapping.ProcessId = metricMappingItem.ProcessId.Value;
            existingMetricMapping.KPIId = metricMappingItem.KPIId.Value;
            existingMetricMapping.MetricId = metricMappingItem.MetricId.Value;
            existingMetricMapping.IsActive = metricMappingItem.IsActive;
            existingMetricMapping.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingMetricMapping.LastModifiedBy = loggedInUserId;
            metricMappingRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a metric
        /// </summary>
        /// <param name="metricItem">Metric item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private string ValidateMetricMappingUpdateRequest(MetricMappingItem metricMappingItem)
        {
            string validationError = string.Empty;
            //get existing metric mapping details
            var existingMetricMapping = metricMappingRepository.Get(metricMappingItem.Id.Value);
            if (existingMetricMapping == null)
            {
                validationError = Constants.MetricMappingNotFound;
            }
            else
            {
                // If the request is to update the metric name it self, check if the 
                // new name already exists or not
                if (!(existingMetricMapping.BusinessSegmentId == metricMappingItem.BusinessSegmentId
                    && existingMetricMapping.DivisionId == metricMappingItem.DivisionId
                    && existingMetricMapping.FacilityId == metricMappingItem.FacilityId
                    && existingMetricMapping.ProductLineId == metricMappingItem.ProductLineId
                    && existingMetricMapping.DepartmentId == metricMappingItem.DepartmentId
                    && existingMetricMapping.ProcessId == metricMappingItem.ProcessId
                    && existingMetricMapping.KPIId == metricMappingItem.KPIId
                    && existingMetricMapping.MetricId == metricMappingItem.MetricId))
                {
                    //Check if the mapping set requested already exists 
                    if (metricMappingRepository.GetAll()
                       .Any(mm => mm.BusinessSegmentId == metricMappingItem.BusinessSegmentId
                       && mm.DivisionId == metricMappingItem.DivisionId
                       && mm.FacilityId == metricMappingItem.FacilityId
                       && mm.ProductLineId == metricMappingItem.ProductLineId
                       && mm.DepartmentId == metricMappingItem.DepartmentId
                       && mm.ProcessId == metricMappingItem.ProcessId
                       && mm.KPIId == metricMappingItem.KPIId
                       && mm.MetricId == metricMappingItem.MetricId))
                    {
                        validationError = Constants.MetricMappingExists;
                    }
                }
            }
            return validationError;
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="metricRepository">Metric Repository</param>
        public MetricManager(IMetricRepository metricRepo,
                            IBaseRepository<MetricMapping> metricMappingRepo,
                            IBaseRepository<User> userRepository)
        {
            if (metricRepo == null || metricMappingRepo == null
                || userRepository == null)
            {
                throw new ArgumentNullException("Repository", "The given parameter cannot be null.");
            }
            this.metricRepository = metricRepo;
            this.metricMappingRepository = metricMappingRepo;
            this.userRepository = userRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Method to retrieve all active metrics
        /// </summary>
        /// <returns>metric list</returns>
        public IEnumerable<MetricItem> GetAll()
        {
            var metrics = metricRepository.GetAll().ToList()
                                          .Select(m => ConvertMetricToMetricItemDTO(m)).OrderBy(m=>m.Name).ToList();
            return metrics;
        }

        /// <summary>
        /// Retrieve selected metric
        /// </summary>
        /// <param name="metricId">selected metric id</param>
        /// <returns>metric item</returns>
        public MetricItem Get(int metricId)
        {
            var metric = metricRepository.Get(metricId);
            return (metric != null) ? ConvertMetricToMetricItemDTO(metric) : null;
        }

        /// <summary>
        /// Retrieves all active goal types and data types 
        /// </summary>
        /// <returns>object with goal types and data types</returns>
        public MetricTemplateData GetMetricTemplateData()
        {
            return metricRepository.GetMetricTemplateData();
        }

        /// <summary>
        /// Get all metrics whose name starts with the input string mentioned.
        /// </summary>
        /// <param name="name">Input name to match</param>
        /// <returns>List of metrics whose name starts with the input string</returns>
        public IEnumerable<MetricSuggestion> GetMetricsWithName(string name)
        {
            return metricRepository.GetAll().Where(x => x.Name.StartsWith(name) && x.IsActive)
                                   .Select(m => new MetricSuggestion()
                                   {
                                       Id = m.Id,
                                       Name = m.Name
                                   }).ToList();
        }

        /// <summary>
        /// Add new metric mapping to database
        /// </summary>
        /// <param name="metricMappingRequest">metric mapping details object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>added metric mapping id</returns>
        public void AddorUpdateMetricMapping(IEnumerable<MetricMappingItem> metricMappingRequest,
            string userName)
        {

            List<string> validationErrors = new List<string>();
            foreach (var metricMappingItem in metricMappingRequest)
            {
                // update an existing metric mapping
                if (metricMappingItem.Id.HasValue)
                {
                    // Validate the metric update request first
                    var error = ValidateMetricMappingUpdateRequest(metricMappingItem);
                    if (!string.IsNullOrEmpty(error))
                    {
                        validationErrors.Add(error);
                    }
                    // if no validation error update the existing mapping
                    if (!validationErrors.Any())
                    {
                        UpdateMetricMapping(metricMappingItem, userName);
                    }
                }
                else
                {
                    if (metricMappingRepository.GetAll()
                       .Any(mm => mm.BusinessSegmentId == metricMappingItem.BusinessSegmentId
                       && mm.DivisionId == metricMappingItem.DivisionId
                       && mm.FacilityId == metricMappingItem.FacilityId
                       && mm.ProductLineId == metricMappingItem.ProductLineId
                       && mm.DepartmentId == metricMappingItem.DepartmentId
                       && mm.ProcessId == metricMappingItem.ProcessId
                       && mm.KPIId == metricMappingItem.KPIId
                       && mm.MetricId == metricMappingItem.MetricId))
                    {
                        validationErrors.Add(Constants.MetricMappingExists);
                    }
                    if (!validationErrors.Any())
                    {
                        AddMetricMapping(metricMappingItem, userName);
                    }
                }
            }

            if (validationErrors.Any())
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }

        /// <summary>
        /// Add or update metric to database
        /// </summary>
        /// <param name="metricRequest">list of metric object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddorUpdateMetric(IEnumerable<MetricItem> metricRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var metricItem in metricRequest)
            {
                // update an existing metric
                if (metricItem.Id.HasValue)
                {
                    // Validate the metric update request first
                    validationErrors = ValidateMetricUpdateRequest(metricItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateMetric(metricItem, userName);
                    }
                }
                // if we are going to add a new metric
                else
                {
                    // If any metric with same name exists, add to the validation errors
                    if (metricRepository.GetAll().Any(m => m.Name == metricItem.Name))
                    {
                        string errorMessage = string.Format(Constants.MetricExists, metricItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddMetric(metricItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }

        /// <summary>
        /// Method to retrieve all assigned metrics
        /// </summary>
        /// <returns>metric mapping list</returns>
        public IEnumerable<MetricMappingItem> GetAllMetricMappings()
        {
            return metricMappingRepository.GetAll().Select(mm => new MetricMappingItem()
            {
                Id = mm.Id,
                BusinessSegmentId = mm.BusinessSegment.Id,
                BusinessSegmentName = mm.BusinessSegment.Name,
                DivisionId = mm.Division.Id,
                DivisionName = mm.Division.Name,
                FacilityId = mm.Facility.Id,
                FacilityName = mm.Facility.Name,
                ProductLineId = mm.ProductLine.Id,
                ProductLineName = mm.ProductLine.Name,
                ProcessId = mm.Process.Id,
                ProcessName = mm.Process.Name,
                DepartmentId = mm.Department.Id,
                DepartmentName = mm.Department.Name,
                KPIId = mm.KPI.Id,
                KPIName = mm.KPI.Name,
                MetricId = mm.Metric.Id,
                MetricName = mm.Metric.Name,
                IsActive = mm.IsActive
            }).ToList();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (metricRepository != null)
                    {
                        metricRepository.Dispose();
                    }
                    if (metricMappingRepository != null)
                    {
                        metricMappingRepository.Dispose();
                    }
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    metricRepository = null;
                    metricMappingRepository = null;
                    userRepository = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
