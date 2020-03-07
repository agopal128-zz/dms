using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    public class ScorecardProductLine : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of ProductLine (Foreign key attribute)
        /// </summary>
        public int ProductLineRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// ProductLine  Navigational Property
        /// </summary>
        public virtual ProductLine ProductLine { get; set; }

        #endregion
    }
}
