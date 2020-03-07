using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a score card entity in the system
    /// </summary>
    public class Scorecard : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the score card
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Identifier of the parent Scorecard(Foreign key attribute)
        /// </summary>
        public int? ParentScorecardId { get; set; }        

        /// <summary>
        /// Flag which says whether bowling chart is applicable or not
        /// </summary>
        public bool IsBowlingChartApplicable { get; set; }

        /// <summary>
        /// Default level up-to which Scorecard hierarchy needs to be expanded 
        /// </summary>
        public int DrilldownLevel { get; set; }

        /// <summary>
        /// The sort order for scorecard
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// User ID of the created user
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// modified date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// User ID of the modified user
        /// </summary>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// Flag which says whether score card is active or not
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Parent score card Navigational property
        /// </summary>
        public virtual Scorecard ParentScorecard { get; set; }

        /// <summary>
        /// Business segment Navigational property
        /// </summary>
        public virtual ICollection<BusinessSegment> BusinessSegments { get; set; }

        /// <summary>
        /// Division Navigational property
        /// </summary>
        public virtual ICollection<Division> Divisions { get; set; }

        /// <summary>
        /// Facility Navigational property
        /// </summary>
        public virtual ICollection<Facility> Facilities { get; set; }

        /// <summary>
        /// Product Line Navigational property
        /// </summary>
        public virtual ICollection<ProductLine> ProductLines { get; set; }

        /// <summary>
        /// Department Navigational property
        /// </summary>
        public virtual ICollection<Department> Departments { get; set; }

        /// <summary>
        /// Process Navigational property
        /// </summary>
        public virtual ICollection<Process> Processes { get; set; }

        /// <summary>
        /// Associated KPI Owners
        /// </summary>
        public virtual ICollection<ScorecardKPIOwner> KPIOwners { get; set; }

        /// <summary>
        /// Associated KPI's
        /// </summary>
        public virtual ICollection<KPI> KPIs { get; set; }

        /// <summary>
        /// Associated Teams
        /// </summary>
        public virtual ICollection<ScorecardTeam> Teams { get; set; }

        /// <summary>
        /// Child Scorecards
        /// </summary>
        public virtual ICollection<Scorecard> ChildScorecards { get; set; }

        /// <summary>
        /// Recordables
        /// </summary>
        public virtual ICollection<Recordable> Recordables { get; set; }

        /// <summary>
        /// Scorecard Workday Pattern
        /// </summary>
        public virtual ICollection<ScorecardWorkdayPattern> ScorecardWorkdayPatterns { get; set; }

        /// <summary>
        /// Scorecard Workday Pattern
        /// </summary>
        public virtual ICollection<ScorecardHolidayPattern> ScorecardHolidayPatterns { get; set; }        
       

        #endregion
    }
}
