using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    public class ScorecardDepartment : BaseEntity
    {
        #region Propertie(s)       

        /// <summary>
        /// Id of Scorecard(Foreign key attribute)
        /// </summary>
        public int ScorecardRefId { get; set; }

        /// <summary>
        /// Id of Department (Foreign key attribute)
        /// </summary>
        public int DepartmentRefId { get; set; }

        public virtual Scorecard Scorecard { get; set; }

        /// <summary>
        /// Department Navigational Property
        /// </summary>
        public virtual Department Department { get; set; }

        #endregion
    }
}
