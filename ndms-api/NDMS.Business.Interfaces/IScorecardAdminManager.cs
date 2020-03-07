using NDMS.DomainModel.DTOs;
using System;
using System.Collections.Generic;

namespace NDMS.Business.Interfaces
{
    /// <summary>
    /// Defines scorecard administration related methods
    /// </summary>
    public interface IScorecardAdminManager : IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Retrieves Scorecard by id
        /// </summary>
        /// <param name="id">ID of score card</param>
        /// <returns>Scorecard with the ID</returns>
        ScorecardItem GetScorecard(int id);

        /// <summary>
        /// Add scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>added scorecard id</returns>
        int AddScorecard(ScorecardItem scorecardRequest, string userName);

        /// <summary>
        /// Update an existing scorecard
        /// </summary>
        /// <param name="scorecardRequest">scorecard details object</param>
        /// <param name="userName">logged in user name</param>
        /// <returns>flag which says whether update is successful or not</returns>
        bool UpdateScorecard(ScorecardItem scorecardRequest, string userName);

        /// <summary>
        /// Retrieve all the Scorecards in a hierarchy mode. 
        /// </summary>
        /// <returns></returns>
        ScorecardNode GetScorecardHierarchy(string userName, int? rootScorecardId, int? selectedScorecardId);

        /// <summary>
        /// Gets all the root nodes for hierarchy dropdown
        /// </summary>
        /// <returns></returns>
        HierarchyMenuDTO GetHierarchyDropdownList(string userName);

        /// <summary>
        /// Finds the root of current tree
        /// </summary>
        /// <param name="parentScorecardId"></param>
        /// <returns></returns>
        int? GetRootScorecardOfTree(int? parentScorecardId);

        /// <summary>
        /// Swaps the order in which score 
        /// </summary>
        /// <param name="scorecardId"></param>
        /// <param name="nextScorecardId"></param>
        /// <returns></returns>
        bool SwapScorecardSortOrders(SwapScorecardSortOrderRequest swapScorecardsRequest, string userName);

        #endregion
    }
}
