﻿using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents Daily Actuals entity
    /// </summary>
    public class DailyActual : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Target identifier
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// Date to which actual is entered
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// actual value entered
        /// </summary>
        public decimal? ActualValue { get; set; }

        /// <summary>
        /// target value
        /// </summary>
        public decimal? GoalValue { get; set; }

        /// <summary>
        /// status when comparing actual and goal value
        /// </summary>
        public ActualStatus Status { get; set; }

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
        /// Target navigational property
        /// </summary>
        public virtual Target Target { get; set; }

        /// <summary>
        /// History of Daily Actuals
        /// </summary>
        public virtual ICollection<DailyActualHistory> DailyActualHistory { get; set; }

        #endregion
    }
}
