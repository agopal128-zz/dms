using System;
using System.Collections.Generic;

namespace NDMS.DomainModel
{
    public class User : BaseEntity
    {
        public string AccountName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int? LastLocationID { get; set; }

        public bool IsAdmin { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        /// <summary>
        /// Associated Scorecards in which the user is KPI Owner
        /// </summary>
        public virtual ICollection<ScorecardKPIOwner> KPIOwnerScorecards { get; set; }

        /// <summary>
        /// Associated Scorecards in which the user is Team Membe
        /// </summary>
        public virtual ICollection<ScorecardTeam> TeamScorecards { get; set; }
    }
}
