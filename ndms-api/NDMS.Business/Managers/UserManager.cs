using NDMS.Business.Interfaces;
using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using NDMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NDMS.Business.Managers
{
    /// <summary>
    /// Implements a user manager to deal with user data related
    /// services
    /// </summary>
    public class UserManager : IUserManager
    {
        #region Field(s)
        /// <summary>
        /// User Repository
        /// </summary>
        private IBaseRepository<User> userRepository;

        /// <summary>
        /// Active directory User Repository
        /// </summary>
        private IADUserRepository adUserRepository;

        /// <summary>
        /// Target Repository
        /// </summary>
        private IBaseRepository<Target> targetRepository;

        /// <summary>
        /// Year Repository
        /// </summary>
        private IBaseRepository<Year> yearRepository;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="userRepository">User Repository</param>
        /// <param name="targetRepository">Target Repository</param>
        /// <param name="yearRepository">Year Repository</param>
        /// <param name="adUserRepository">AD User Repository</param>
        public UserManager(IBaseRepository<User> userRepository,
            IBaseRepository<Target> targetRepository,
            IBaseRepository<Year> yearRepository,
            IADUserRepository adUserRepository)
        {
            this.userRepository = userRepository;
            this.targetRepository = targetRepository;
            this.yearRepository = yearRepository;
            this.adUserRepository = adUserRepository;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves all users whose last name starts with mentioned input string
        /// </summary>
        /// <param name="name">Input string match</param>
        /// <returns>List of users matched</returns>
        public IEnumerable<NDMSUserSuggestion> GetNDMSUsersWithLastName(string name)
        {
            return userRepository.GetAll()
                .Where(x => x.LastName.StartsWith(name))
                .Select(user => new NDMSUserSuggestion()
                {
                    Id = user.Id,
                    FullName = user.FirstName + " " + user.LastName + "(" + user.AccountName + ")"
                }).ToList();
        }

        /// <summary>
        /// Retrieves all Active directory users whose account name or last name starts with mentioned 
        /// input string
        /// </summary>
        /// <param name="name">Input string to match</param>
        /// <returns>List of users matched</returns>
        public IEnumerable<ADUserSuggestion> GetADUsersWithLastNameOrAccountName(string name)
        {
            return adUserRepository.GetAll()
               .Where(x => x.LastName.StartsWith(name) || x.AccountName.StartsWith(name))
               .Select(user => new ADUserSuggestion()
               {
                   AccountName = user.AccountName,
                   FullName = user.FirstName + " " + user.LastName + "(" + user.AccountName + ")"
               }).ToList();
        }

        /// <summary>
        /// Method to check whether logged in user is kpi owner of the given scorecard
        /// </summary>
        /// <param name="userName">logged in user name</param>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <returns>Boolean</returns>
        public bool IsUserKPIOwnerOfScorecard(string userName, int scorecardId)
        {
            bool isUserKPIOwnerOfScorecard = userRepository.GetAll().Any(
               x => x.AccountName == userName &&
               x.KPIOwnerScorecards.Any(s => s.ScorecardId == scorecardId && s.IsActive));

            return isUserKPIOwnerOfScorecard;
        }

        ///<summary>
        ///Method to check whether logged in user is kpi owner of the parent of given scorecard
        ///</summary>
        ///<param name="username">logged in user name</param>
        ///<param name="scorecardId">Scorecard Id</param>
        ///<returns>Boolean</returns>
        public bool IsUserKPIOwnerOfParentScorecard(string userName, int scorecardId)
        {
            bool isUserKPIOwnerOfParentScorecard = userRepository.GetAll().Any(x => 
                                            x.AccountName == userName &&
                                            x.KPIOwnerScorecards.Any(s => s.IsActive &&
                                                 s.Scorecard.ChildScorecards.Any(c => c.Id == scorecardId)));
            return isUserKPIOwnerOfParentScorecard;
        }

        /// <summary>
        /// Method to get scorecard id and target availability status
        /// </summary>
        /// <param name="username">Logged in user name</param>
        /// <returns>ScorecardTargetStatusData DS</returns>
        public ScorecardTargetStatusData GetScorecardIdAndTargetStatus(string userName)
        {
            int? scorecardId = null;
            bool isTargetAvailable = false;

            var user = userRepository.GetAll().Where(x => x.AccountName == userName && x.IsActive)
                .FirstOrDefault();

            scorecardId = GetDefaultScorecardId(user);
            if (scorecardId.HasValue)
            {
                DateTime curDate = TimeZoneUtility.GetCurrentTimestamp().Date;
                var currentYear = yearRepository.GetAll().Where(x => x.StartDate <= curDate &&
                    x.EndDate >= curDate).FirstOrDefault();
                if (currentYear != null)
                {
                    isTargetAvailable = targetRepository.GetAll().Any(
                        x => x.ScorecardId == scorecardId &&
                        x.CalendarYearId == currentYear.Id
                        && x.IsActive);
                }

            }
            return new ScorecardTargetStatusData()
            {
                Id = scorecardId,
                IsTargetAvailable = isTargetAvailable,
            };
        }

        ///<summary>
        ///To Get the Id of the default scorecard to show the user
        ///</summary>
        ///<param name="userName"> The Logged in user name</param>
        ///<returns> Returns the Id of the default scorecard to show the user</returns>
        public int? GetDefaultScorecardId(User user)
        {
            int? scorecardId = null;
            List<int> scorecardIdList = new List<int>();
            
            // check if user is a KPI Owner of any scorecard(s), add the scorecardId to  scorecardIdList
            if (user?.KPIOwnerScorecards.Count(x => x.IsActive) > 0)
            {
                scorecardIdList = user.KPIOwnerScorecards.Where(x => x.IsActive)?.Select(x => x.ScorecardId).ToList();
            }
            //check if user is a KPI Owner of any scorecard(s), add the scorecardId to  scorecardIdList
            else if (user?.TeamScorecards.Count(x => x.IsActive) > 0)
            {
                scorecardIdList.AddRange(user.TeamScorecards.Where(x => x.IsActive)?.Select(x => x.ScorecardId).ToList());
            }
            scorecardId = scorecardIdList.Any()? scorecardIdList?.Min() : null ;

            return scorecardId;
        }

        /// <summary>
        /// To check whether user is assigned to multiple scorecards
        /// </summary>
        /// <param name="user">The logged in user</param>
        /// <returns>true if multiple scorecards are assigned to user, otherwise returns false</returns>
        public bool IsUserAssignedToMultipleScorecards(User user)
        {
            IEnumerable<int> userScoreCardIds = new List<int>();

            if (user?.KPIOwnerScorecards.Count(x => x.IsActive) > 0)
            {
                userScoreCardIds = user.KPIOwnerScorecards
                                   .Where(x => x.IsActive)
                                   .Select(x => x.ScorecardId);
            }

            if (user?.TeamScorecards.Count(x => x.IsActive) > 0)
            {
                userScoreCardIds = userScoreCardIds.Union(user.TeamScorecards
                                   .Where(x => x.IsActive)
                                   .Select(x => x.ScorecardId));
            }

            bool hasMulitpleScorecards = userScoreCardIds.Distinct().Count() > 1;
            return hasMulitpleScorecards;
        }
        /// <summary>
        /// To check whether user is assigned to multiple scorecards
        /// </summary>
        /// <param name="userName">The logged in userName</param>
        /// <returns>true if multiple scorecards are assigned to user, otherwise returns false</returns>
        public bool IsUserAssignedToMultipleScorecards(string userName)
        {
            var user = userRepository.GetAll().Where(x => x.AccountName == userName && x.IsActive)
               .FirstOrDefault();

            return IsUserAssignedToMultipleScorecards(user);
        }

        /// <summary>
        /// Determines whether [is user team member of scorecard] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is user team member of scorecard] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserTeamMemberOfScorecard(string userName, int scorecardId)
        {
            bool isUserTeamMemberOfScorecard = userRepository.GetAll().Any(
               x => x.AccountName == userName &&
               x.TeamScorecards.Any(s => s.ScorecardId == scorecardId && s.IsActive));

            return isUserTeamMemberOfScorecard;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (userRepository != null)
                    {
                        userRepository.Dispose();
                    }
                    if (targetRepository != null)
                    {
                        targetRepository.Dispose();
                    }
                    if (yearRepository != null)
                    {
                        yearRepository.Dispose();
                    }
                    // Assign user repository reference to null
                    userRepository = null;
                    targetRepository = null;
                    yearRepository = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
