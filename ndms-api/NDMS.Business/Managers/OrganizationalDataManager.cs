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
    public class OrganizationalDataManager : IOrganizationalDataManager
    {
        #region Field(s)
        /// <summary>
        /// Business Segment Repository
        /// </summary>
        private IBaseRepository<BusinessSegment> businessSegmentRepository;

        /// <summary>
        /// Productline Repository
        /// </summary>
        private IBaseRepository<ProductLine> productLineRepository;

        /// <summary>
        /// Process Repository
        /// </summary>
        private IBaseRepository<Process> processRepository;

        /// <summary>
        /// Department Repository
        /// </summary>
        private IBaseRepository<Department> departmentRepository;

        /// <summary>
        /// KPI Repository
        /// </summary>
        private IBaseRepository<KPI> kpiRepository;

        /// <summary>
        /// Division Repository
        /// </summary>
        private IBaseRepository<Division> divisionRepository; 

        /// <summary>
        /// Facility Repository
        /// </summary>
        private IBaseRepository<Facility> facilityRepository;

        /// <summary>
        /// holiday pattern Repository
        /// </summary>
        private IBaseRepository<HolidayPattern> holidayPatternRepository;

        /// <summary>
        /// organizational data Repository
        /// </summary>
        private IBaseRepository<OrganizationalData> organizationalDataRepository;

        /// <summary>
        /// IScorecardManager reference
        /// </summary>
        private IScorecardAdminManager scorecardManager;

        /// <summary>
        /// User Repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        #endregion

        #region Private Method(s)        

        /// <summary>
        /// Get active business segments
        /// </summary>
        /// <returns>The business segment list</returns>
        private List<BusinessSegmentItem> GetActiveBusinessSegments()
        {
            var businessSegments = new List<BusinessSegmentItem>();
            businessSegments = businessSegmentRepository.GetAll().Where(d => d.IsActive).
                OrderBy(d => d.Id).Select(d => new BusinessSegmentItem()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            businessSegments = businessSegments.OrderByDescending(x => x.Id == 0).ThenBy(x => x.Name).ToList();
            return businessSegments;
        }

        /// <summary>
        /// Get active divisions
        /// </summary>
        /// <returns>The division list</returns>
        private List<DivisionItem> GetActiveDivisions()
        {
            var divisions = new List<DivisionItem>();
            divisions = divisionRepository.GetAll().Where(d => d.IsActive).
                OrderBy(d => d.Id).Select(d => new DivisionItem()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            divisions = divisions.OrderByDescending(x => x.Id == 0).ThenBy(x => x.Name).ToList();
            return divisions;
        }

        /// <summary>
        /// Get active facilities
        /// </summary>
        /// <returns>The facility list</returns>
        private List<FacilityItem> GetActiveFacilities()
        {
            var facilities = new List<FacilityItem>();
            facilities = facilityRepository.GetAll().Where(f => f.IsActive).
                    OrderBy(f => f.Id).Select(f => new FacilityItem()
                    {
                        Id = f.Id,
                        Name = f.Name
                    }).ToList();
            facilities = facilities.OrderByDescending(d => d.Id == 0).ThenBy(d => d.Name).ToList();
            return facilities;
        }

        /// <summary>
        /// Get all active product line
        /// </summary>
        /// <returns>The product line list</returns>
        private List<ProductLineItem> GetActiveProductLines()
        {
            var productLines = new List<ProductLineItem>();
            productLines = productLineRepository.GetAll().Where(pl => pl.IsActive).
                    OrderBy(pl => pl.Id).Select(pl => new ProductLineItem()
                    {
                        Id = pl.Id,
                        Name = pl.Name
                    }).ToList();
            productLines = productLines.OrderByDescending(pl => pl.Id == 0).ThenBy(pl => pl.Name).ToList();
            return productLines;
        }

        /// <summary>
        /// Get all active processes
        /// </summary>
        /// <returns>The process list</returns>
        private List<ProcessItem> GetActiveProcesses()
        {
            var processes = new List<ProcessItem>();
            processes = processRepository.GetAll().Where(p => p.IsActive).
                   OrderBy(p => p.Id).Select(p => new ProcessItem()
                   {
                       Id = p.Id,
                       Name = p.Name
                   }).ToList();           
            processes =processes.OrderByDescending(p => p.Id == 0).ThenBy(p => p.Name).ToList();

            return processes;
        }

        /// <summary>
        /// Get all active departments
        /// </summary>
        /// <returns>The department list</returns>
        private List<DepartmentItem> GetActiveDepartments()
        {
            var departments = new List<DepartmentItem>();
            departments = departmentRepository.GetAll().Where(d => d.IsActive).
                    OrderBy(d => d.Id).Select(d => new DepartmentItem()
                    {
                        Id = d.Id,
                        Name = d.Name
                    }).ToList();
            departments=departments.OrderByDescending(d => d.Id == 0).ThenBy(d => d.Name).ToList();
            return departments;
        }

        /// <summary>
        /// Get all active kpi's
        /// </summary>
        /// <returns>List of KPI's</returns>
        private List<KPIItem> GetAllKPIs()
        {
            var kpis = new List<KPIItem>();
            kpis = kpiRepository.GetAll().Where(d => d.IsActive).
                OrderBy(d => d.Id).Select(d => new KPIItem()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

            return kpis;
        }

        /// <summary>
        /// Get all active kpi's
        /// </summary>
        /// <returns>List of KPI's</returns>
        private List<ScorecardHolidayPatternItem> GetAllScorecardHolidayPatterns()
        {
            var holidayPatterns = new List<ScorecardHolidayPatternItem>();
            holidayPatterns = holidayPatternRepository.GetAll().Where(d => d.IsActive).
                OrderBy(d => d.Id).Select(d => new ScorecardHolidayPatternItem()
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            holidayPatterns = holidayPatterns.OrderByDescending(x => x.Id == 1).ThenBy(x => x.Name).ToList();
            return holidayPatterns;
        }

        /// <summary>
        /// Method to add a business segment to database
        /// </summary>
        /// <param name="businessSegmentItem">business segment item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddBusinessSegment(BusinessSegmentItem businessSegmentItem,string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var businessSegment = new BusinessSegment()
            {
                Name = businessSegmentItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = businessSegmentItem.IsActive
            };
            businessSegmentRepository.AddOrUpdate(businessSegment);
            businessSegmentRepository.Save();
        }

        /// <summary>
        /// Method to update a business segment item
        /// </summary>
        /// <param name="businessSegmentItem">business segment item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateBusinessSegment(BusinessSegmentItem businessSegmentItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing business segment details
            var existingBusinessSegment = businessSegmentRepository.Get(businessSegmentItem.Id.Value);
            existingBusinessSegment.Name = businessSegmentItem.Name;
            existingBusinessSegment.IsActive = businessSegmentItem.IsActive;
            existingBusinessSegment.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingBusinessSegment.LastModifiedBy = loggedInUserId;
            businessSegmentRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a business segment
        /// </summary>
        /// <param name="businessSegmentItem">Business segment item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateBusinessSegmentUpdateRequest(BusinessSegmentItem businessSegmentItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing business segment details
            var existingBusinessSegment = businessSegmentRepository.Get(businessSegmentItem.Id.Value);
            if (existingBusinessSegment == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    businessSegmentItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the business segment name it self, check if the 
                // new name already exists or not
                if (existingBusinessSegment.Name != businessSegmentItem.Name)
                {
                    if (businessSegmentRepository.GetAll().Any(m => m.Name == businessSegmentItem.Name && m.Id != businessSegmentItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            businessSegmentItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
                                      }
            return validationErrors;
        }
        
        /// <summary>
        /// Method to add a division to database
        /// </summary>
        /// <param name="divisiontItem">business segment item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddDivision(DivisionItem divisiontItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var division = new Division()
            {
                Name = divisiontItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = divisiontItem.IsActive
            };
            divisionRepository.AddOrUpdate(division);
            divisionRepository.Save();
        }

        /// <summary>
        /// Method to update a division item
        /// </summary>
        /// <param name="divisionItem">division item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateDivision(DivisionItem divisionItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing division details
            var existingDivision = divisionRepository.Get(divisionItem.Id.Value);
            existingDivision.Name = divisionItem.Name;
            existingDivision.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingDivision.LastModifiedBy = loggedInUserId;
            existingDivision.IsActive = divisionItem.IsActive;
            divisionRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a division
        /// </summary>
        /// <param name="divisionItem">Division item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateDivisionUpdateRequest(DivisionItem divisionItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing division details
            var existingDivision = divisionRepository.Get(divisionItem.Id.Value);
            if (existingDivision == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    divisionItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the division name it self, check if the 
                // new name already exists or not
                if (existingDivision.Name != divisionItem.Name)
                {
                    if (divisionRepository.GetAll().Any(m => m.Name == divisionItem.Name && m.Id != divisionItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            divisionItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }

        /// <summary>
        /// Method to add a facility to database
        /// </summary>
        /// <param name="facilityItem">facility item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddFacility(FacilityItem facilityItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var facility = new Facility()
            {
                Name = facilityItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = facilityItem.IsActive
            };
            facilityRepository.AddOrUpdate(facility);
            facilityRepository.Save();
        }
        
        /// <summary>
        /// Method to update a facility item
        /// </summary>
        /// <param name="facilityItem">facility item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateFacility(FacilityItem facilityItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing business segment details
            var existingfacility = facilityRepository.Get(facilityItem.Id.Value);
            existingfacility.Name = facilityItem.Name;
            existingfacility.IsActive = facilityItem.IsActive;
            existingfacility.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingfacility.LastModifiedBy = loggedInUserId;
            facilityRepository.Save();
        }
        
        /// <summary>
        /// Validates an update request for a facility
        /// </summary>
        /// <param name="facilityItem">facility item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateFacilityUpdateRequest(FacilityItem facilityItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing facility details
            var existingfacility = facilityRepository.Get(facilityItem.Id.Value);
            if (existingfacility == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    facilityItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the facility name it self, check if the 
                // new name already exists or not
                if (existingfacility.Name != facilityItem.Name)
                {
                    if (facilityRepository.GetAll().Any(m => m.Name == facilityItem.Name && m.Id != facilityItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            facilityItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }
        
        /// <summary>
        /// Method to add a product line to database
        /// </summary>
        /// <param name="productLineItem">product line item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddProductLine(ProductLineItem productLineItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var productLine = new ProductLine()
            {
                Name = productLineItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = productLineItem.IsActive
            };
            productLineRepository.AddOrUpdate(productLine);
            productLineRepository.Save();
        }

        /// <summary>
        /// Method to update a product line item
        /// </summary>
        /// <param name="productLineItem">product line item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateProductLine(ProductLineItem productLineItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing product line details
            var existingproductLine = productLineRepository.Get(productLineItem.Id.Value);
            existingproductLine.Name = productLineItem.Name;
            existingproductLine.IsActive = productLineItem.IsActive;
            existingproductLine.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingproductLine.LastModifiedBy = loggedInUserId;
            productLineRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a product line
        /// </summary>
        /// <param name="productLineItem">product line item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateProductLineUpdateRequest(ProductLineItem productLineItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing product line details
            var existingproductLine = productLineRepository.Get(productLineItem.Id.Value);
            if (existingproductLine == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    productLineItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the product line name it self, check if the 
                // new name already exists or not
                if (existingproductLine.Name != productLineItem.Name)
                {
                    if (productLineRepository.GetAll().Any(m => m.Name == productLineItem.Name && m.Id != productLineItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            productLineItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }

        /// <summary>
        /// Method to add a department to database
        /// </summary>
        /// <param name="departmentItem">department item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddDepartment(DepartmentItem departmentItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var department = new Department()
            {
                Name = departmentItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = departmentItem.IsActive
            };
            departmentRepository.AddOrUpdate(department);
            departmentRepository.Save();
        }

        /// <summary>
        /// Method to update a department item
        /// </summary>
        /// <param name="departmentItem">department item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateDepartment(DepartmentItem departmentItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing department line details
            var existingDepartment = departmentRepository.Get(departmentItem.Id.Value);
            existingDepartment.Name = departmentItem.Name;
            existingDepartment.IsActive = departmentItem.IsActive;
            existingDepartment.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingDepartment.LastModifiedBy = loggedInUserId;
            departmentRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a department
        /// </summary>
        /// <param name="departmentItem">department item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateDepartmentUpdateRequest(DepartmentItem departmentItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing department details
            var existingDepartment = departmentRepository.Get(departmentItem.Id.Value);
            if (existingDepartment == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    departmentItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the department name it self, check if the 
                // new name already exists or not
                if (existingDepartment.Name != departmentItem.Name)
                {
                    if (departmentRepository.GetAll().Any(m => m.Name == departmentItem.Name && m.Id != departmentItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            departmentItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }

        /// <summary>
        /// Method to add a process to database
        /// </summary>
        /// <param name="processItem">process item to be added</param>
        /// <param name="userName">logged in user name</param>
        private void AddProcess(ProcessItem processItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                x => x.AccountName == userName).Id;
            var process = new Process()
            {
                Name = processItem.Name,
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = processItem.IsActive
            };
            processRepository.AddOrUpdate(process);
            processRepository.Save();
        }

        /// <summary>
        /// Method to update a process item
        /// </summary>
        /// <param name="processItem">process item to update</param>
        ///  <param name="userName">logged in user name</param>
        private void UpdateProcess(ProcessItem processItem, string userName)
        {
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
                 x => x.AccountName == userName).Id;
            //get existing process line details
            var existingProcess = processRepository.Get(processItem.Id.Value);
            existingProcess.Name = processItem.Name;
            existingProcess.IsActive = processItem.IsActive;
            existingProcess.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            existingProcess.LastModifiedBy = loggedInUserId;
            processRepository.Save();
        }

        /// <summary>
        /// Validates an update request for a process
        /// </summary>
        /// <param name="processItem">process item to update</param>
        /// <returns>List of validation errors of exists</returns>
        private List<string> ValidateProcessUpdateRequest(ProcessItem processItem)
        {
            List<string> validationErrors = new List<string>();
            //get existing process details
            var existingProcess = processRepository.Get(processItem.Id.Value);
            if (existingProcess == null)
            {
                string errorMessage = string.Format(Constants.OrganizationalDataNotFound,
                    processItem.Name);
                validationErrors.Add(errorMessage);
            }
            else
            {
                // If the request is to update the process name it self, check if the 
                // new name already exists or not
                if (existingProcess.Name != processItem.Name)
                {
                    if (processRepository.GetAll().Any(m => m.Name == processItem.Name && m.Id != processItem.Id))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists,
                            processItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                }
            }
            return validationErrors;
        }

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="businessSegmentRepository">Business Segment Repository</param>
        /// <param name="divisionRepository">Division Repository</param>
        /// <param name="facilityRepository">Facility Repository</param>
        /// <param name="productLineRepository">ProductLine Repository</param>
        /// <param name="departmentRepository">Department Repository</param>
        /// <param name="processRepository">Process Repository</param>
        /// <param name="kpiRepository">KPI Repository</param>
        ///  /// <param name="holidayPatternRepository">HolidayPattern Repository</param>
        /// <param name="scorecardManager">Scorecard manager</param>
        public OrganizationalDataManager(IBaseRepository<BusinessSegment> businessSegmentRepository,
            IBaseRepository<Division> divisionRepository, IBaseRepository<Facility> facilityRepository,
            IBaseRepository<ProductLine> productLineRepository, IBaseRepository<Department> departmentRepository,
            IBaseRepository<Process> processRepository, IBaseRepository<KPI> kpiRepository,
            IBaseRepository<HolidayPattern> holidayPatternRepository, IBaseRepository<User> userRepository,
             IBaseRepository<OrganizationalData> organizationalDataRepository, IScorecardAdminManager scorecardManager)
        {
            if (businessSegmentRepository == null || divisionRepository == null || facilityRepository == null
                || productLineRepository == null || departmentRepository == null || processRepository == null
                || kpiRepository == null || holidayPatternRepository == null||
                organizationalDataRepository == null || scorecardManager == null)
            {
                throw new ArgumentNullException("Repository", "The given parameter cannot be null.");
            }
            this.businessSegmentRepository = businessSegmentRepository;
            this.productLineRepository = productLineRepository;
            this.processRepository = processRepository;
            this.departmentRepository = departmentRepository;
            this.kpiRepository = kpiRepository;
            this.divisionRepository = divisionRepository;
            this.facilityRepository = facilityRepository;
            this.scorecardManager = scorecardManager;
            this.holidayPatternRepository = holidayPatternRepository;
            this.userRepository = userRepository;
            this.organizationalDataRepository = organizationalDataRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Method to get list of kpi, business segment, product line, process and department
        /// </summary>
        /// <param name="parentScorecardId">ID of parent scorecard</param>
        /// <returns>object with list of kpi, business segment, product line, process and 
        /// department</returns>
        public ScorecardTemplateData GetScorecardTemplateData(int? parentScorecardId)
        {
            ScorecardTemplateData template = new ScorecardTemplateData();
            if (parentScorecardId.HasValue)
            {
                //Get parent scorecard
                template.ParentScorecardItem = scorecardManager.GetScorecard
                    (parentScorecardId.Value);
                // No need to keep the KPI owners, teams and KPI's from parent score card
                template.ParentScorecardItem.KPIOwners = null;
                template.ParentScorecardItem.Teams = null;
                template.ParentScorecardItem.KPIs = null;
            }
            // Forms the template data to add new score card
            template.BusinessSegments = GetActiveBusinessSegments();
            template.Divisions = GetActiveDivisions();
            template.Facilities = GetActiveFacilities();
            template.ProductLines = GetActiveProductLines();
            template.Processes = GetActiveProcesses();
            template.Departments = GetActiveDepartments();
            template.KPIs = GetAllKPIs();
            template.ScorecardHolidayPatterns = GetAllScorecardHolidayPatterns();
            template.RootScorecardId = scorecardManager.GetRootScorecardOfTree(parentScorecardId);
            return template;
        }        

        /// <summary>
        /// Method to get list of kpi, business segment, product line, process and department
        /// </summary>
        /// <returns>object with list of kpi, business segment, product line, process and department</returns>
        public MetricMappingTemplateData GetMetricMappingTemplateData()
        {
            // Forms the template data to add new metric mapping
            MetricMappingTemplateData template = new MetricMappingTemplateData();
            template.BusinessSegments = GetActiveBusinessSegments();
            template.Divisions = GetActiveDivisions();
            template.Facilities = GetActiveFacilities();
            template.ProductLines = GetActiveProductLines();
            template.Processes = GetActiveProcesses();
            template.Departments = GetActiveDepartments();
            template.KPIs = GetAllKPIs();
            template.KPIs = template.KPIs.OrderBy(p => p.Name).ToList();
            return template;
        }

        /// <summary>
        /// Get all business segments
        /// </summary>
        /// <returns>List of business segments</returns>
        public List<BusinessSegmentItem> GetAllBusinessSegments()
        {
            var businessSegments = new List<BusinessSegmentItem>();
            businessSegments = businessSegmentRepository.GetAll().
                OrderBy(d => d.Id).Select(d => new BusinessSegmentItem()
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive
                }).ToList();
            businessSegments = businessSegments.OrderByDescending(x => x.Id == 0).ThenBy(x => x.Name).ToList();
            return businessSegments;
        }

        /// <summary>
        /// Get all divisions
        /// </summary>
        /// <returns>List of divisions</returns>
        public List<DivisionItem> GetAllDivisions()
        {
            var divisions = new List<DivisionItem>();
            divisions = divisionRepository.GetAll().
                OrderBy(d => d.Id).Select(d => new DivisionItem()
                {
                    Id = d.Id,
                    Name = d.Name,
                     IsActive = d.IsActive
                }).ToList();
            divisions = divisions.OrderByDescending(x => x.Id == 0).ThenBy(x => x.Name).ToList();
            return divisions;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>List of departments</returns>
        public List<DepartmentItem> GetAllDepartments()
        {
            var departments = new List<DepartmentItem>();
            departments = departmentRepository.GetAll().
                    OrderBy(d => d.Id).Select(d => new DepartmentItem()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        IsActive = d.IsActive
                    }).ToList();
            departments = departments.OrderByDescending(d => d.Id == 0).ThenBy(d => d.Name).ToList();
            return departments;
        }

        /// <summary>
        /// Get all facilities
        /// </summary>
        /// <returns>The facility list</returns>
        public List<FacilityItem> GetAllFacilities()
        {
            var facilities = new List<FacilityItem>();
            facilities = facilityRepository.GetAll().
                OrderBy(f => f.Id).
                Select(f => new FacilityItem()
                {
                    Id = f.Id,
                    Name = f.Name,
                    IsActive = f.IsActive
                }).ToList();
            facilities = facilities.OrderByDescending(f => f.Id == 0).ThenBy(f => f.Name).ToList();
            return facilities;
        }

        /// <summary>
        /// Get all product line
        /// </summary>
        /// <returns>The product line list</returns>
        public List<ProductLineItem> GetAllProductLines()
        {
            var productLines = new List<ProductLineItem>();
            productLines = productLineRepository.GetAll().
                OrderBy(pl => pl.Id).
                Select(pl => new ProductLineItem()
                {
                    Id = pl.Id,
                    Name = pl.Name,
                    IsActive = pl.IsActive
                }).ToList();
            productLines = productLines.OrderByDescending(pl => pl.Id == 0).ThenBy(pl => pl.Name).ToList();
            return productLines;
        }

        /// <summary>
        /// Get all processes
        /// </summary>
        /// <returns>The process list</returns>
        public List<ProcessItem> GetAllProcesses()
        {
            var processes = new List<ProcessItem>();
            processes = processRepository.GetAll().
                OrderBy(p => p.Id).
                Select(p => new ProcessItem()
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsActive = p.IsActive
                }).ToList();
            processes = processes.OrderByDescending(p => p.Id == 0).ThenBy(p => p.Name).ToList();

            return processes;
        }

        /// <summary>
        /// Add or update business segment to database
        /// </summary>
        /// <param name="businessSegmentRequest">list of business segment object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateBusinessSegments(IEnumerable<BusinessSegmentItem> businessSegmentRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var businessSegmentItem in businessSegmentRequest)
            {
                // update an existing business segment
                if (businessSegmentItem.Id.HasValue)
                {
                    // Validate the business segment update request first
                    validationErrors = ValidateBusinessSegmentUpdateRequest(businessSegmentItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateBusinessSegment(businessSegmentItem, userName);
                    }
                }
                // if we are going to add a new business segment
                else
                {
                    // If any business segment with same name exists, add to the validation errors
                    if (businessSegmentRepository.GetAll().Any(m => m.Name == businessSegmentItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, businessSegmentItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddBusinessSegment(businessSegmentItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }

        /// <summary>
        /// Add or update division to database
        /// </summary>
        /// <param name="divisionRequest">list of division object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateDivisions(IEnumerable<DivisionItem> divisionRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var divisionItem in divisionRequest)
            {
                // update an existing division
                if (divisionItem.Id.HasValue)
                {
                    // Validate the division update request first
                    validationErrors = ValidateDivisionUpdateRequest(divisionItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateDivision(divisionItem, userName);
                    }
                }
                // if we are going to add a new division
                else
                {
                    // If any division with same name exists, add to the validation errors
                    if (divisionRepository.GetAll().Any(m => m.Name == divisionItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, divisionItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddDivision(divisionItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }
        
        /// <summary>
        /// Add or update facility to database
        /// </summary>
        /// <param name="facilityRequest">list of facility object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateFacilities(IEnumerable<FacilityItem> facilityRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var facilityItem in facilityRequest)
            {
                // update an existing facility
                if (facilityItem.Id.HasValue)
                {
                    // Validate the facility update request first
                    validationErrors = ValidateFacilityUpdateRequest(facilityItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateFacility(facilityItem, userName);
                    }
                }
                // if we are going to add a new facility
                else
                {
                    // If any facility with same name exists, add to the validation errors
                    if (facilityRepository.GetAll().Any(m => m.Name == facilityItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, facilityItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddFacility(facilityItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }
        
        /// <summary>
        /// Add or update product line to database
        /// </summary>
        /// <param name="productLineRequest">list of product line object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateProductLines(IEnumerable<ProductLineItem> productLineRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var productLineItem in productLineRequest)
            {
                // update an existing product line
                if (productLineItem.Id.HasValue)
                {
                    // Validate the product line update request first
                    validationErrors = ValidateProductLineUpdateRequest(productLineItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateProductLine(productLineItem, userName);
                    }
                }
                // if we are going to add a new product line
                else
                {
                    // If any product line with same name exists, add to the validation errors
                    if (facilityRepository.GetAll().Any(m => m.Name == productLineItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, productLineItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddProductLine(productLineItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }
        
        /// <summary>
        /// Add or update department to database
        /// </summary>
        /// <param name="departmentRequest">list of department object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateDepartments(IEnumerable<DepartmentItem> departmentRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var departmentItem in departmentRequest)
            {
                // update an existing department
                if (departmentItem.Id.HasValue)
                {
                    // Validate the department update request first
                    validationErrors = ValidateDepartmentUpdateRequest(departmentItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateDepartment(departmentItem, userName);
                    }
                }
                // if we are going to add a new department
                else
                {
                    // If any department with same name exists, add to the validation errors
                    if (departmentRepository.GetAll().Any(m => m.Name == departmentItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, departmentItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddDepartment(departmentItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }
        
        /// <summary>
        /// Add or update process to database
        /// </summary>
        /// <param name="processRequest">list of process object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns></returns>
        public void AddOrUpdateProcess(IEnumerable<ProcessItem> processRequest, string userName)
        {
            List<string> validationErrors = new List<string>();
            foreach (var processItem in processRequest)
            {
                // update an existing process
                if (processItem.Id.HasValue)
                {
                    // Validate the process update request first
                    validationErrors = ValidateProcessUpdateRequest(processItem);
                    if (validationErrors.Count <= 0)
                    {
                        UpdateProcess(processItem, userName);
                    }
                }
                // if we are going to add a new process
                else
                {
                    // If any process with same name exists, add to the validation errors
                    if (processRepository.GetAll().Any(m => m.Name == processItem.Name))
                    {
                        string errorMessage = string.Format(Constants.OrganizationalDataExists, processItem.Name);
                        validationErrors.Add(errorMessage);
                    }
                    if (validationErrors.Count <= 0)
                    {
                        AddProcess(processItem, userName);
                    }
                }
            }

            if (validationErrors.Count > 0)
            {
                throw new NDMSBusinessException(validationErrors);
            }
        }

        /// <summary>
        /// Get organizational data
        /// </summary>
        /// <returns>
        /// The organizational data
        /// </returns>
        public List<OrganizationalDataItem> GetOrganizationalData()
        {
            var organizationalData = new List<OrganizationalDataItem>();
            organizationalData = organizationalDataRepository.GetAll().Where(o => o.IsActive)
                .OrderBy(o => o.Id).Select(
                  o => new OrganizationalDataItem
                  {
                      Id = o.Id,
                      Name = o.Name
                  }
                ).ToList();
            return organizationalData;
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
                    if (businessSegmentRepository != null)
                    {
                        businessSegmentRepository.Dispose();
                    }
                    if (productLineRepository != null)
                    {
                        productLineRepository.Dispose();
                    }
                    if (processRepository != null)
                    {
                        processRepository.Dispose();
                    }
                    if (departmentRepository != null)
                    {
                        departmentRepository.Dispose();
                    }
                    if (kpiRepository != null)
                    {
                        kpiRepository.Dispose();
                    }
                    if (divisionRepository != null)
                    {
                        divisionRepository.Dispose();
                    }
                    if (facilityRepository != null)
                    {
                        facilityRepository.Dispose();
                    }
                    if(holidayPatternRepository != null)
                    {
                        holidayPatternRepository.Dispose();
                    }
                    if (scorecardManager != null)
                    {
                        scorecardManager.Dispose();
                    }
                    if(organizationalDataRepository != null)
                    {
                        organizationalDataRepository.Dispose();
                    }
                    
                    businessSegmentRepository = null;
                    productLineRepository = null;
                    processRepository = null;
                    departmentRepository = null;
                    kpiRepository = null;
                    divisionRepository = null;
                    facilityRepository = null;
                    holidayPatternRepository = null;
                    organizationalDataRepository = null;
                    scorecardManager = null;

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
