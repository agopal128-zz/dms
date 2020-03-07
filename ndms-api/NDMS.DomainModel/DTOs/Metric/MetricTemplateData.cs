using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    /// <summary>
    /// DTO to list Goal Types and Data Types
    /// </summary>
    public class MetricTemplateData
    {
        #region Propertie(s)
        /// <summary>
        /// Collection of Goal Types
        /// </summary>
        public IEnumerable<GoalTypeItem> GoalTypes { get; set; }

        /// <summary>
        /// Collection of Data Types
        /// </summary>
        public IEnumerable<DataTypeItem> DataTypes { get; set; }
        #endregion
    }
}
