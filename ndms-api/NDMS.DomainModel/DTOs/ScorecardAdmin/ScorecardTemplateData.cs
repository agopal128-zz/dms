using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to list business segment, product line, process, department and KPI
    /// </summary>
    public class ScorecardTemplateData
    {
        #region Propertie(s)
        /// <summary>
        /// Collection of Business Segments
        /// </summary>
        public IEnumerable<BusinessSegmentItem> BusinessSegments { get; set; }

        /// <summary>
        /// Collection of divisions
        /// </summary>
        public IEnumerable<DivisionItem> Divisions { get; set; }

        /// <summary>
        /// Collection of facilities
        /// </summary>
        public IEnumerable<FacilityItem> Facilities { get; set; }

        /// <summary>
        /// Collection of Product Lines
        /// </summary>
        public IEnumerable<ProductLineItem> ProductLines { get; set; }

        /// <summary>
        /// Collection of Process
        /// </summary>
        public IEnumerable<ProcessItem> Processes { get; set; }

        /// <summary>
        /// Collection of Departments
        /// </summary>
        public IEnumerable<DepartmentItem> Departments { get; set; }

        /// <summary>
        /// Collection of KPI's
        /// </summary>
        public IEnumerable<KPIItem> KPIs { get; set; }

        /// <summary>
        /// Parent scorecard. This is to default certain attributes of child scorecard to same as
        /// parent
        /// </summary>
        public ScorecardItem ParentScorecardItem { get; set; }

        ///<summary>
        /// Root scorecard of the tree in which currently adding or updating scorecard exist
        /// </summary>
        public int? RootScorecardId { get; set; }

        /// <summary>
        /// Collection of KPI's
        /// </summary>
        public IEnumerable<ScorecardHolidayPatternItem> ScorecardHolidayPatterns { get; set; }
        #endregion
    }
}
