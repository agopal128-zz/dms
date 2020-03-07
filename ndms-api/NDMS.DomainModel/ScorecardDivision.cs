using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    public class ScorecardDivision : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of Division (Foreign key attribute)
        /// </summary>
        public int DivisionRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// Division Navigational Property
        /// </summary>
        public virtual Division Division { get; set; }

        #endregion
    }
}
