using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a counter measure entity in the system
    /// </summary>
    public class CounterMeasure : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Id of the corresponding Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Id of the KPI(Foreign key attribute)
        /// </summary>
        public int KPIId { get; set; }

        /// <summary>
        /// Id of the Metric(Foreign key attribute)
        /// </summary>
        public int MetricId { get; set; }

        /// <summary>
        /// Issue
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// Action for the Counter Measure
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Due Date of Counter Measure
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Id of Counter Measure Status(Foreign key attribute)
        /// </summary>
        public int CounterMeasureStatusId { get; set; }

        /// <summary>
        /// Gets or sets the counter measure priority(Foreign key attribute).
        /// </summary>
        public int? CounterMeasurePriorityId { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// User ID of the created user
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// modified date
        /// </summary>
        public DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// User ID of the modified user
        /// </summary>
        public int LastModifiedBy { get; set; }

        /// <summary>
        /// Assigned User Id(Foreign key attribute)
        /// </summary>
        public int AssignedTo { get; set; }

        /// <summary>
        /// Score card Navigational Property
        /// </summary>
        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// KPI Navigational Property
        /// </summary>
        public virtual KPI KPI { get; set; }

        /// <summary>
        /// Metric Navigational Property
        /// </summary>
        public virtual Metric Metric { get; set; }

        /// <summary>
        /// CounterMeasure Status Navigational Property
        /// </summary>
        public virtual CounterMeasureStatus CounterMeasureStatus { get; set; }

        /// <summary>
        /// Associated Comments
        /// </summary>
        public virtual ICollection<CounterMeasureComment> CounterMeasureComments { get; set; }

        /// <summary>
        /// CounterMeasure Priority Navigational Property.
        /// </summary>
        public virtual CounterMeasurePriority CounterMeasurePriority { get; set; }

        /// <summary>
        /// CreatedBy User Navigation Property
        /// </summary>
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Last ModifiedBy User Navigation Property
        /// </summary>
        public virtual User LastModifiedByUser { get; set; }

        /// <summary>
        /// Assigned User Navigation Property
        /// </summary>
        public virtual User AssignedUser { get; set; }
        #endregion
    }
}
