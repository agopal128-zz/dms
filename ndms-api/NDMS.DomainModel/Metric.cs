using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Defines a metric in the system
    /// </summary>
    public class Metric : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Flag which says whether metric is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Identifier of the GoalType(Foreign key attribute)
        /// </summary>
        public int GoalTypeId { get; set; }

        /// <summary>
        /// Identifier of the DatalType(Foreign key attribute)
        /// </summary>
        public int DataTypeId { get; set; }

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
        /// Goal type of this metric
        /// </summary>
        public virtual GoalType GoalType { get; set; }

        /// <summary>
        /// Data type of this metric
        /// </summary>
        public virtual DataType DataType { get; set; }

        /// <summary>
        /// Targets which are part of the metric
        /// </summary>
        public virtual ICollection<Target> AnnualTargets { get; set; }
        #endregion
    }

    /// <summary>
    /// Defines the type of the metric
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// Primary metric
        /// </summary>
        Primary,

        /// <summary>
        /// Secondary metric
        /// </summary>
        Secondary
    }

}
