using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Security
{
    /// <summary>
    /// Implements a role manager for NOV
    /// </summary>
    public class NOVRoleManager : IRoleManager
    {
        #region Field(s)
        /// <summary>
        /// User repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Scorecard KPI Owner Repository
        /// </summary>
        private IBaseRepository<ScorecardKPIOwner> scorecardKPIOwnerRepository;

        /// <summary>
        /// Scorecard Team Repository
        /// </summary>
        private IBaseRepository<ScorecardTeam> scorecardTeamRepository;
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~NOVRoleManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="userRepository">User Repository</param>
        /// <param name="scorecardKPIOwnRepo">ScorecardKPIOwner Repository</param>
        /// <param name="scorecardTeamRepo">ScorecardTeam Repository</param>
        public NOVRoleManager(IBaseRepository<User> userRepository, 
            IBaseRepository<ScorecardKPIOwner> scorecardKPIOwnRepo,
            IBaseRepository<ScorecardTeam> scorecardTeamRepo)
        {
            this.userRepository = userRepository;
            this.scorecardKPIOwnerRepository = scorecardKPIOwnRepo;
            this.scorecardTeamRepository = scorecardTeamRepo;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves the roles associated with a user
        /// </summary>
        /// <param name="username">Input user name</param>
        /// <returns>Roles associated with the user</returns>
        public List<string> GetUserRoles(string username)
        {
            List<string> roles = new List<string>();
            var userInfo = userRepository.GetAll().Where(
                x => x.AccountName == username && x.IsActive).FirstOrDefault();
            if(userInfo != null)
            {
                if(userInfo.IsAdmin)
                {
                    roles.Add(NDMSSecurityConstants.AdminRole);
                }

                var kpiOwner = scorecardKPIOwnerRepository.GetAll().FirstOrDefault(x =>
                    x.UserId == userInfo.Id && x.IsActive);
                if (kpiOwner != null)
                {
                    roles.Add(NDMSSecurityConstants.KPIOwnerRole);
                }

                var teamMember = scorecardTeamRepository.GetAll().FirstOrDefault(x =>
                    x.UserId == userInfo.Id && x.IsActive);
                if (teamMember != null)
                {
                    roles.Add(NDMSSecurityConstants.TeamMemberRole);
                }
            }
            return roles;
        }
        #endregion
    }
}
