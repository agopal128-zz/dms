using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NDMS.DataAccess.Repositories
{
    /// <summary>
    /// Repository to store and retrieve Scorecard entity
    /// </summary>
    public class ScorecardRepository : BaseRepository<Scorecard>, IScorecardRepository
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScorecardRepository() : base(new NDMSDataContext())
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="context">DBContext associated</param>
        public ScorecardRepository(NDMSDataContext context) : base(context)
        {
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Update an existing scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard details object</param>
        /// <param name="updatedKPIOwnerIds">Updated KPI Owner Id's</param>
        /// <param name="updatedTeamIds">Updated Team Member Id's</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>flag which says whether update is successful or not</returns>
        public bool UpdateScorecard(ScorecardItem scorecardRequest, List<int> updatedKPIOwnerIds,
             List<int> updatedTeamIds, string userName)
        {
            int loggedInUserId = Context.Users.FirstOrDefault(
              x => x.AccountName == userName).Id;
            var existingScorecard = Context.Scorecards
                       .Include(x => x.KPIOwners)
                       .Include(x => x.Teams)
                       .Include(x => x.KPIs)
                       .Include(x => x.ScorecardWorkdayPatterns)
                       .Single(c => c.Id == scorecardRequest.Id);
            if (existingScorecard == null)
            {
                return false;
            }

            existingScorecard.Name = scorecardRequest.Name;
            existingScorecard.IsActive = scorecardRequest.IsActive;
            existingScorecard.IsBowlingChartApplicable = scorecardRequest.IsBowlingChartApplicable;
            existingScorecard.DrilldownLevel = scorecardRequest.DrilldownLevel;
           // existingScorecard.ProductLineId = scorecardRequest.ProductLine.Id.Value;
           // existingScorecard.DepartmentId = scorecardRequest.Department.Id.Value;
            //existingScorecard.ProcessId = scorecardRequest.Process.Id.Value;
            existingScorecard.LastModifiedBy = loggedInUserId;
            existingScorecard.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();

            // Fetching existing scorecard teams and Kpi Owners from Context directly
            // This is to update deactivated on and IsActive flag
            var existingKPIOwners = existingScorecard.KPIOwners
                .Where(owner => owner.ScorecardId == scorecardRequest.Id && owner.IsActive);
            var existingTeams = existingScorecard.Teams
                .Where(team => team.ScorecardId == scorecardRequest.Id && team.IsActive);

            foreach (var owner in existingKPIOwners)
            {
                // Deactivating the kpi owners which are not present in update list
                if (!updatedKPIOwnerIds.Any(id => id == owner.UserId))
                {
                    owner.DeactivatedOn = TimeZoneUtility.GetCurrentTimestamp();
                    owner.IsActive = false;
                }
            }
            foreach (var team in existingTeams)
            {
                // Deactivating the teams which are not present in update list
                if (!updatedTeamIds.Any(id => id == team.UserId))
                {
                    team.DeactivatedOn = TimeZoneUtility.GetCurrentTimestamp();
                    team.IsActive = false;
                }
            }

            // Find the  newly added KPI owners for this score card
            var newKPIOwnerIDs = updatedKPIOwnerIds.Except(existingKPIOwners.
                Select(x => x.UserId)).ToList();

            foreach (var kpiOwnerID in newKPIOwnerIDs)
            {
                // If the new KPI owner were part of this scorecard long back, we just need to update entries
                var kpiOwner = existingScorecard.KPIOwners.Where(x => x.ScorecardId == scorecardRequest.Id
                && x.UserId == kpiOwnerID).FirstOrDefault();
                if (kpiOwner != null)
                {
                    kpiOwner.IsActive = true;
                    kpiOwner.DeactivatedOn = null;
                    kpiOwner.AssignedOn = TimeZoneUtility.GetCurrentTimestamp();
                }
                else
                {
                    if (existingScorecard.KPIOwners == null)
                    {
                        existingScorecard.KPIOwners = new List<ScorecardKPIOwner>();
                    }
                    // Adding an entirely new KPI owner 
                    existingScorecard.KPIOwners.Add(new ScorecardKPIOwner
                    {
                        UserId = kpiOwnerID,
                        AssignedOn = TimeZoneUtility.GetCurrentTimestamp(),
                        Scorecard = existingScorecard,
                        ScorecardId = existingScorecard.Id,
                        IsActive = true
                    });
                }
            }

            // Find the  newly added teams for this score card
            var newScorecardTeamIDs = updatedTeamIds.Except(existingTeams.Select(x => x.UserId)).ToList();
            foreach (var teamID in newScorecardTeamIDs)
            {
                var teamUser = existingScorecard.Teams.Where(x => x.ScorecardId == scorecardRequest.Id
                   && x.UserId == teamID).FirstOrDefault();
                if (teamUser != null)
                {
                    teamUser.IsActive = true;
                    teamUser.DeactivatedOn = null;
                    teamUser.AssignedOn = TimeZoneUtility.GetCurrentTimestamp();
                }
                else
                {
                    if (existingScorecard.Teams == null)
                    {
                        existingScorecard.Teams = new List<ScorecardTeam>();
                    }
                    existingScorecard.Teams.Add(new ScorecardTeam
                    {
                        UserId = teamID,
                        AssignedOn = TimeZoneUtility.GetCurrentTimestamp(),
                        Scorecard = existingScorecard,
                        ScorecardId = existingScorecard.Id,
                        IsActive = true
                    });
                }
            }

            // Update KPI's of score card
            List<KPI> updatedKpiEntityList = new List<KPI>();
            foreach (var kpi in scorecardRequest.KPIs)
            {
                updatedKpiEntityList.Add(Context.KPIs.Find(kpi.Id));
            }
            // First clear the existing ones
            if (existingScorecard.KPIs != null)
            {
                existingScorecard.KPIs.Clear();
            }
            else
            {
                existingScorecard.KPIs = new List<KPI>();
            }
            foreach (KPI kpi in updatedKpiEntityList)
            {
                // Add kpi in existing scorecard's kpi collection
                existingScorecard.KPIs.Add(kpi);
            }

            // Update business segments of scorecard
            List<BusinessSegment> updatedBusinessSegmentEntityList = new List<BusinessSegment>();
            if (scorecardRequest.BusinessSegments.Any(x => x.Id == 0))
            {
                updatedBusinessSegmentEntityList.Add(Context.BusinessSegments.Find(0));
            }
            else
            {
                foreach (var businessSegment in scorecardRequest.BusinessSegments)
                {
                    updatedBusinessSegmentEntityList.Add(Context.BusinessSegments.Find(businessSegment.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.BusinessSegments != null)
            {
                existingScorecard.BusinessSegments.Clear();
            }
            else
            {
                existingScorecard.BusinessSegments = new List<BusinessSegment>();
            }
            foreach (BusinessSegment businessSegment in updatedBusinessSegmentEntityList)
            {
                // Add business segment in existing scorecard's business segment collection
                existingScorecard.BusinessSegments.Add(businessSegment);
            }

            // Update Divisions of scorecard
            List<Division> updatedDivisionEntityList = new List<Division>();
            if (scorecardRequest.Divisions.Any(x => x.Id == 0))
            {
                updatedDivisionEntityList.Add(Context.Divisions.Find(0));
            }
            else
            {
                foreach (var division in scorecardRequest.Divisions)
                {
                    updatedDivisionEntityList.Add(Context.Divisions.Find(division.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.Divisions != null)
            {
                existingScorecard.Divisions.Clear();
            }
            else
            {
                existingScorecard.Divisions = new List<Division>();
            }
            foreach (Division division in updatedDivisionEntityList)
            {
                // Add divisions in existing scorecard's division collection
                existingScorecard.Divisions.Add(division);
            }

            // Update Facilities of scorecard
            List<Facility> updatedFacilityEntityList = new List<Facility>();
            if (scorecardRequest.Facilities.Any(x => x.Id == 0))
            {
                updatedFacilityEntityList.Add(Context.Facilities.Find(0));
            }
            else
            {
                foreach (var facility in scorecardRequest.Facilities)
                {
                    updatedFacilityEntityList.Add(Context.Facilities.Find(facility.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.Facilities != null)
            {
                existingScorecard.Facilities.Clear();
            }
            else
            {
                existingScorecard.Facilities = new List<Facility>();
            }
            foreach (Facility facility in updatedFacilityEntityList)
            {
                // Add Facility in existing scorecard's facility collection
                existingScorecard.Facilities.Add(facility);
            }

            // Update Product Lines of scorecard
            List<ProductLine> updatedProductLineEntityList = new List<ProductLine>();
            if (scorecardRequest.ProductLines.Any(x => x.Id == 0))
            {
                updatedProductLineEntityList.Add(Context.ProductLines.Find(0));
            }
            else
            {
                foreach (var productLine in scorecardRequest.ProductLines)
                {
                    updatedProductLineEntityList.Add(Context.ProductLines.Find(productLine.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.ProductLines != null)
            {
                existingScorecard.ProductLines.Clear();
            }
            else
            {
                existingScorecard.ProductLines = new List<ProductLine>();
            }
            foreach (ProductLine productline in updatedProductLineEntityList)
            {
                // Add Product Lines in existing scorecard's Product Line collection
                existingScorecard.ProductLines.Add(productline);
            }
            // Update Departments of scorecard
            List<Department> updatedDepartmentEntityList = new List<Department>();
            if (scorecardRequest.Departments.Any(x => x.Id == 0))
            {
                updatedDepartmentEntityList.Add(Context.Departments.Find(0));
            }
            else
            {
                foreach (var department in scorecardRequest.Departments)
                {
                    updatedDepartmentEntityList.Add(Context.Departments.Find(department.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.Departments != null)
            {
                existingScorecard.Departments.Clear();
            }
            else
            {
                existingScorecard.Departments = new List<Department>();
            }
            foreach (Department department in updatedDepartmentEntityList)
            {
                // Add departments in existing scorecard's department collection
                existingScorecard.Departments.Add(department);
            }

            // Update Processes of scorecard
            List<Process> updatedProcessEntityList = new List<Process>();
            if (scorecardRequest.Processes.Any(x => x.Id == 0))
            {
                updatedProcessEntityList.Add(Context.Processes.Find(0));
            }
            else
            {
                foreach (var process in scorecardRequest.Processes)
                {
                    updatedProcessEntityList.Add(Context.Processes.Find(process.Id));
                }
            }
            // First clear the existing ones
            if (existingScorecard.Processes != null)
            {
                existingScorecard.Processes.Clear();
            }
            else
            {
                existingScorecard.Processes = new List<Process>();
            }
            foreach (Process process in updatedProcessEntityList)
            {
                // Add processes in existing scorecard's process collection
                existingScorecard.Processes.Add(process);
            }

            DateTime currentDate = TimeZoneUtility.GetCurrentTimestamp().Date;
            DateTime EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            //Scorecard Workday Pattern
            if (scorecardRequest.ScorecardWorkdayPattern != null)
            {
                if (scorecardRequest.ActiveScorecardWorkdayPattern != null && scorecardRequest.ScorecardWorkdayPattern != null)
                {
                    if (scorecardRequest.ActiveScorecardWorkdayPattern.EffectiveStartDate.Date == scorecardRequest.ScorecardWorkdayPattern.EffectiveStartDate.Date)
                    {
                        ScorecardWorkdayPattern scorecardWorkdayPatternExisting = existingScorecard.ScorecardWorkdayPatterns.Where(x => x.Id == scorecardRequest.ScorecardWorkdayPattern.Id).FirstOrDefault();
                        scorecardWorkdayPatternExisting.LastModifiedBy = loggedInUserId;
                        scorecardWorkdayPatternExisting.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                        scorecardWorkdayPatternExisting.EffectiveEndDate = EndDate.Date;
                        existingScorecard.ScorecardWorkdayPatterns.Add(scorecardWorkdayPatternExisting);

                        ScorecardWorkdayPattern scorecardWorkdayPatternNew = new ScorecardWorkdayPattern();

                        scorecardWorkdayPatternNew.ScorecardId = scorecardRequest.ScorecardWorkdayPattern.ScorecardId;
                        scorecardWorkdayPatternNew.IsSunday = scorecardRequest.ScorecardWorkdayPattern.IsSunday;
                        scorecardWorkdayPatternNew.IsMonday = scorecardRequest.ScorecardWorkdayPattern.IsMonday;
                        scorecardWorkdayPatternNew.IsTuesday = scorecardRequest.ScorecardWorkdayPattern.IsTuesday;
                        scorecardWorkdayPatternNew.IsWednesday = scorecardRequest.ScorecardWorkdayPattern.IsWednesday;
                        scorecardWorkdayPatternNew.IsThursday = scorecardRequest.ScorecardWorkdayPattern.IsThursday;
                        scorecardWorkdayPatternNew.IsFriday = scorecardRequest.ScorecardWorkdayPattern.IsFriday;
                        scorecardWorkdayPatternNew.IsSaturday = scorecardRequest.ScorecardWorkdayPattern.IsSaturday;
                        scorecardWorkdayPatternNew.CreatedOn = TimeZoneUtility.GetCurrentTimestamp();
                        scorecardWorkdayPatternNew.CreatedBy = loggedInUserId;
                        scorecardWorkdayPatternNew.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                        scorecardWorkdayPatternNew.LastModifiedBy = loggedInUserId;

                        scorecardWorkdayPatternNew.EffectiveStartDate = EndDate.AddDays(1).Date;
                        existingScorecard.ScorecardWorkdayPatterns.Add(scorecardWorkdayPatternNew);
                    }
                    else
                    {
                        ScorecardWorkdayPattern scorecardWorkdayPatternExisting = existingScorecard.ScorecardWorkdayPatterns.Where(x => x.Id == scorecardRequest.ScorecardWorkdayPattern.Id).FirstOrDefault();
                        scorecardWorkdayPatternExisting.LastModifiedBy = loggedInUserId;
                        scorecardWorkdayPatternExisting.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                        scorecardWorkdayPatternExisting.IsSunday = scorecardRequest.ScorecardWorkdayPattern.IsSunday;
                        scorecardWorkdayPatternExisting.IsMonday = scorecardRequest.ScorecardWorkdayPattern.IsMonday;
                        scorecardWorkdayPatternExisting.IsTuesday = scorecardRequest.ScorecardWorkdayPattern.IsTuesday;
                        scorecardWorkdayPatternExisting.IsWednesday = scorecardRequest.ScorecardWorkdayPattern.IsWednesday;
                        scorecardWorkdayPatternExisting.IsThursday = scorecardRequest.ScorecardWorkdayPattern.IsThursday;
                        scorecardWorkdayPatternExisting.IsFriday = scorecardRequest.ScorecardWorkdayPattern.IsFriday;
                        scorecardWorkdayPatternExisting.IsSaturday = scorecardRequest.ScorecardWorkdayPattern.IsSaturday;
                        existingScorecard.ScorecardWorkdayPatterns.Add(scorecardWorkdayPatternExisting);
                    }
                }
                else
                {
                    var EffectiveStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ScorecardWorkdayPattern scorecardWorkdayPatternNew = new ScorecardWorkdayPattern();

                    scorecardWorkdayPatternNew.ScorecardId = existingScorecard.Id;
                    scorecardWorkdayPatternNew.IsSunday = scorecardRequest.ScorecardWorkdayPattern.IsSunday;
                    scorecardWorkdayPatternNew.IsMonday = scorecardRequest.ScorecardWorkdayPattern.IsMonday;
                    scorecardWorkdayPatternNew.IsTuesday = scorecardRequest.ScorecardWorkdayPattern.IsTuesday;
                    scorecardWorkdayPatternNew.IsWednesday = scorecardRequest.ScorecardWorkdayPattern.IsWednesday;
                    scorecardWorkdayPatternNew.IsThursday = scorecardRequest.ScorecardWorkdayPattern.IsThursday;
                    scorecardWorkdayPatternNew.IsFriday = scorecardRequest.ScorecardWorkdayPattern.IsFriday;
                    scorecardWorkdayPatternNew.IsSaturday = scorecardRequest.ScorecardWorkdayPattern.IsSaturday;
                    scorecardWorkdayPatternNew.CreatedOn = TimeZoneUtility.GetCurrentTimestamp();
                    scorecardWorkdayPatternNew.CreatedBy = loggedInUserId;
                    scorecardWorkdayPatternNew.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    scorecardWorkdayPatternNew.LastModifiedBy = loggedInUserId;
                    scorecardWorkdayPatternNew.EffectiveStartDate = EffectiveStartDate.Date;
                    existingScorecard.ScorecardWorkdayPatterns.Add(scorecardWorkdayPatternNew);

                }
            }
            //Scorecard Holiday Pattern
            if (scorecardRequest.ScorecardHolidayPattern != null)
            {
                if (scorecardRequest.ActiveScorecardHolidayPattern.EffectiveStartDate.Date == scorecardRequest.ScorecardHolidayPattern.EffectiveStartDate.Date)
                {
                    ScorecardHolidayPattern scorecardHolidayPatternExisting = existingScorecard.ScorecardHolidayPatterns.Where(x => x.Id == scorecardRequest.ScorecardHolidayPattern.Id).FirstOrDefault();
                    scorecardHolidayPatternExisting.LastModifiedBy = loggedInUserId;
                    scorecardHolidayPatternExisting.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    scorecardHolidayPatternExisting.EffectiveEndDate = EndDate.Date;
                    existingScorecard.ScorecardHolidayPatterns.Add(scorecardHolidayPatternExisting);

                    ScorecardHolidayPattern scorecardHolidayPatternNew = new ScorecardHolidayPattern();

                    scorecardHolidayPatternNew.ScorecardId = scorecardRequest.ScorecardHolidayPattern.ScorecardId;
                    scorecardHolidayPatternNew.HolidayPatternId = scorecardRequest.ScorecardHolidayPattern.HolidayPatternId;
                    scorecardHolidayPatternNew.CreatedOn = TimeZoneUtility.GetCurrentTimestamp();
                    scorecardHolidayPatternNew.CreatedBy = loggedInUserId;
                    scorecardHolidayPatternNew.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    scorecardHolidayPatternNew.LastModifiedBy = loggedInUserId;

                    scorecardHolidayPatternNew.EffectiveStartDate = EndDate.AddDays(1).Date;
                    existingScorecard.ScorecardHolidayPatterns.Add(scorecardHolidayPatternNew);
                }
                else
                {
                    ScorecardHolidayPattern sorecardHolidayPatternExisting = existingScorecard.ScorecardHolidayPatterns.Where(x => x.Id == scorecardRequest.ScorecardHolidayPattern.Id).FirstOrDefault();
                    sorecardHolidayPatternExisting.LastModifiedBy = loggedInUserId;
                    sorecardHolidayPatternExisting.LastModifiedOn = TimeZoneUtility.GetCurrentTimestamp();
                    sorecardHolidayPatternExisting.HolidayPatternId = scorecardRequest.ScorecardHolidayPattern.HolidayPatternId;
                    existingScorecard.ScorecardHolidayPatterns.Add(sorecardHolidayPatternExisting);
                }
            }
            // Call save
            Save();
            return true;
        }
        #endregion
    }
}
