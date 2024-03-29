﻿using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represent a facility in a region
    /// </summary>
    public class Facility : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the facility
        /// </summary>
        public string Name { get; set; }

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
        /// Flag which says whether facility is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// CreatedBy User Navigation Property
        /// </summary>
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Last ModifiedBy User Navigation Property
        /// </summary>
        public virtual User LastModifiedByUser { get; set; }

        /// <summary>
        /// List of Scorecards which are having this Facility
        /// </summary>
        public virtual ICollection<Scorecard> Scorecards { get; set; }
        #endregion
    }
}
