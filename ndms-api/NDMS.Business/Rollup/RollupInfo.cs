using NDMS.DomainModel.DTOs;
using System;

namespace NDMS.Business.Rollup
{
    /// <summary>
    /// Details needed to perform actuals rollup operations
    /// </summary>
    public class RollupInfo
    {
        #region Propertie(s)
        /// <summary>
        /// Parent target Id
        /// </summary>
        public int? ParentTargetId
        {
            get;
            set;
        }

        /// <summary>
        /// Parent Scorecard Id
        /// </summary>
        public int? ParentScorecardId
        {
            get;
            set;
        }

        /// <summary>
        /// Roll up method identifier
        /// </summary>
        public int? RollupMethodId
        {
            get;
            set;
        }

        /// <summary>
        /// Tracking method Id
        /// </summary>
        public int TrackingMethodId
        {
            get;
            set;
        }
        
        /// <summary>
        /// Metric Goal type Id
        /// </summary>
        public int GoalTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Metric Data type Id
        /// </summary>
        public int DataTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Target Entry Date
        /// </summary>
        public DateTime TargetEntryDate
        {
            get;
            set;
        }

        /// <summary>
        /// Actual Entry which is triggering the roll up action
        /// </summary>
        public ActualItem ActualEntry
        {
            get;
            set;
        }

        /// <summary>
        /// Logged in user name
        /// </summary>
        public string Username
        {
            get;
            set;
        }

        public bool UpdateRecordable
        {
            get;
            set;
        }
        #endregion
    }
}
