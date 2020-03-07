using System;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Defines a metric mapping rule
    /// </summary>
    public class MetricMapping : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Business Segment Identifier
        /// </summary>
        public int BusinessSegmentId { get; set; }

        /// <summary>
        /// Division identifier
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Facility Identifier
        /// </summary>
        public int FacilityId { get; set; }

        /// <summary>
        /// Product line identifier
        /// </summary>
        public int ProductLineId { get; set; }

        /// <summary>
        /// Department identifier
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Process identifier
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// KPI Identifier
        /// </summary>
        public int KPIId { get; set; }

        /// <summary>
        /// Metric Identifier
        /// </summary>
        public int MetricId { get; set; }

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
        /// Flag which says whether mapping is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// KPI Navigational property
        /// </summary>
        public virtual KPI KPI { get; set; }

        /// <summary>
        /// Metric Navigational property
        /// </summary>
        public virtual Metric Metric { get; set; }

        /// <summary>
        /// Business segment Navigational property
        /// </summary>
        public virtual BusinessSegment BusinessSegment { get; set; }

        /// <summary>
        /// Division Navigational property
        /// </summary>
        public virtual Division Division { get; set; }

        /// <summary>
        /// Facility Navigational property
        /// </summary>
        public virtual Facility Facility { get; set; }

        /// <summary>
        /// ProductLine Navigational property
        /// </summary>
        public virtual ProductLine ProductLine { get; set; }

        /// <summary>
        /// Department Navigational property
        /// </summary>
        public virtual Department Department { get; set; }

        /// <summary>
        /// Process Navigational property
        /// </summary>
        public virtual Process Process { get; set; }

        #endregion
    }
}
