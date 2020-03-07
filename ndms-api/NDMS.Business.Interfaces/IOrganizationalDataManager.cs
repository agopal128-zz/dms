using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    public interface IOrganizationalDataManager : IDisposable
    {
        /// <summary>
        /// Retrieves all kpis, business segments, productlines, processes and departments
        /// </summary>
        /// <param name="parentScorecardId">ID of parent scorecard</param>
        /// <returns>object with list of kpi, business segment, productline, process and department</returns>
        ScorecardTemplateData GetScorecardTemplateData(int? parentScorecardId);
        

        /// <summary>
        /// Method to get list of kpi, business segment, product line, process and department
        /// </summary>
        /// <returns>object with list of kpi, business segment, product line, process and department</returns>
        MetricMappingTemplateData GetMetricMappingTemplateData();

        /// <summary>
        /// Get all business segments
        /// </summary>
        /// <returns>List of business segments</returns>
        List<BusinessSegmentItem> GetAllBusinessSegments();

        /// <summary>
        /// Get all divisions
        /// </summary>
        /// <returns>List of divisions</returns>
        List<DivisionItem> GetAllDivisions();

        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>List of departments</returns>
        List<DepartmentItem> GetAllDepartments();

        /// <summary>
        /// Get all facilities
        /// </summary>
        /// <returns>The facility list</returns>
        List<FacilityItem> GetAllFacilities();

        /// <summary>
        /// Get all product line
        /// </summary>
        /// <returns>The product line list</returns>
        List<ProductLineItem> GetAllProductLines();

        /// <summary>
        /// Get all processes
        /// </summary>
        /// <returns>The process list</returns>
        List<ProcessItem> GetAllProcesses();

        /// <summary>
        /// Add or Update business segment 
        /// </summary>
        /// <param name="businessSegmentList">list of business segment object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateBusinessSegments(IEnumerable<BusinessSegmentItem> businessSegmentRequest, string userName);

        /// <summary>
        /// Add or Update division 
        /// </summary>
        /// <param name="divisionRequest">list of division object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateDivisions(IEnumerable<DivisionItem> divisionRequest, string userName);

       /// <summary>
        /// Add or Update facility 
        /// </summary>
        /// <param name="facilityRequest">list of facility object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateFacilities(IEnumerable<FacilityItem> facilityRequest, string username);
        
        /// <summary>
        /// Add or Update product line 
        /// </summary>
        /// <param name="productLineRequest">list of product line object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateProductLines(IEnumerable<ProductLineItem> productLineRequest, string username);

        /// <summary>
        /// Add or Update department
        /// </summary>
        /// <param name="departmentRequest">list of department object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateDepartments(IEnumerable<DepartmentItem> departmentRequest, string username);
        
        /// <summary>
        /// Add or Update process
        /// </summary>
        /// <param name="processRequest">list of process object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        void AddOrUpdateProcess(IEnumerable<ProcessItem> processRequest, string username);

        /// <summary>
        /// Get organizational data
        /// </summary>
        /// <returns>The organizational data</returns>
        List<OrganizationalDataItem> GetOrganizationalData();

    }
}
