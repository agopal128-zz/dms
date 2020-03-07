using NDMS.DomainModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO for scorecard entity
    /// </summary>
    public class ScorecardItem
    {
        #region Propertie(s)

        /// <summary>
        /// Identifier of the scorecard
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Name of the score card
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.ScorecardNameEmpty)]
        [MaxLength(100, ErrorMessage = ValidationMessages.ScorecardNameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// ID of the parent scorecard
        /// </summary>
        public int? ParentScorecardId { get; set; }

        /// <summary>
        /// Root Scorecard Id
        /// </summary>
        public int? RootScorecardId { get; set; }

        /// <summary>
        /// Selected Business segments
        /// </summary>
        public IEnumerable<BusinessSegmentItem> BusinessSegments { get; set; }

        /// <summary>
        /// Selected Divisions
        /// </summary>
        public IEnumerable<DivisionItem> Divisions { get; set; }

        /// <summary>
        /// Selected Facility
        /// </summary>
        public IEnumerable<FacilityItem> Facilities { get; set; }

        /// <summary>
        /// Selected Product Line
        /// </summary>
        public IEnumerable<ProductLineItem> ProductLines { get; set; }

        /// <summary>
        /// Selected Process
        /// </summary>
        public IEnumerable<ProcessItem> Processes { get; set; }

        /// <summary>
        /// Selected Department
        /// </summary>
        public IEnumerable<DepartmentItem> Departments { get; set; }

        /// <summary>
        /// Hierarchy drill down level
        /// </summary>
        public int DrilldownLevel { get; set; }

        /// <summary>
        /// Flag which says whether score card is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Flag which says whether bowling chart is applicable or not
        /// </summary>
        public bool IsBowlingChartApplicable { get; set; }

        /// <value>
        /// The recordable details.
        /// </value>
        public RecordableItem Recordable { get; set; }

        /// <summary>
        /// Associated KPI Owners
        /// </summary>
        public IEnumerable<KPIOwnerItem> KPIOwners { get; set; }

        /// <summary>
        /// Associated KPI's
        /// </summary>
        public IEnumerable<KPIItem> KPIs { get; set; }

        /// <summary>
        /// Associated Teams
        /// </summary>
        public IEnumerable<ScorecardTeamItem> Teams { get; set; }

        /// <summary>
        ///Scorecard Workday Pattern
        /// </summary>
        public ScorecardWorkdayPatternItem  ScorecardWorkdayPattern { get; set; }

        /// <summary>
        /// Active Scorecard Workday Pattern
        /// </summary>
        public ScorecardWorkdayPatternItem ActiveScorecardWorkdayPattern { get; set; }

        /// <summary>
        ///Scorecard Holiday Pattern
        /// </summary>
        public ScorecardHolidayPatternItem ScorecardHolidayPattern { get; set; }

        /// <summary>
        /// Active Scorecard Workday Pattern
        /// </summary>
        public ScorecardHolidayPatternItem ActiveScorecardHolidayPattern { get; set; }

        /// <summary>
        /// Effective start date of next workday pattern
        /// </summary>
        public DateTime? NextWorkdayPatternStartDate { get; set; }

        /// <summary>
        /// Effective start date of next holiday pattern
        /// </summary>
        public DateTime? NextHolidayPatternStartDate { get; set; }

        /// <summary>
        /// Start date of current month
        /// </summary>
        public DateTime? CurrentWorkdayPatternStartDate { get; set; }
    }
    #endregion
}
