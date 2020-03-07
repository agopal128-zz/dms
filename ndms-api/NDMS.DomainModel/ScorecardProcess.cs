using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
  
    public class ScorecardProcess : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of Process (Foreign key attribute)
        /// </summary>
        public int ProcessRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// Process  Navigational Property
        /// </summary>
        public virtual Process Process { get; set; }

        #endregion
    }
}
