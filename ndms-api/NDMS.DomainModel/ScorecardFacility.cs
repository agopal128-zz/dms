using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    public class ScorecardFacility : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of Facility (Foreign key attribute)
        /// </summary>
        public int FacilityRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        ///Facility Navigational Property
        /// </summary>
        public virtual Facility Facility { get; set; }

        #endregion
    }
}
