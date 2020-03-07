using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;
using System.Linq;

namespace NDMS.DataAccess.Repositories
{
    /// <summary>
    /// Repository to store and retrieve Metric entity
    /// </summary>
    public class MetricRepository : BaseRepository<Metric>, IMetricRepository
    {
        #region Public Method(s)
        /// <summary>
        /// Method to retrieve all active goal types and data types 
        /// </summary>
        /// <returns>object with goal types and data types</returns>
        public MetricTemplateData GetMetricTemplateData()
        {

            var goalTypes = Context.GoalTypes.Where(gt => gt.IsActive)
                .Select(gt => new GoalTypeItem()
                {
                    Id = gt.Id,
                    Name = gt.Name
                }).OrderBy(gt => gt.Name).ToList();

            var dataTypes = Context.DataTypes.Where(dt => dt.IsActive)
                .Select(dt => new DataTypeItem()
                {
                    Id = dt.Id,
                    Name = dt.Name
                }).OrderBy(dt=>dt.Name).ToList();            
            return new MetricTemplateData()
            {
                GoalTypes = goalTypes,
                DataTypes = dataTypes
            };
        }

        #endregion


    }
}
