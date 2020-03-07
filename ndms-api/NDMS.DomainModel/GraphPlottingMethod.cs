using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.DomainModel
{
    /// <summary>
    /// Represents a GraphPlotting Method entity
    /// </summary>
    public class GraphPlottingMethod: BaseEntity
    {
        #region Propertie(s)
        /// <summary>
        /// Name of the Graph plotting Method
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
