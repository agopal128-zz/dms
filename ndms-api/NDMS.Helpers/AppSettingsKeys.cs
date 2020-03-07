using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Helpers
{
    /// <summary>
    /// Contains all App settings keys
    /// </summary>
    public static class AppSettingsKeys
    {
        #region Constant(s)
        public const string KPIPrimaryMetricCount = "KPIPrimaryMetricCount";
        public const string KPISecondaryMetricCount = "KPISecondaryMetricCount";
        public const string MaxKPIOwnerCount = "MaxKPIOwnerCount";
        public const string MaxTeamCount = "MaxTeamCount";
        public const string SameKPIOwnerforMultipleScorecards = "SameKPIOwnerforMultipleScorecards";
        public const string SameTeamMemberforMultipleScorecards = "SameTeamMemberforMultipleScorecards";
        public const string SessionTimeout = "NDMSSessionTimeout";
        public const string AutoRefreshDuration = "NDMSAutoRefreshDuration";
        public const string TimeZone = "NDMSTimeZone";
        public const string ADServer = "DefaultActiveDirectoryServer";
        public const string EnableNumberOfDaysWithoutRecordables = "EnableNumberOfDaysWithoutRecordables";
        public const string RecordableKPIId = "RecordableKPIId";
        public const string RecordableMetricId = "RecordableMetricId";
        public const string CopyMetricByDate = "CopyMetricByDate";
        public const string MaxKPICount = "MaxKPICount";
        #endregion
    }
}
