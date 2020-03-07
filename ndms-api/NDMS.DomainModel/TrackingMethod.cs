using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a Tracking Method entity
    /// </summary>
    public class TrackingMethod : BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the Tracking Method
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
