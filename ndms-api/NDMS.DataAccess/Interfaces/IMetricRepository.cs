using NDMS.DomainModel;
using NDMS.DomainModel.DTOs;

namespace NDMS.DataAccess.Interfaces
{
    public interface IMetricRepository : IBaseRepository<Metric>
    {
        /// <summary>
        /// Retrieves all active goal types and data types 
        /// </summary>
        /// <returns>object with goal types and data types</returns>
        MetricTemplateData GetMetricTemplateData();
    }
}
