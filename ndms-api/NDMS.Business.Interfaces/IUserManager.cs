using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines an interface which contains all methods to deal with User database
    /// </summary>
    public interface IUserManager : IDisposable
    {
        /// <summary>
        /// Retrieves all NDMS users whose last name starts with mentioned 
        /// input string
        /// </summary>
        /// <param name="name">Input string to match</param>
        /// <returns>List of users matched</returns>
        IEnumerable<NDMSUserSuggestion> GetNDMSUsersWithLastName(string name);

        /// <summary>
        /// Retrieves all Active directory users whose account name or last name starts with mentioned 
        /// input string
        /// </summary>
        /// <param name="name">Input string to match</param>
        /// <returns>List of users matched</returns>
        IEnumerable<ADUserSuggestion> GetADUsersWithLastNameOrAccountName(string name);

        /// <summary>
        /// Method to check whether logged in user is kpi owner of the given scorecard
        /// </summary>
        /// <param name="userName">Logged in user name</param>
        /// <param name="scorecardId">Scorecard Id</param>
        /// <returns></returns>
        bool IsUserKPIOwnerOfScorecard(string userName, int scorecardId);

        ///<summary>
        ///Method to check whether logged in user is kpi owner of the parent of given scorecard
        ///</summary>
        ///<param name="username">logged in user name</param>
        ///<param name="scorecardId">Scorecard Id</param>
        ///<returns>Boolean</returns>
        bool IsUserKPIOwnerOfParentScorecard(string userName, int scorecardId);

        /// <summary>
        /// Method to get scorecard id and and target availability status
        /// </summary>
        /// <param name="username">Logged in user name</param>
        /// <returns>ScorecardTargetStatusData DS</returns>
        ScorecardTargetStatusData GetScorecardIdAndTargetStatus(string username);

        /// <summary>
        /// Determines whether [is user assigned to multiple scorecards] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        ///   <c>true</c> if [is user assigned to multiple scorecards] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>
        bool IsUserAssignedToMultipleScorecards(string userName);

        /// <summary>
        /// Determines whether [is user team member of scorecard] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="scorecardId">The scorecard identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is user team member of scorecard] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>
        bool IsUserTeamMemberOfScorecard(string userName, int scorecardId);
    }
}
