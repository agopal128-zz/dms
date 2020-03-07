using NDMS.DataAccess.Repositories;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    /// <summary>
    /// Provides a role manager implementation
    /// </summary>
    public class RoleManagerProvider
    {
        #region Public Method(s)
        /// <summary>
        /// Returns the role manager requested
        /// </summary>
        /// <param name="authSvcIdentifier">Unique string to identify the auth service</param>
        /// <returns>Requested auth service</returns>
        public static IRoleManager GetRoleManager(string authSvcIdentifier)
        {
            var userRepo = new BaseRepository<User>();
            var kpiOwnerRepo = new BaseRepository<ScorecardKPIOwner>(userRepo.Context);
            var teamMemberRepo = new BaseRepository<ScorecardTeam>(userRepo.Context);
            switch (authSvcIdentifier)
            {
                case "NOV":
                    {
                        return new NOVRoleManager(userRepo, kpiOwnerRepo, teamMemberRepo);
                    }
                default:
                    {
                        return new NOVRoleManager(userRepo, kpiOwnerRepo, teamMemberRepo);
                    }
            }
        }
        #endregion
    }
}
