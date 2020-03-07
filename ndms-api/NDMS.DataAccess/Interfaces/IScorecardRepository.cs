using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System.Collections.Generic;

namespace NDMS.DataAccess.Interfaces
{
    public interface IScorecardRepository : IBaseRepository<Scorecard>
    {
        /// <summary>
        /// Update an existing scorecard
        /// </summary>
        /// <param name="scorecardRequest">Scorecard DTO to update</param>
        /// <param name="updatedKPIOwnerIds">Updated KPI Owner Id's</param>
        /// <param name="updatedTeamIds">Updated Team Member Id's</param>
        /// <param name="userName">Logged in user name</param>
        /// <returns>flag which says whether update is successful or not</returns>
        bool UpdateScorecard(ScorecardItem scorecardRequest, List<int> updatedKPIOwnerIds,
             List<int> updatedTeamIds, string userName);
    }
}
