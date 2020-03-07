using System.Collections.Generic;

namespace NDMS.DomainModel.DTOs
{
    public class ScorecardData
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScorecardData()
        {
            this.Kpis = new List<ScorecardKPIData>();
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Identifier of Scorecard
        /// </summary>
        public int ScorecardId { get; set; }

        /// <summary>
        /// Name of Scorecard
        /// </summary>
        public string ScorecardName { get; set; }

        /// <summary>
        /// Number of days without recordable
        /// </summary>
        public double? DaysWithoutRecordables { get; set; }

        /// <summary>
        /// List of KPI Owners
        /// </summary>
        public IEnumerable<string> KPIOwners { get; set; }

        /// <summary>
        /// List of applicable KPIs for Scorecard
        /// </summary>
        public List<ScorecardKPIData> Kpis { get; set; }

        /// <summary>
        /// Flag to show pattern assigned or not
        /// </summary>
        public bool IsPatternAssigned { get; set; }

        #endregion
    }
}
