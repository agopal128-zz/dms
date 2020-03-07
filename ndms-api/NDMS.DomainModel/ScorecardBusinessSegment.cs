using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    public class ScorecardBusinessSegment : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of Business Segment (Foreign key attribute)
        /// </summary>
        public int BussinessSegmentRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// Business Segment Navigational Property
        /// </summary>
        public virtual BusinessSegment BusinessSegment { get; set; }
        
        #endregion
    }
}
