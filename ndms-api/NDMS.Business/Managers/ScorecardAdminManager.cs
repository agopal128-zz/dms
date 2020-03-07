using NDMS.Business.Common;
using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Implements IScorecardAdminManager interface which handles score administration related 
    /// functionalities
    /// </summary>
    public class ScorecardAdminManager : IScorecardAdminManager
    {
        #region Field(s)
        /// <summary>
        /// KPI Repository
        /// </summary>
        private IBaseRepository<KPI> kpiRepository;

        /// <summary>
        /// Scorecard Repository
        /// </summary>
        private IScorecardRepository scorecardRepository;

        /// <summary>
        /// Scorecard KPI Owner Repository
        /// </summary>
        private IBaseRepository<ScorecardKPIOwner> scorecardKPIOwnerRepository;

        /// <summary>
        /// Scorecard Team Repository
        /// </summary>
        private IBaseRepository<ScorecardTeam> scorecardTeamRepository;

        /// <summary>
        /// Recordable Repository
        /// </summary>
        private IBaseRepository<Recordable> recordableRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// User Repository
        /// </summary>
        private INDMSUserRepository userRepository;

        /// <summary>
        /// Workday Repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository;

        /// <summary>
        /// Holiday Repository
        /// </summary>
        private IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;
        /// <summary>
        /// Daily actual repository
        /// </summary>
        private IBaseRepository<DailyActual> dailyActualRepository;

        /// <summary>
        /// Monthly actual repository
        /// </summary>
        private IBaseRepository<MonthlyActual> monthlyActualRepository;

        /// <summary>
        /// Recordables Calculator
        /// </summary>
        private ScorecardRecordablesCalculator recordablesCalculator;

        ///<summary>
        /// Reference to MTD Calculator
        /// </summary>
        private MTDPerformanceCalculator mtdCalculator;

        /// <summary>
        /// Reference to User Manager
        /// </summary>
        private UserManager userManager;


        /// <summary>
        /// The scorecard workday tracker repository
        /// </summary>
        private IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository;

        /// <summary>
        /// Business Segment Repository
        /// </summary>
        private IBaseRepository<BusinessSegment> businessSegmentRepository;

        /// <summary>
        /// Division Repository
        /// </summary>
        private IBaseRepository<Division> divisionRepository;

        /// <summary>
        /// Facility Repository
        /// </summary>
        private IBaseRepository<Facility> facilityRepository;

        /// <summary>
        /// Product Line Repository
        /// </summary>
        private IBaseRepository<ProductLine> productLineRepository;

        /// <summary>
        /// Department Repository
        /// </summary>
        private IBaseRepository<Department> departmentRepository;

        /// <summary>
        /// Process Repository
        /// </summary>
        private IBaseRepository<Process> processRepository;
        #endregion

        #region Private Method(s)
        /// <summary>
        /// converting scorecard entity to scorecard item DTO
        /// </summary>
        /// <param name="scorecard">scorecard entity</param>
        /// <returns>scorecard item DTO</returns>
        private ScorecardItem ConvertScorecardToScorecardItemDTO(Scorecard scorecard)
        {
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            DateTime CurrentMonthEnd = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            DateTime NextStartDate = CurrentMonthEnd.AddDays(1).Date;
            DateTime CurrentMonthStart = new DateTime(currentDate.Year, currentDate.Month, 1);
            return new ScorecardItem()
            {
                Id = scorecard.Id,
                ParentScorecardId = scorecard.ParentScorecardId,
                Name = scorecard.Name,
                BusinessSegments = (scorecard.BusinessSegments != null) ? scorecard.BusinessSegments
                               .Select(businessSegment => new BusinessSegmentItem()
                               {
                                   Id = businessSegment.Id,
                                   Name = businessSegment.Name,
                                   IsActive =businessSegment.IsActive
                               }).ToList() : null,
                Divisions = (scorecard.Divisions != null) ? scorecard.Divisions
                .Select(division => new DivisionItem()
                {
                    Id = division.Id,
                    Name = division.Name,
                    IsActive = division.IsActive
                }).ToList() : null,
                Facilities = (scorecard.Facilities != null) ? scorecard.Facilities
                               .Select(facility => new FacilityItem()
                               {
                                   Id = facility.Id,
                                   Name = facility.Name,
                                   IsActive = facility.IsActive
                               }).ToList() : null,
                ProductLines = (scorecard.ProductLines != null) ? scorecard.ProductLines
                                .Select(productLine => new ProductLineItem()
                                {
                                    Id = productLine.Id,
                                    Name = productLine.Name,
                                    IsActive = productLine.IsActive
                                }).ToList() : null,
                Processes = (scorecard.Processes != null) ? scorecard.Processes
                             .Select(process => new ProcessItem()
                             {
                                 Id = process.Id,
                                 Name = process.Name,
                                 IsActive = process.IsActive
                             }).ToList() : null,
                Departments = (scorecard.Departments != null) ? scorecard.Departments
                               .Select(department => new DepartmentItem()
                               {
                                   Id = department.Id,
                                   Name = department.Name,
                                   IsActive = department.IsActive
                               }).ToList() : null,
                IsActive = scorecard.IsActive,
                IsBowlingChartApplicable = scorecard.IsBowlingChartApplicable,
                DrilldownLevel = scorecard.DrilldownLevel,
                Recordable = (scorecard.Recordables != null && scorecard.Recordables.Any(x => x.IsActive)) ?
                                    scorecard.Recordables.Where(x => x.IsActive).OrderByDescending(x => x.RecordableDate)
                                    .Select(recordable => new RecordableItem
                                    {
                                        RecordableDate = recordable.RecordableDate,
                                        IsManual = recordable.IsManual
                                    }).First() : null,
                KPIOwners = (scorecard.KPIOwners != null) ? scorecard.KPIOwners.Where(x => x.IsActive)
                               .Select(owner => new KPIOwnerItem()
                               {
                                   AccountName = owner.User.AccountName,
                                   FullName = owner.User.FirstName + " " + owner.User.LastName
                                    + "(" + owner.User.AccountName + ")"
                               }).ToList() : null,
                Teams = (scorecard.Teams != null) ? scorecard.Teams.Where(x => x.IsActive)
                               .Select(team => new ScorecardTeamItem()
                               {
                                   AccountName = team.User.AccountName,
                                   FullName = team.User.FirstName + " " + team.User.LastName
                                    + "(" + team.User.AccountName + ")"
                               }).ToList() : null,
                KPIs = (scorecard.KPIs != null) ? scorecard.KPIs
                               .Select(kpi => new KPIItem()
                               {
                                   Id = kpi.Id,
                                   Name = kpi.Name
                               }).ToList() : null,
                ScorecardWorkdayPattern = (scorecard.ScorecardWorkdayPatterns != null) ?
                               scorecard.ScorecardWorkdayPatterns.Where(x => x.ScorecardId == scorecard.Id && x.EffectiveEndDate == null)
                               .Select(workdaypatterns => new ScorecardWorkdayPatternItem()
                               {
                                   Id = workdaypatterns.Id,
                                   ScorecardId = workdaypatterns.ScorecardId,
                                   IsSunday = workdaypatterns.IsSunday,
                                   IsMonday = workdaypatterns.IsMonday,
                                   IsTuesday = workdaypatterns.IsTuesday,
                                   IsWednesday = workdaypatterns.IsWednesday,
                                   IsThursday = workdaypatterns.IsThursday,
                                   IsFriday = workdaypatterns.IsFriday,
                                   IsSaturday = workdaypatterns.IsSaturday,
                                   EffectiveStartDate = workdaypatterns.EffectiveStartDate,
                                   EffectiveEndDate = workdaypatterns.EffectiveEndDate
                               }).FirstOrDefault() : null,

                ActiveScorecardWorkdayPattern = (scorecard.ScorecardWorkdayPatterns != null) ? scorecard.ScorecardWorkdayPatterns
                               .Where(x => x.ScorecardId == scorecard.Id &&
                               x.EffectiveStartDate <= currentDate && (x.EffectiveEndDate == null || x.EffectiveEndDate >= currentDate))
                               .Select(workdaypatterns => new ScorecardWorkdayPatternItem()
                               {
                                   Id = workdaypatterns.Id,
                                   ScorecardId = workdaypatterns.ScorecardId,
                                   IsSunday = workdaypatterns.IsSunday,
                                   IsMonday = workdaypatterns.IsMonday,
                                   IsTuesday = workdaypatterns.IsTuesday,
                                   IsWednesday = workdaypatterns.IsWednesday,
                                   IsThursday = workdaypatterns.IsThursday,
                                   IsFriday = workdaypatterns.IsFriday,
                                   IsSaturday = workdaypatterns.IsSaturday,
                                   EffectiveStartDate = workdaypatterns.EffectiveStartDate,
                                   EffectiveEndDate = workdaypatterns.EffectiveEndDate
                               }).FirstOrDefault() : null,

                ScorecardHolidayPattern = (scorecard.ScorecardHolidayPatterns != null) ?
                                scorecard.ScorecardHolidayPatterns.Where(x => x.ScorecardId == scorecard.Id && x.EffectiveEndDate == null)
                               .Select(holidaypatterns => new ScorecardHolidayPatternItem()
                               {
                                   Id = holidaypatterns.Id,
                                   ScorecardId = holidaypatterns.ScorecardId,
                                   HolidayPatternId = holidaypatterns.HolidayPatternId,
                                   Name = holidaypatterns.HolidayPattern.Name,
                                   EffectiveStartDate = holidaypatterns.EffectiveStartDate,
                                   EffectiveEndDate = holidaypatterns.EffectiveEndDate
                               }).FirstOrDefault() : null,

                ActiveScorecardHolidayPattern = (scorecard.ScorecardHolidayPatterns != null) ? scorecard.ScorecardHolidayPatterns
                               .Where(x => x.ScorecardId == scorecard.Id &&
                               x.EffectiveStartDate <= currentDate && (x.EffectiveEndDate == null || x.EffectiveEndDate >= currentDate))
                               .Select(holidaypatterns => new ScorecardHolidayPatternItem()
                               {
                                   Id = holidaypatterns.Id,
                                   ScorecardId = holidaypatterns.ScorecardId,
                                   HolidayPatternId = holidaypatterns.HolidayPatternId,
                                   Name = holidaypatterns.HolidayPattern.Name,
                                   EffectiveStartDate = holidaypatterns.EffectiveStartDate,
                                   EffectiveEndDate = holidaypatterns.EffectiveEndDate
                               }).FirstOrDefault() : null,
                NextWorkdayPatternStartDate = NextStartDate.Date,
                NextHolidayPatternStartDate = NextStartDate.Date,
                CurrentWorkdayPatternStartDate = CurrentMonthStart.Date,
                RootScorecardId = GetTopLevelScorecardOfHierarchyTree(scorecard.Id).Id

            };
        }

        /// <summary>
        /// Method to restrict the add/update of same kpi owner to multiple scorecard 
        /// based on the configuration settings
        /// </summary>
        /// <param name="kpiOwners">list of kpi owner</param>
        /// <param name="scorecardId">scorecard identifier</param>
        private void ValidateKPIOwners(IEnumerable<KPIOwnerItem> kpiOwners, int? scorecardId)
        {
            //configuration setting value 
            bool canHaveSameKPIOwnerforMultipleScorecards = Convert.ToBoolean(
                ConfigurationManager.AppSettings[AppSettingsKeys.
                        SameKPIOwnerforMultipleScorecards]);

            if (!canHaveSameKPIOwnerforMultipleScorecards)
            {
                //looping through kpi owner list
                foreach (var kpiOwner in kpiOwners)
                {
                    bool kpiOwnerExists = false;
                    //in case of update scorecard check whether the 
                    //user is already kpi owner of another scorecard
                    if (scorecardId.HasValue)
                    {
                        kpiOwnerExists = scorecardKPIOwnerRepository.GetAll().Any(x => x.IsActive
                            && x.User.AccountName == kpiOwner.AccountName
                            && x.ScorecardId != scorecardId.Value);
                    }
                    else
                    {
                        kpiOwnerExists = scorecardKPIOwnerRepository.GetAll()
                            .Any(x => x.IsActive && x.User.AccountName == kpiOwner.AccountName);
                    }

                    if (kpiOwnerExists)
                    {
                        var errorMessage = string.Format(Constants.ScorecardKPIOwnerAlreadyExists,
                            kpiOwner.FullName);
                        throw new NDMSBusinessException(errorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Method to restrict the add/update of same team member to multiple scorecard 
        /// based on the configuration settings
        /// </summary>
        /// <param name="teams">list of teams</param>
        /// <param name="scorecardId">scorecard identifier</param>
        private void ValidateTeams(IEnumerable<ScorecardTeamItem> teams, int? scorecardId)
        {
            //configuration setting value 
            bool canHaveSameTeamMemberforMultipleScorecards = Convert.ToBoolean(
                ConfigurationManager.AppSettings[AppSettingsKeys.
                    SameTeamMemberforMultipleScorecards]);

            if (!canHaveSameTeamMemberforMultipleScorecards)
            {
                //looping through team list
                foreach (var team in teams)
                {
                    bool teamExists = false;
                    //in case of update scorecard check whether the 
                    //user is already team member of another scorecard
                    if (scorecardId.HasValue)
                    {
                        teamExists = scorecardTeamRepository.GetAll().Any(x => x.IsActive
                            && x.User.AccountName == team.AccountName
                            && x.ScorecardId != scorecardId.Value);
                    }
                    else
                    {
                        teamExists = scorecardTeamRepository.GetAll()
                            .Any(x => x.IsActive && x.User.AccountName == team.AccountName);
                    }

                    if (teamExists)
                    {
                        var errorMessage = string.Format(Constants.ScorecardTeamAlreadyExists,
                            team.FullName);
                        throw new NDMSBusinessException(errorMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Method to validate scorecard while adding/updating scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard item object</param>
        private void ValidateScorecard(ScorecardItem scorecardRequest)
        {
            var errors = new List<string>();
            int maxKPIOwnerCount = Convert.ToInt32(ConfigurationManager.
                AppSettings[AppSettingsKeys.MaxKPIOwnerCount]);
            int maxTeamCount = Convert.ToInt32(ConfigurationManager.
                AppSettings[AppSettingsKeys.MaxTeamCount]);
            int maxKPICount = Convert.ToInt32(ConfigurationManager.
                AppSettings[AppSettingsKeys.MaxKPICount]);          

            if (!scorecardRequest.BusinessSegments.Any())
            {
                errors.Add(Constants.EmptyBusinessSegmentData);
            } 
           if(!scorecardRequest.Divisions.Any())
            {
                errors.Add(Constants.EmptyDivisionData);
            }
            if (!scorecardRequest.Facilities.Any())
            {
                errors.Add(Constants.EmptyFacilityData);
            }
            if (!scorecardRequest.ProductLines.Any())
            {
                errors.Add(Constants.EmptyProductLineData);
            }
            if (!scorecardRequest.Departments.Any())
            {
                errors.Add(Constants.EmptyDepartmentData);
            }
            if (!scorecardRequest.Processes.Any())
            {
                errors.Add(Constants.EmptyProcessData);
            }
            if (scorecardRequest.KPIOwners == null || scorecardRequest.KPIOwners.Count() == 0)
            {
                errors.Add(Constants.KPIOwnersEmptyErrorMessage);
            }
            else if (scorecardRequest.KPIOwners.Count() > maxKPIOwnerCount)
            {
                errors.Add(string.Format(Constants.KPIOwnerMaxCountErrorMessage, maxKPIOwnerCount));
            }
            else
            {
                ValidateKPIOwners(scorecardRequest.KPIOwners, scorecardRequest.Id);
            }

            if (scorecardRequest.Teams == null || scorecardRequest.Teams.Count() == 0)
            {
                errors.Add(Constants.ScorecardTeamsEmptyErrorMessage);
            }
            else if (scorecardRequest.Teams.Count() > maxTeamCount)
            {
                errors.Add(string.Format(Constants.ScorecardTeamMaxCountErrorMessage, maxTeamCount));
            }
            else
            {
                ValidateTeams(scorecardRequest.Teams, scorecardRequest.Id);
            }

            if (scorecardRequest.KPIs.Count() > maxKPICount)
            {
                errors.Add(string.Format(Constants.KPIMaxCountErrorMessage, maxKPICount));
            }
            if (scorecardRequest.Recordable != null && scorecardRequest.Recordable.RecordableDate != null)
            {
                DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                if (scorecardRequest.Recordable.RecordableDate.Value.Date > currentDate)
                {
                    errors.Add(Constants.LastRecordableDateErrorMessage);
                }
            }

            if (errors.Count > 0)
            {
                throw new NDMSBusinessException(errors);
            }
        }

        /// <summary>
        /// map scorecard children to DTO
        /// </summary>
        /// <param name="children">scorecard children</param>
        /// <returns>child scorecards</returns>
        private List<ScorecardNode> AddChildScorecardNodes(ICollection<Scorecard> children, bool isAdmin, MonthItem month, ScorecardNode exisitngNode = null)
        {
            var childScorecards = new List<ScorecardNode>();
            foreach (var child in children)
            {
                var nextLevelChildren = child.ChildScorecards.ToList();
                if (exisitngNode?.Id == child.Id)
                {
                    childScorecards.Add(exisitngNode);
                }
                else
                {
                    childScorecards.Add(new ScorecardNode()
                    {
                        Id = child.Id,
                        Name = child.Name,
                        SortOrder = child.SortOrder,
                        DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                        DrillDownLevel = child.DrilldownLevel,
                        CanAddScorecard = isAdmin,
                        CanEditScorecard = isAdmin,
                        CanManageTarget = isAdmin,
                        CanReOrder = child.ParentScorecard.ChildScorecards.Count() > 1 ? isAdmin : false,
                        CanViewScorecard = true,
                        IsActive = child.IsActive,
                        ScorecardStatus = GetScorecardStatus(child, month),
                        Children = AddChildScorecardNodes(nextLevelChildren, isAdmin, month, exisitngNode)
                    });
                }

            }
            return childScorecards;
        }

        /// <summary>
        /// Retrieves the hierarchy top down
        /// </summary>
        /// <returns>Scorecard Hierarchy</returns>
        private ScorecardNode GetScorecardHierarchy(int? rootScorecardId, bool isAdmin, MonthItem month, ScorecardNode existingNode)
        {
            if (rootScorecardId == null && existingNode != null)
            {
                rootScorecardId = GetTopLevelScorecardOfHierarchyTree(existingNode.Id).Id;
            }
            var scorecard = scorecardRepository.GetAll()
                                               .Where(s => s.ParentScorecardId == null &&
                                               (rootScorecardId.HasValue ? s.Id == rootScorecardId.Value : true))
                                               .OrderBy(x => x.Id).FirstOrDefault();

            if (scorecard != null)
            {
                // show active only or all children
                var childScorecards = scorecard.ChildScorecards.ToList();
                if (existingNode?.Id == scorecard.Id)
                {
                    return existingNode;

                }
                else
                {
                    var scorecardData = new ScorecardNode()
                    {
                        Id = scorecard.Id,
                        Name = scorecard.Name,
                        SortOrder = scorecard.SortOrder,
                        DrillDownLevel = scorecard.DrilldownLevel,
                        DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(scorecard),
                        CanAddScorecard = isAdmin,
                        CanEditScorecard = isAdmin,
                        CanManageTarget = isAdmin,
                        CanReOrder = isAdmin,
                        IsRootNode = isAdmin,
                        IsActive = scorecard.IsActive,
                        ExpandTillDrilldownLevel = isAdmin,
                        CanViewScorecard = true,
                        Children = AddChildScorecardNodes(childScorecards, isAdmin, month, existingNode),
                        ScorecardStatus = GetScorecardStatus(scorecard, month)
                    };

                    return scorecardData;
                }

            }
            return null;

        }

        /// <summary>
        /// Sets the selected scorecards parent as root node and selected scorecard as expanded node
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="rootNodeId"></param>
        /// <param name="expandedNodeId"></param>
        /// <returns>ScorecardNode</returns>
        private ScorecardNode SetRootAndExpandedNodes(ScorecardNode treeNode, bool isAdmin, int? rootNodeId, int? expandedNodeId, int? defaultScorecardId)
        {
            if (rootNodeId.HasValue && !expandedNodeId.HasValue)
            {
                treeNode.IsRootNode = rootNodeId.Value == treeNode.Id;
                treeNode.ExpandTillDrilldownLevel = rootNodeId.Value == treeNode.Id;
                SetDefaultRootAndExpandedNodeRecursively(treeNode.Children, rootNodeId);
            }
            else if (!expandedNodeId.HasValue && isAdmin)
            {
                // return tree
            }
            else if (expandedNodeId.HasValue || defaultScorecardId.HasValue)
            {
                var expandedId = expandedNodeId ?? defaultScorecardId;
                treeNode.IsRootNode = treeNode.Id == expandedId.Value || treeNode.Children.Any(x => x.Id == expandedId);
                treeNode.ExpandTillDrilldownLevel = expandedId.Value == treeNode.Id;
                SetDefaultRootAndExpandedNodeRecursively(treeNode.Children, expandedId);
            }

            return treeNode;

        }


        /// <summary>
        /// Recursively Sets the default scorecards parent as root node and default scorecard as expanded node
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="rootNodeId"></param>
        /// <param name="expandedNodeId"></param>
        /// <returns>ScorecardNode</returns>
        private void SetDefaultRootAndExpandedNodeRecursively(IEnumerable<ScorecardNode> nodeChildren, int? defaultScorecardId)
        {
            foreach (var child in nodeChildren)
            {
                child.IsRootNode = child.Children?.Any(x => x.Id == defaultScorecardId) ?? false;
                child.ExpandTillDrilldownLevel = child.Id == defaultScorecardId;
                SetDefaultRootAndExpandedNodeRecursively(child.Children, defaultScorecardId);
            }
        }
        /// <summary>
        /// Method to convert collection of Child Scorecards of KPI Owner Scorecard 
        /// to collection of ScorecardNode DTO
        /// </summary>
        /// <param name="childScorecards">Child Scorecard of KPI Owner Scorecard</param>
        /// <returns>collection of ScorecardNode DTO</returns>
        private ICollection<ScorecardNode> ConvertChildScorecardsOfKPIOwnerToScorecardNodeDTO(ICollection<Scorecard> childScorecards, MonthItem month)
        {
            var kpiOwnerChildren = new List<ScorecardNode>();
            foreach (var child in childScorecards)
            {
                var nextLevelChildren = child.ChildScorecards.ToList();
                var scorecardNode = new ScorecardNode()
                {
                    Id = child.Id,
                    Name = child.Name,
                    SortOrder = child.SortOrder,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                    DrillDownLevel = child.DrilldownLevel,
                    IsActive = child.IsActive,
                    CanViewScorecard = true,
                    CanEditScorecard = true,
                    CanManageTarget = true,
                    ScorecardStatus = GetScorecardStatus(child, month),
                    Children = AddChildScorecardNodes(nextLevelChildren, false, month)
                };

                kpiOwnerChildren.Add(scorecardNode);
            }
            return kpiOwnerChildren;
        }


        /// <summary>
        /// Converts the sibling score cards of kpi owner to scorecard node dto.
        /// </summary>
        /// <param name="siblingScoreCards">The sibling score cards.</param>
        /// <returns>collection of ScorecardNode DTO</returns>
        private ICollection<ScorecardNode> ConvertSiblingScorecardsOfKPIOwnerToScorecardNodeDTO(ICollection<Scorecard> siblingScoreCards, MonthItem month)
        {
            var kpiOwnerSiblings = new List<ScorecardNode>();
            foreach (var sibling in siblingScoreCards)
            {
                var nextLevelChildren = sibling.ChildScorecards.ToList();
                var scoreCardNode = new ScorecardNode()
                {
                    Id = sibling.Id,
                    Name = sibling.Name,
                    SortOrder = sibling.SortOrder,
                    IsActive = sibling.IsActive,
                    DrillDownLevel = sibling.DrilldownLevel,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(sibling),
                    CanViewScorecard = true,
                    ScorecardStatus = GetScorecardStatus(sibling, month),
                    Children = AddChildScorecardNodes(nextLevelChildren, false, month)
                };

                kpiOwnerSiblings.Add(scoreCardNode);
            }
            return kpiOwnerSiblings;
        }

        /// <summary>
        /// Gets the score hierarchy for kpi owner with single scorecard.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The scorecard hierarchy for KPI Owner</returns>
        private ScorecardNode GetScoreHierarchyForKPIOwnerWithSingleScorecard(User user, MonthItem month)
        {
            //get scorecard of kpi owner
            var kpiOwnerScorecard = user.KPIOwnerScorecards
                .FirstOrDefault(x => x.IsActive).Scorecard;
            // if KPI Owner has parent scorecard
            if (kpiOwnerScorecard.ParentScorecardId.HasValue)
            {
                //creating scorecard hierarchy
                var parentScorecardNode = new ScorecardNode()
                {
                    Id = kpiOwnerScorecard.ParentScorecard.Id,
                    Name = kpiOwnerScorecard.ParentScorecard.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(kpiOwnerScorecard.ParentScorecard),
                    CanViewScorecard = true,
                    IsRootNode = true,
                    IsActive = kpiOwnerScorecard.ParentScorecard.IsActive,
                    DrillDownLevel = kpiOwnerScorecard.ParentScorecard.DrilldownLevel,
                    SortOrder = kpiOwnerScorecard.ParentScorecard.SortOrder,
                    ScorecardStatus = GetScorecardStatus(kpiOwnerScorecard.ParentScorecard, month)
                };

                var childrenNodesOfParentScorecard = new List<ScorecardNode>();

                var childrenOfParentScorecard = kpiOwnerScorecard.ParentScorecard.ChildScorecards.ToList();// show all scorecards

                foreach (var child in childrenOfParentScorecard)
                {
                    var nextLevelChildren = child.ChildScorecards.ToList();

                    var childScorecardNode = new ScorecardNode()
                    {
                        Id = child.Id,
                        Name = child.Name,
                        DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                        CanViewScorecard = true,
                        SortOrder = child.SortOrder,
                        DrillDownLevel = child.DrilldownLevel,
                        IsActive = child.IsActive,
                        ScorecardStatus = GetScorecardStatus(child, month),
                        ExpandTillDrilldownLevel = kpiOwnerScorecard.Id == child.Id,
                        CanAddScorecard = kpiOwnerScorecard.Id == child.Id,
                        CanViewTargets = kpiOwnerScorecard.Id == child.Id,
                        Children = kpiOwnerScorecard.Id == child.Id ? ConvertChildScorecardsOfKPIOwnerToScorecardNodeDTO(nextLevelChildren, month)
                                                : ConvertSiblingScorecardsOfKPIOwnerToScorecardNodeDTO(nextLevelChildren, month)

                    };

                    childrenNodesOfParentScorecard.Add(childScorecardNode);
                }
                parentScorecardNode.Children = childrenNodesOfParentScorecard;
                return parentScorecardNode;
            }
            //else if the KPI Owner Scorecard is the top level scorecard 
            //return that scorecard and its child scorecards
            else
            {
                var nextLevelChildren = kpiOwnerScorecard.ChildScorecards.ToList();
                var scorecardNode = new ScorecardNode()
                {
                    Id = kpiOwnerScorecard.Id,
                    Name = kpiOwnerScorecard.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(kpiOwnerScorecard),
                    SortOrder = kpiOwnerScorecard.SortOrder,
                    CanViewScorecard = true,
                    CanAddScorecard = true,
                    CanViewTargets = true,
                    IsRootNode = true,
                    ExpandTillDrilldownLevel = true,
                    DrillDownLevel = kpiOwnerScorecard.DrilldownLevel,
                    IsActive = kpiOwnerScorecard.IsActive,
                    ScorecardStatus = GetScorecardStatus(kpiOwnerScorecard, month),
                    Children = ConvertChildScorecardsOfKPIOwnerToScorecardNodeDTO(nextLevelChildren, month)
                };
                return scorecardNode;
            }
        }

        /// <summary>
        /// Gets the scorecard hierarchy for user with multiple scorecards.
        /// KPI owner for multiple scorecards, Team member for multiple scorecards or 
        /// KPI owner for one scorecard and Team member for another
        /// </summary>
        /// <returns>The scorecard node</returns>
        private ScorecardNode GetScoreHierarchyForUserWithMultipleScorecard(User user, MonthItem month, int? defaultScorecardId)
        {
            var treeId = GetTopLevelScorecardOfHierarchyTree(defaultScorecardId.Value).Id;
            var parentScorecard = GetParentScorecardOfUserInTree(user, treeId);

            if (parentScorecard == null)
            {
                return null;
            }

            var kpiOwnerScorecardIds = new List<int>();
            var teamScorecardIds = new List<int>();

            if (user.KPIOwnerScorecards.Any(x => x.IsActive))
            {
                kpiOwnerScorecardIds = user.KPIOwnerScorecards
                .Where(x => x.IsActive).Select(x => x.Scorecard.Id).ToList();
            }

            if (user.TeamScorecards.Any(x => x.IsActive))
            {
                // get all team scorecards for the user
                teamScorecardIds = user.TeamScorecards
                .Where(x => x.IsActive).Select(x => x.Scorecard.Id).ToList();
            }

            bool isKpiOwner = kpiOwnerScorecardIds.Contains(parentScorecard.Id);
            bool isTeamMember = teamScorecardIds.Contains(parentScorecard.Id);

            var childScorecards = parentScorecard.ChildScorecards.ToList();

            var scorecardNode = new ScorecardNode()
            {
                Id = parentScorecard.Id,
                Name = parentScorecard.Name,
                DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(parentScorecard),
                CanViewScorecard = true,
                IsRootNode = true,
                SortOrder = parentScorecard.SortOrder,
                ExpandTillDrilldownLevel = true,
                DrillDownLevel = parentScorecard.DrilldownLevel,
                IsActive = parentScorecard.IsActive,
                CanAddScorecard = isKpiOwner,
                CanViewTargets = isKpiOwner,
                ScorecardStatus = GetScorecardStatus(parentScorecard, month),
                Children = AddChildScorecardNodesOfMultipleScorecardUser(childScorecards, kpiOwnerScorecardIds, teamScorecardIds, isKpiOwner, month)
            };
            return scorecardNode;
        }

        /// <summary>
        /// Gets the parent scorecard of user in the required tree
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The parent scorecard</returns>
        private Scorecard GetParentScorecardOfUserInTree(User user, int treeId)
        {
            var scorecards = new List<Scorecard>();
            var kpiOwnerScorecardsInTree = user.KPIOwnerScorecards.Where(x => x.IsActive && treeId == GetTopLevelScorecardOfHierarchyTree(x.Scorecard).Id);
            if (kpiOwnerScorecardsInTree.Any())
            {
                scorecards.AddRange(kpiOwnerScorecardsInTree.Select(x => x.Scorecard));
            }
            var teamMemeberScorecardsInTree = user.TeamScorecards.Where(x => x.IsActive && !scorecards.Any(y => y.Id == x.Scorecard.Id) &&
                                                                        treeId == GetTopLevelScorecardOfHierarchyTree(x.Scorecard).Id);
            if (teamMemeberScorecardsInTree.Any())
            {
                scorecards.AddRange(teamMemeberScorecardsInTree.Select(x => x.Scorecard));
            }

            if (!scorecards.Any())
            {
                return null;
            }

            if (scorecards.Any(x => !x.ParentScorecardId.HasValue))
            {
                return scorecards.Where(x => !x.ParentScorecardId.HasValue).FirstOrDefault();
            }

            // get the path of first node
            var firstNodeParentPath = GetParents(scorecards.FirstOrDefault());
            // remove first node as it is already processed
            scorecards.RemoveAt(0);
            return GetLowestCommonParentOfScorecard(scorecards, firstNodeParentPath);
        }

        /// <summary>
        /// Gets the lowest common parent of scorecard.
        /// </summary>
        /// <param name="scorecards">The scorecards.</param>
        /// <param name="firstNodeParentPath">The first node parent path.</param>
        /// <returns>The common parent</returns>
        private Scorecard GetLowestCommonParentOfScorecard(List<Scorecard> scorecards, IList<Scorecard> firstNodeParentPath)
        {
            foreach (var scorecard in scorecards)
            {
                var tempScorecard = scorecard;
                while (tempScorecard.ParentScorecardId.HasValue)
                {
                    if (firstNodeParentPath.Contains(tempScorecard.ParentScorecard))
                    {
                        // remove child nodes of common parent
                        firstNodeParentPath = firstNodeParentPath.Skip(firstNodeParentPath.IndexOf(tempScorecard.ParentScorecard)).ToList();
                        break;
                    }

                    tempScorecard = tempScorecard.ParentScorecard;
                }
            }

            return firstNodeParentPath.FirstOrDefault();
        }

        /// <summary>
        /// Gets the parents of given scorecard.
        /// </summary>
        /// <param name="scorecard">The scorecard.</param>
        /// <returns>The list of parent scorecards</returns>
        private IList<Scorecard> GetParents(Scorecard scorecard)
        {
            var parentPath = new List<Scorecard>();
            while (scorecard.ParentScorecardId.HasValue)
            {
                parentPath.Add(scorecard.ParentScorecard);
                scorecard = scorecard.ParentScorecard;
            }

            return parentPath;
        }

        /// <summary>
        /// Adds the child scorecard nodes of user with multiple scorecards for hierarchy page.
        /// </summary>
        /// <param name="childScorecards">The child scorecards.</param>
        /// <param name="kpiOwnerScorecardIds">The kpi owner scorecard ids.</param>
        /// <param name="teamScorecardIds">The team scorecard ids.</param>
        /// <param name="isChildOfKPIOwner">if set to <c>true</c> [is child of kpi owner].</param>
        /// <returns>The scorecard node</returns>
        private ICollection<ScorecardNode> AddChildScorecardNodesOfMultipleScorecardUser(ICollection<Scorecard> childScorecards,
            IList<int> kpiOwnerScorecardIds, IList<int> teamScorecardIds, bool isChildOfKPIOwner, MonthItem month)
        {
            var kpiOwnerChildren = new List<ScorecardNode>();
            bool isKpiOwner = false;
            bool isTeamMember = false;
            foreach (var child in childScorecards)
            {
                var nextLevelChildren = child.ChildScorecards.ToList();
                isKpiOwner = kpiOwnerScorecardIds.Contains(child.Id);
                isTeamMember = teamScorecardIds.Contains(child.Id);

                var scorecardNode = new ScorecardNode()
                {
                    Id = child.Id,
                    Name = child.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                    DrillDownLevel = child.DrilldownLevel,
                    SortOrder = child.SortOrder,
                    IsActive = child.IsActive,
                    CanViewScorecard = true,
                    CanAddScorecard = isKpiOwner,
                    CanEditScorecard = isChildOfKPIOwner,
                    CanManageTarget = isChildOfKPIOwner,
                    CanViewTargets = !isChildOfKPIOwner && isKpiOwner,
                    ScorecardStatus = GetScorecardStatus(child, month),
                    Children = AddChildScorecardNodesOfMultipleScorecardUser(child.ChildScorecards, kpiOwnerScorecardIds, teamScorecardIds, isKpiOwner, month)
                };

                kpiOwnerChildren.Add(scorecardNode);
            }
            return kpiOwnerChildren;
        }

        ///<summary>
        /// Gets all the root scorecard info for scorecard dropdown
        ///</summary>
        ///<returns> All root scorecards </returns>
        private IEnumerable<ScorecardMenuItem> GetTopLevelScorecardsForHierarchyDropdown()
        {
            var topLevelScorecards = scorecardRepository.GetAll()
                                .Where(x => x.ParentScorecardId == null).ToList();
            var topLevelMenuOptions = topLevelScorecards.Select(x => new ScorecardMenuItem
            {
                Id = x.Id,
                Name = x.Name,
                TopScorecardId = x.Id,
                ParentScorecardId = null,
                NamePrefix = GetNamePrefixForScorecardMenuOption(x)
            }).OrderBy(x => x.Name).ToList();

            return topLevelMenuOptions;
        }
        /// <summary>
        ///  return the root scorecard
        /// </summary>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        private Scorecard GetTopLevelScorecardOfHierarchyTree(Scorecard scorecard)
        {
            if (scorecard != null)
            {
                while (scorecard.ParentScorecardId != null)
                {
                    scorecard = scorecard.ParentScorecard;
                }
            }
            return scorecard;
        }

        /// <summary>
        ///  return the root scorecard
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <returns></returns>
        private Scorecard GetTopLevelScorecardOfHierarchyTree(int scorecardId)
        {
            var currentScorecard = scorecardRepository.GetAll().FirstOrDefault(x => x.Id == scorecardId);
            return GetTopLevelScorecardOfHierarchyTree(currentScorecard);
        }



        /// <summary>
        /// Convertor to convert KPI Owner scorecard to scorecard menu item
        /// </summary>
        /// <param name="kpiOwnerScorecards"></param>
        /// <returns></returns>
        private IEnumerable<ScorecardMenuItem> ConvertKPIScorecardToScorecardMenuItemDTO(ICollection<ScorecardKPIOwner> kpiOwnerScorecards)
        {
            List<ScorecardMenuItem> userScorecard = new List<ScorecardMenuItem>();
            foreach (ScorecardKPIOwner kpiOwnerScorecard in kpiOwnerScorecards)
            {
                ScorecardMenuItem scorecard = new ScorecardMenuItem();
                scorecard.Id = kpiOwnerScorecard.ScorecardId;
                scorecard.ParentScorecardId = kpiOwnerScorecard.Scorecard.ParentScorecardId;
                scorecard.TopScorecardId = GetTopLevelScorecardOfHierarchyTree(kpiOwnerScorecard.ScorecardId).Id;
                scorecard.Name = kpiOwnerScorecard.Scorecard.Name;
                scorecard.NamePrefix = GetNamePrefixForScorecardMenuOption(kpiOwnerScorecard.Scorecard);
                userScorecard.Add(scorecard);
            }

            return userScorecard;
        }

        /// <summary>
        /// Convert  team member scorecard to menu item DTO
        /// </summary>
        /// <param name="teamScorecards"></param>
        /// <returns></returns>
        private IEnumerable<ScorecardMenuItem> ConvertTeamScorecardToScorecardMenuItemDTO(ICollection<ScorecardTeam> teamScorecards)
        {
            List<ScorecardMenuItem> userScorecard = new List<ScorecardMenuItem>();
            foreach (ScorecardTeam teamScorecard in teamScorecards)
            {
                ScorecardMenuItem scorecard = new ScorecardMenuItem();
                scorecard.Id = teamScorecard.ScorecardId;
                scorecard.ParentScorecardId = teamScorecard.Scorecard.ParentScorecardId;
                scorecard.TopScorecardId = GetTopLevelScorecardOfHierarchyTree(teamScorecard.ScorecardId).Id;
                scorecard.Name = teamScorecard.Scorecard.Name;
                scorecard.NamePrefix = GetNamePrefixForScorecardMenuOption(teamScorecard.Scorecard);
                userScorecard.Add(scorecard);
            }

            return userScorecard;
        }
        /// <summary>
        /// return the prefix shown with socrecard name at the dropdown menu
        /// </summary>
        /// <param name="scorecard"></param>
        /// <returns></returns>
        private string GetNamePrefixForScorecardMenuOption(Scorecard scorecard)
        {
            // gets Facility Name if scorecard is assigned to only one facility and Not All Facilities, otherwise,
            // gets Division Name if scorecard is assigned to only one Division  and Not All Divisions, otherwise,
            // gets Business Segment Name if scorecard is assigned to only one Business Segment and Not All Business Segments, otherwise null

            var namePrefix = scorecard.Facilities.Count == 1 && scorecard.Facilities.FirstOrDefault()?.Id != 0 ? scorecard.Facilities.FirstOrDefault()?.Name
                           : scorecard.Divisions.Count == 1 && scorecard.Divisions.FirstOrDefault()?.Id != 0 ? scorecard.Divisions.FirstOrDefault()?.Name
                           : scorecard.BusinessSegments.Count == 1 && scorecard.BusinessSegments.FirstOrDefault()?.Id != 0 ? scorecard.BusinessSegments.FirstOrDefault()?.Name : null;
            return namePrefix;
        }

        /// <summary>
        /// Method to update scorecard recordable
        /// </summary>
        /// <param name="recordableItem"></param>
        /// <param name="scorecardId"></param>
        /// <param name="userName"></param>
        private void UpdateScorecardRecordables(RecordableItem recordableItem, int scorecardId, string userName)
        {
            // number of recordable entered via actual entry
            int autoTrackedRecordableCount = recordableRepository.GetAll()
                                             .Where(x => x.ScorecardId == scorecardId &&
                                             x.IsActive && !x.IsManual).Count();
            if (autoTrackedRecordableCount > 0)
            {
                // if recordable already tracked via actual it can not be edited from scorecard 
                return;
            }

            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;

            var scorecardRecordables = recordableRepository.GetAll()
                                       .Where(x => x.ScorecardId == scorecardId &&
                                       x.IsActive && x.IsManual).FirstOrDefault();

            if (scorecardRecordables == null && !recordableItem.RecordableDate.HasValue)
            {
                return;
            }

            if (scorecardRecordables == null && recordableItem.RecordableDate.HasValue)
            {
                // Add new recordable date
                scorecardRecordables = new Recordable
                {
                    ScorecardId = scorecardId,
                    RecordableDate = recordableItem.RecordableDate.Value,
                    IsManual = true,
                    IsActive = true,
                    CreatedBy = loggedInUserId,
                    LastModifiedBy = loggedInUserId,
                    CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
                };
            }
            else if (scorecardRecordables != null)
            {
                // update recordable date
                if (recordableItem.RecordableDate.HasValue)
                {
                    scorecardRecordables.RecordableDate = recordableItem.RecordableDate.Value;
                }
                else
                {
                    scorecardRecordables.IsActive = false;
                }

                scorecardRecordables.LastModifiedBy = loggedInUserId;
                scorecardRecordables.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
            }

            recordableRepository.AddOrUpdate(scorecardRecordables);
        }


        /// <summary>
        /// Gets the score hierarchy for Team Member with single scorecard.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The scorecard hierarchy for Team Member</returns>
        private ScorecardNode GetScoreHierarchyForTeamMemberWithSingleScorecard(User user, MonthItem month)
        {
            //get scorecard of Team Member
            var teamMemberScorecard = user.TeamScorecards
                .FirstOrDefault(x => x.IsActive).Scorecard;
            // if Team Member has parent scorecard
            if (teamMemberScorecard.ParentScorecardId.HasValue)
            {
                //creating scorecard hierarchy
                var parentScorecardNode = new ScorecardNode()
                {
                    Id = teamMemberScorecard.ParentScorecard.Id,
                    Name = teamMemberScorecard.ParentScorecard.Name,
                    DrillDownLevel = teamMemberScorecard.ParentScorecard.DrilldownLevel,
                    IsActive = teamMemberScorecard.IsActive,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(teamMemberScorecard.ParentScorecard),
                    CanViewScorecard = true,
                    IsRootNode = true,
                    SortOrder = teamMemberScorecard.ParentScorecard.SortOrder,
                    ScorecardStatus = GetScorecardStatus(teamMemberScorecard, month)
                };

                var childrenNodesOfParentScorecard = new List<ScorecardNode>();
                var childrenOfParentScorecard = teamMemberScorecard.ParentScorecard.ChildScorecards.ToList();

                foreach (var child in childrenOfParentScorecard)
                {
                    var nextLevelChildren = child.ChildScorecards.ToList();
                    var childScorecardNode = new ScorecardNode()
                    {
                        Id = child.Id,
                        Name = child.Name,
                        DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                        SortOrder = child.SortOrder,
                        CanViewScorecard = true,
                        ExpandTillDrilldownLevel = teamMemberScorecard.Id == child.Id,
                        DrillDownLevel = teamMemberScorecard.DrilldownLevel,
                        IsActive = teamMemberScorecard.IsActive,
                        ScorecardStatus = GetScorecardStatus(child, month),
                        Children = ConvertChildScorecardsOfTeamMemberToScorecardNodeDTO(nextLevelChildren, month)
                    };

                    //if the scorecard is the selected user's scorecard 
                    //add children of that scorecard
                    childrenNodesOfParentScorecard.Add(childScorecardNode);
                }
                parentScorecardNode.Children = childrenNodesOfParentScorecard;
                return parentScorecardNode;
            }
            else
            {//else if the Team Member Scorecard is the top level scorecard 
             //return that scorecard and its child scorecards
                var nextLevelChildren = teamMemberScorecard.ChildScorecards.ToList();
                var scorecardNode = new ScorecardNode()
                {
                    Id = teamMemberScorecard.Id,
                    Name = teamMemberScorecard.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(teamMemberScorecard),
                    IsRootNode = true,
                    ExpandTillDrilldownLevel = true,
                    CanViewScorecard = true,
                    SortOrder = teamMemberScorecard.SortOrder,
                    DrillDownLevel = teamMemberScorecard.DrilldownLevel,
                    IsActive = teamMemberScorecard.IsActive,
                    Children = ConvertChildScorecardsOfTeamMemberToScorecardNodeDTO(nextLevelChildren, month)
                };

                return scorecardNode;
            }
        }

        /// <summary>
        /// Method to convert collection of Child Scorecards of Team Member Scorecard 
        /// to collection of ScorecardNode DTO
        /// </summary>
        /// <param name="childScorecards">Child Scorecard of Team Member Scorecard</param>
        /// <returns>collection of ScorecardNode DTO</returns>
        private ICollection<ScorecardNode> ConvertChildScorecardsOfTeamMemberToScorecardNodeDTO(ICollection<Scorecard> childScorecards, MonthItem month)
        {
            var TeamMemberChildren = new List<ScorecardNode>();
            foreach (var child in childScorecards)
            {
                var nextLevelChildren = child.ChildScorecards.ToList();
                var scorecardNode = new ScorecardNode()
                {
                    Id = child.Id,
                    Name = child.Name,
                    DaysWithoutRecordables = recordablesCalculator.GetNumberOfDaysWithOutRecordables(child),
                    SortOrder = child.SortOrder,
                    IsActive = child.IsActive,
                    CanViewScorecard = true,
                    DrillDownLevel = child.DrilldownLevel,
                    ScorecardStatus = GetScorecardStatus(child, month),
                    Children = AddChildScorecardNodes(nextLevelChildren, false, month)
                };

                TeamMemberChildren.Add(scorecardNode);
            }
            return TeamMemberChildren;
        }


        /// <summary>
        /// Method to get drill down status based on kpi metrics of given scorecard 
        /// </summary>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <param name="month">month</param>
        /// <param name="yearId">Year Id</param>
        /// <returns>scorecard status</returns>
        private ScorecardStatus GetScorecardStatus(Scorecard scorecard, MonthItem monthItem)
        {
            int month = monthItem.Id,
                year = monthItem.Year,
                yearId = monthItem.YearId;
            ScorecardStatus scorecardStatus = ScorecardStatus.NotApplicable;

            //form a date using the given month and the corresponding year
            var requestedDate = new DateTime(year, month, 1);
            // get the metrics which are active in the selected month by
            //comparing target start date (set day as 1) and target end date (set day as last day of the month)
            var scorecardKPIMetrics = targetRepository.GetAll().Where(x =>
            x.ScorecardId == scorecard.Id && x.CalendarYearId == yearId && x.IsActive).ToList();
            var scorecardMetricList = scorecardKPIMetrics.Where(x =>
                        requestedDate >= new DateTime(x.EffectiveStartDate.Year, x.EffectiveStartDate.Month, 1)
                        && requestedDate <= new DateTime(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month,
                        DateTime.DaysInMonth(x.EffectiveEndDate.Year, x.EffectiveEndDate.Month)));

            if (scorecard.IsActive)
            {
                if (scorecardMetricList.Any())
                {
                    var primaryMetricList = scorecardMetricList.Where(x => x.MetricType == MetricType.Primary);
                    var secondaryMetricList = scorecardMetricList.Where(x => x.MetricType == MetricType.Secondary);
                    if (primaryMetricList != null && primaryMetricList.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.NotAchieved))
                    {
                        scorecardStatus = ScorecardStatus.PrimaryNotAchieved;
                        return scorecardStatus;
                    }
                    if (secondaryMetricList != null && secondaryMetricList.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.NotAchieved))
                    {
                        scorecardStatus = ScorecardStatus.SecondaryNotAchieved;
                        return scorecardStatus;
                    }
                    if (secondaryMetricList.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.Achieved))
                    {
                        scorecardStatus = ScorecardStatus.Achieved;
                    }
                    if (primaryMetricList.Any(x => mtdCalculator.GetMetricMTDStatus(x, year, month) == ActualStatus.Achieved))
                    {
                        scorecardStatus = ScorecardStatus.Achieved;
                    }
                }
            }
            else
            {
                scorecardStatus = ScorecardStatus.Inactive;
            }
            return scorecardStatus;
        }

        /// <summary>
        /// Get Scorecard status from actual status
        /// </summary>
        /// <param name="status">Input actual status</param>
        /// <returns>Scorecard status as output</returns>
        private ScorecardStatus GetScorecardStatusFromActualStatus(ActualStatus status)
        {
            ScorecardStatus scorecardStatus = ScorecardStatus.NotApplicable;
            if (status == ActualStatus.NotAchieved)
            {
                scorecardStatus = ScorecardStatus.PrimaryNotAchieved;
            }
            else if (status == ActualStatus.Achieved)
            {
                scorecardStatus = ScorecardStatus.Achieved;
            }
            return scorecardStatus;
        }

        /// <summary>
        /// Gets all scorecards where the user is a KpiOwner
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IEnumerable<ScorecardMenuItem> GetKpiOwnedScorecardsOfUser(User user)
        {
            var kpiOwnerScorecards = new List<ScorecardMenuItem>();

            if (user?.KPIOwnerScorecards.Count(x => x.IsActive) > 0)
            {
                kpiOwnerScorecards.AddRange(ConvertKPIScorecardToScorecardMenuItemDTO(user.KPIOwnerScorecards.Where(x => x.IsActive).ToList()));
            }
            return kpiOwnerScorecards;

        }
        /// <summary>
        /// Gets all scorecards where the user is a team memeber and not a KpiOwner
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IEnumerable<ScorecardMenuItem> GetTeamMemberScorecardsOfUser(User user)
        {
            var teamMemeberScorecards = new List<ScorecardMenuItem>();

            if (user.TeamScorecards.Count(x => x.IsActive) > 0)
            {
                var teamScorecards = user.KPIOwnerScorecards.Any(x => x.IsActive) ?
                   user.TeamScorecards.Where(x => x.IsActive && !user.KPIOwnerScorecards.Any(y => y.IsActive && y.ScorecardId == x.ScorecardId)).ToList()
                    : user.TeamScorecards.Where(x => x.IsActive).ToList();

                teamMemeberScorecards.AddRange(ConvertTeamScorecardToScorecardMenuItemDTO(teamScorecards));
            }

            return teamMemeberScorecards;

        }
        #endregion

        #region Protected Method(s)

        /// <summary>
        /// Creates an instance of RecordablesCalculator and returns
        /// </summary>
        /// <returns></returns>
        protected virtual ScorecardRecordablesCalculator CreateRecordablesCalculator(IBaseRepository<Target> targetRepository,
            IBaseRepository<Recordable> recordableRepository, IBaseRepository<User> userRepository)
        {
            if (recordablesCalculator == null)
            {
                recordablesCalculator = new ScorecardRecordablesCalculator(targetRepository, recordableRepository, userRepository);
            }
            return recordablesCalculator;
        }

        /// <summary>
        /// Creates an instance of MTDCalculator and returns
        /// </summary>
        /// <returns>The MTDCalculator</returns>
        protected virtual MTDPerformanceCalculator CreateMTDCalculator(
            IBaseRepository<Target> targetRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository)
        {
            if (mtdCalculator == null)
            {
                mtdCalculator = new MTDPerformanceCalculator(targetRepository, dailyActualRepository, monthlyActualRepository,
                    scorecardHolidayPatternRepository, scorecardWorkdayTrackerRepository, scorecardWorkdayPatternRepository);
            }
            return mtdCalculator;
        }
        /// <summary>
        /// Creates an instance of user Repository and returns
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="targetRepository"></param>
        /// <param name="yearRepository"></param>
        /// <param name="adUserRepository"></param>
        /// <returns></returns>
        protected virtual UserManager CreateUserManager(
            INDMSUserRepository userRepository,
            IBaseRepository<Target> targetRepository,
            IBaseRepository<Year> yearRepository,
            IADUserRepository adUserRepository)
        {
            if (userManager == null)
            {
                userManager = new UserManager(userRepository, targetRepository, yearRepository, adUserRepository);
            }
            return userManager;
        }

        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="kpiRepository">KPI Repository</param>
        /// <param name="scorecardRepository">Scorecard Repository</param>
        public ScorecardAdminManager(IBaseRepository<KPI> kpiRepository,
            IScorecardRepository scorecardRepository,
            IBaseRepository<ScorecardKPIOwner> scorecardKPIOwnerRepository,
            IBaseRepository<ScorecardTeam> scorecardTeamRepository,
            IBaseRepository<Recordable> recordableRepository,
            IBaseRepository<Target> targetRepository,
            IBaseRepository<ScorecardWorkdayPattern> scorecardWorkdayPatternRepository,
            IBaseRepository<ScorecardHolidayPattern> scorecardHolidayPatternRepository,
            INDMSUserRepository userRepository,
            IADUserRepository adUserRepository,
            IBaseRepository<Year> yearRepository,
            IBaseRepository<DailyActual> dailyActualRepository,
            IBaseRepository<MonthlyActual> monthlyActualRepository,
            IBaseRepository<ScorecardWorkdayTracker> scorecardWorkdayTrackerRepository,
            IBaseRepository<BusinessSegment> businessSegmentRepository,
            IBaseRepository<Division> divisionRepository,
            IBaseRepository<Facility> facilityRepository,
            IBaseRepository<ProductLine> productLineRepository,
            IBaseRepository<Department> departmentRepository,
            IBaseRepository<Process> processRepository)
        {
            if (kpiRepository == null || scorecardRepository == null
                || scorecardKPIOwnerRepository == null || scorecardTeamRepository == null
                || recordableRepository == null || targetRepository == null || adUserRepository == null
                || userRepository == null || scorecardWorkdayPatternRepository == null || scorecardHolidayPatternRepository == null
                || yearRepository == null || dailyActualRepository == null || monthlyActualRepository == null || scorecardWorkdayTrackerRepository == null
                || businessSegmentRepository == null || divisionRepository == null || facilityRepository == null || productLineRepository == null
                || departmentRepository == null || processRepository == null)
            {
                throw new ArgumentNullException("Repository", "The given parameter cannot be null.");
            }
            this.kpiRepository = kpiRepository;
            this.scorecardRepository = scorecardRepository;
            this.scorecardKPIOwnerRepository = scorecardKPIOwnerRepository;
            this.scorecardTeamRepository = scorecardTeamRepository;
            this.recordableRepository = recordableRepository;
            this.targetRepository = targetRepository;
            this.userRepository = userRepository;
            this.recordablesCalculator = CreateRecordablesCalculator(targetRepository, recordableRepository, userRepository);
            this.scorecardWorkdayPatternRepository = scorecardWorkdayPatternRepository;
            this.scorecardHolidayPatternRepository = scorecardHolidayPatternRepository;
            this.yearRepository = yearRepository;
            this.dailyActualRepository = dailyActualRepository;
            this.monthlyActualRepository = monthlyActualRepository;
            this.scorecardWorkdayTrackerRepository = scorecardWorkdayTrackerRepository;
            this.businessSegmentRepository = businessSegmentRepository;
            this.divisionRepository = divisionRepository;
            this.facilityRepository = facilityRepository;
            this.productLineRepository = productLineRepository;
            this.departmentRepository = departmentRepository;
            this.processRepository = processRepository;
            this.mtdCalculator = CreateMTDCalculator(targetRepository, dailyActualRepository, monthlyActualRepository, scorecardWorkdayPatternRepository, scorecardHolidayPatternRepository
               , scorecardWorkdayTrackerRepository);
            this.userManager = CreateUserManager(userRepository, targetRepository, yearRepository, adUserRepository);

        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves Scorecard by id
        /// </summary>
        /// <param name="id">ID of score card</param>
        /// <returns>Scorecard with the ID</returns>
        public ScorecardItem GetScorecard(int id)
        {
            var scorecard = scorecardRepository.Get(id);
            if (scorecard != null)
            {
                return ConvertScorecardToScorecardItemDTO(scorecard);
            }
            return null;
        }

        /// <summary>
        /// Add scorecard to database
        /// </summary>
        /// <param name="scorecardRequest">scorecard details object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>added scorecard id</returns>
        public int AddScorecard(ScorecardItem scorecardRequest, string userName)
        {
            int parentScorecardChildCount = 0;
            ValidateScorecard(scorecardRequest);
            int loggedInUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            List<KPI> kpiList = new List<KPI>();
            foreach (var kpi in scorecardRequest.KPIs)
            {
                kpiList.Add(kpiRepository.Get(kpi.Id));
            }

            // Get the list of added kpi owners and team members
            var scorecardKPIOwners = userRepository.AddADUsersToNDMS(
                scorecardRequest.KPIOwners.Select(x => x.AccountName));
            var scorecardTeams = userRepository.AddADUsersToNDMS(
                scorecardRequest.Teams.Select(x => x.AccountName));

            if (scorecardRequest.ParentScorecardId.HasValue)
            {
                parentScorecardChildCount = scorecardRepository.Get(scorecardRequest.ParentScorecardId.Value)?.ChildScorecards?.Count ?? 0;
            }
            List<BusinessSegment> businessSegmentList = new List<BusinessSegment>();
            if(scorecardRequest.BusinessSegments.Any(x => x.Id == 0))
            {
                businessSegmentList.Add(businessSegmentRepository.Get(0));
            }
            else
            {
                foreach (var businessSegment in scorecardRequest.BusinessSegments)
                {
                    businessSegmentList.Add(businessSegmentRepository.Get(businessSegment.Id.Value));
                }
            }
            List<Division> divisiontList = new List<Division>();
            if (scorecardRequest.Divisions.Any(x => x.Id == 0))
            {
                divisiontList.Add(divisionRepository.Get(0));
            }
            else
            {
                foreach (var division in scorecardRequest.Divisions)
                {
                    divisiontList.Add(divisionRepository.Get(division.Id.Value));
                }
            }
            List<Facility> facilitytList = new List<Facility>();
            if (scorecardRequest.Facilities.Any(x => x.Id == 0))
            {
                facilitytList.Add(facilityRepository.Get(0));
            }
            else
            {
                foreach (var facility in scorecardRequest.Facilities)
                {
                    facilitytList.Add(facilityRepository.Get(facility.Id.Value));
                }
            }
            List<ProductLine> productLineList = new List<ProductLine>();

            if (scorecardRequest.ProductLines.Any(x => x.Id == 0))
            {
                productLineList.Add(productLineRepository.Get(0));
            }
            else
            {
                foreach (var productLine in scorecardRequest.ProductLines)
                {
                    productLineList.Add(productLineRepository.Get(productLine.Id.Value));
                }
            }
            List<Department> departmentList = new List<Department>();
            if (scorecardRequest.Departments.Any(x => x.Id == 0))
            {
                departmentList.Add(departmentRepository.Get(0));
            }
            else
            {
                foreach (var department in scorecardRequest.Departments)
                {
                    departmentList.Add(departmentRepository.Get(department.Id.Value));
                }
            }
            List<Process> processtList = new List<Process>();
            if (scorecardRequest.Processes.Any(x => x.Id == 0))
            {
                processtList.Add(processRepository.Get(0));
            }
            else
            {
                foreach (var process in scorecardRequest.Processes)
                {
                    processtList.Add(processRepository.Get(process.Id.Value));
                }
            }
            var scorecard = new Scorecard
            {
                Name = scorecardRequest.Name,
                ParentScorecardId = scorecardRequest.ParentScorecardId,
                IsBowlingChartApplicable = scorecardRequest.IsBowlingChartApplicable,
                DrilldownLevel = scorecardRequest.DrilldownLevel,
                SortOrder = parentScorecardChildCount + 1,
                KPIOwners = scorecardKPIOwners.Select(userId => new ScorecardKPIOwner()
                {
                    UserId = userId,
                    AssignedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    IsActive = true
                }).ToList(),
                Teams = scorecardTeams.Select(userId => new ScorecardTeam()
                {
                    UserId = userId,
                    AssignedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    IsActive = true
                }).ToList(),
                CreatedBy = loggedInUserId,
                LastModifiedBy = loggedInUserId,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                IsActive = true,
                KPIs = kpiList,
                BusinessSegments = businessSegmentList,
                Divisions = divisiontList,
                Facilities = facilitytList,
                ProductLines = productLineList,
                Departments = departmentList,
                Processes = processtList
            };

            if (scorecardRequest.Recordable != null && scorecardRequest.Recordable.RecordableDate != null)
            {
                scorecard.Recordables = new List<Recordable> {
                                            new Recordable
                                            {
                                                RecordableDate = scorecardRequest.Recordable.RecordableDate.Value,
                                                IsManual = true,
                                                IsActive = true,
                                                CreatedBy = loggedInUserId,
                                                LastModifiedBy = loggedInUserId,
                                                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                                                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp()
                                            }
                                        };
            }

            scorecardRepository.AddOrUpdate(scorecard);
            scorecardRepository.Save();
            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            var EffectiveStartDateValue = new DateTime(currentDate.Year, currentDate.Month, 1);

            if (scorecardRequest.ScorecardWorkdayPattern.IsSunday || scorecardRequest.ScorecardWorkdayPattern.IsMonday
              || scorecardRequest.ScorecardWorkdayPattern.IsTuesday || scorecardRequest.ScorecardWorkdayPattern.IsWednesday
              || scorecardRequest.ScorecardWorkdayPattern.IsThursday || scorecardRequest.ScorecardWorkdayPattern.IsFriday
              || scorecardRequest.ScorecardWorkdayPattern.IsSaturday)
            {
                var scorecardWorkdayPattern = new ScorecardWorkdayPattern
                {
                    ScorecardId = scorecard.Id,
                    IsSunday = scorecardRequest.ScorecardWorkdayPattern.IsSunday,
                    IsMonday = scorecardRequest.ScorecardWorkdayPattern.IsMonday,
                    IsTuesday = scorecardRequest.ScorecardWorkdayPattern.IsTuesday,
                    IsWednesday = scorecardRequest.ScorecardWorkdayPattern.IsWednesday,
                    IsThursday = scorecardRequest.ScorecardWorkdayPattern.IsThursday,
                    IsFriday = scorecardRequest.ScorecardWorkdayPattern.IsFriday,
                    IsSaturday = scorecardRequest.ScorecardWorkdayPattern.IsSaturday,
                    EffectiveStartDate = EffectiveStartDateValue.Date,
                    CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    CreatedBy = loggedInUserId,
                    LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                    LastModifiedBy = loggedInUserId

                };
                scorecardWorkdayPatternRepository.AddOrUpdate(scorecardWorkdayPattern);
                scorecardWorkdayPatternRepository.Save();
            }
            var scorecardHolidayPattern = new ScorecardHolidayPattern
            {
                ScorecardId = scorecard.Id,
                HolidayPatternId = scorecardRequest.ScorecardHolidayPattern.HolidayPatternId,
                EffectiveStartDate = EffectiveStartDateValue.Date,
                CreatedOn = TimeZoneUtility.GetCurrentTimestamp(),
                CreatedBy = loggedInUserId,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedBy = loggedInUserId

            };
            scorecardHolidayPatternRepository.AddOrUpdate(scorecardHolidayPattern);
            scorecardHolidayPatternRepository.Save();
            return scorecard.Id;
        }

        /// <summary>
        /// Update an existing scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard details object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>flag which says whether update is successful or not</returns>
        public bool UpdateScorecard(ScorecardItem scorecardRequest, string userName)
        {
            ValidateScorecard(scorecardRequest);
            //get the list of updated kpi owners and team members
            var updatedKPIOwnerIds = userRepository.AddADUsersToNDMS(scorecardRequest.
                KPIOwners.Select(x => x.AccountName));
            var updatedTeamIds = userRepository.AddADUsersToNDMS(scorecardRequest.
                Teams.Select(x => x.AccountName));

            if (scorecardRequest.Recordable != null)
            {
                UpdateScorecardRecordables(scorecardRequest.Recordable, scorecardRequest.Id.Value, userName);
            }

            return scorecardRepository.UpdateScorecard(scorecardRequest, updatedKPIOwnerIds,
                    updatedTeamIds, userName);
        }

        /// <summary>
        /// Swap Scorecard Sort Orders 
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <param name="nextScorecardId"></param>
        /// <returns></returns>
        public bool SwapScorecardSortOrders(SwapScorecardSortOrderRequest swapRequest, string userName)
        {
            int loggedinUserId = userRepository.GetAll().FirstOrDefault(
               x => x.AccountName == userName).Id;
            var scorecardFrom = new Scorecard()
            {
                Id = swapRequest.SwapFrom.Id,
                SortOrder = swapRequest.SwapTo.SortOrder,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedBy = loggedinUserId
            };

            var scorecardTo = new Scorecard()
            {
                Id = swapRequest.SwapTo.Id,
                SortOrder = swapRequest.SwapFrom.SortOrder,
                LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp(),
                LastModifiedBy = loggedinUserId
            };

            scorecardRepository.Update(scorecardFrom, e => e.SortOrder, e => e.LastModifiedOn, e => e.LastModifiedBy);
            scorecardRepository.Update(scorecardTo, e => e.SortOrder, e => e.LastModifiedOn, e => e.LastModifiedBy);
            scorecardRepository.Save();
            return true;
        }

        /// <summary>
        /// Retrieve Scorecards in a hierarchy mode based on logged in user. 
        /// </summary>
        /// <param name="userName">Logged in User</param>
        /// <returns>Scorecards in a hierarchy mode</returns>
        public ScorecardNode GetScorecardHierarchy(string userName, int? rootScorecardId, int? selectedScorecardId)
        {
            var user = userRepository.GetAll().Where(x => x.AccountName == userName && x.IsActive)
                .FirstOrDefault();

            var hasMultipleScorecards = userManager.IsUserAssignedToMultipleScorecards(user);
            int? defaultScorecardId = selectedScorecardId ?? userManager.GetDefaultScorecardId(user);

            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp();
            var month = new MonthItem()
            {
                Id = currentDate.Month,
                Year = currentDate.Year,
                YearId = yearRepository.GetAll().FirstOrDefault(x => x.Name == currentDate.Year.ToString()).Id
            };

            if (user != null)
            {
                ScorecardNode computedRootNode = null;

                //if the user is assigned to multiple scorecard return all scorecards assigned, the common parent and siblings
                if (!user.IsAdmin && hasMultipleScorecards)
                {

                    computedRootNode = GetScoreHierarchyForUserWithMultipleScorecard(user, month, defaultScorecardId);

                }
                //if the user is kpi owner of a single scorecard return his scorecard, its parent, children and siblings
                else if (!user.IsAdmin && user.KPIOwnerScorecards.Any(x => x.IsActive))
                {
                    computedRootNode = GetScoreHierarchyForKPIOwnerWithSingleScorecard(user, month);
                }
                else if (!user.IsAdmin && user.TeamScorecards.Any(x => x.IsActive))
                {
                    computedRootNode = GetScoreHierarchyForTeamMemberWithSingleScorecard(user, month);
                }
                if (rootScorecardId.HasValue && computedRootNode != null)
                {
                    if (rootScorecardId.Value != GetTopLevelScorecardOfHierarchyTree(computedRootNode.Id).Id)
                    {
                        computedRootNode = null;
                    }
                }

                //return all scorecards in a hierarchy node
                var scorecardNode = GetScorecardHierarchy(rootScorecardId, user.IsAdmin, month, computedRootNode);
                return SetRootAndExpandedNodes(scorecardNode, user.IsAdmin, rootScorecardId, selectedScorecardId, defaultScorecardId);

            }
            return null;
        }


        /// <summary>
        /// Gets all the Hierarchy dropdown option list
        /// </summary>
        /// <returns></returns>
        public HierarchyMenuDTO GetHierarchyDropdownList(string userName)
        {
            var user = userRepository.GetAll().Where(x => x.AccountName == userName && x.IsActive)
                .FirstOrDefault();
            var hierarchyMenu = new HierarchyMenuDTO()
            {
                KpiOwnerScorecards = GetKpiOwnedScorecardsOfUser(user),
                TeamMemberScorecards = GetTeamMemberScorecardsOfUser(user),
                RootScorecards = GetTopLevelScorecardsForHierarchyDropdown()
            };
            return hierarchyMenu;
        }

        /// <summary>
        /// Get Root scorecard of current hierarchy tree
        /// </summary>
        /// <param name="parentScorecardId"></param>
        /// <returns></returns>
        public int? GetRootScorecardOfTree(int? parentScorecardId)
        {
            int? rootScorecardId = null;
            if (parentScorecardId.HasValue)
            {
                rootScorecardId = GetTopLevelScorecardOfHierarchyTree(parentScorecardId.Value).Id;
            }
            return rootScorecardId;
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
                    if (scorecardRepository != null)
                    {
                        scorecardRepository.Dispose();
                    }
                    if (scorecardTeamRepository != null)
                    {
                        scorecardTeamRepository.Dispose();
                    }
                    if (scorecardKPIOwnerRepository != null)
                    {
                        scorecardKPIOwnerRepository.Dispose();
                    }
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    if (kpiRepository != null)
                    {
                        kpiRepository.Dispose();
                    }
                    if (scorecardWorkdayPatternRepository != null)
                    {
                        scorecardWorkdayPatternRepository.Dispose();
                    }
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    if (dailyActualRepository != null)
                    {
                        dailyActualRepository.Dispose();
                    }
                    if (monthlyActualRepository != null)
                    {
                        monthlyActualRepository.Dispose();
                    }
                    if (scorecardWorkdayTrackerRepository != null)
                    {
                        scorecardWorkdayTrackerRepository.Dispose();
                    }
                    if (businessSegmentRepository != null)
                    {
                        businessSegmentRepository.Dispose();
                    }
                    if (divisionRepository != null)
                    {
                        divisionRepository.Dispose();
                    }
                    if (facilityRepository != null)
                    {
                        facilityRepository.Dispose();
                    }
                    if (productLineRepository != null)
                    {
                        productLineRepository.Dispose();
                    }
                    if (departmentRepository != null)
                    {
                        departmentRepository.Dispose();
                    }
                    if (processRepository != null)
                    {
                        processRepository.Dispose();
                    }
                    // Assign references to null
                    scorecardRepository = null;
                    scorecardTeamRepository = null;
                    scorecardKPIOwnerRepository = null;
                    userRepository = null;
                    kpiRepository = null;
                    scorecardWorkdayPatternRepository = null;
                    yearRepository = null;
                    dailyActualRepository = null;
                    monthlyActualRepository = null;
                    scorecardWorkdayTrackerRepository = null;
                    businessSegmentRepository = null;
                    divisionRepository = null;
                    facilityRepository = null;
                    productLineRepository = null;
                    departmentRepository = null;
                    processRepository = null;
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
