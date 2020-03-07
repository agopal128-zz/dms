using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Helpers
{
    /// <summary>
    /// Utility class to deal with time zone
    /// </summary>
    public static class TimeZoneUtility
    {
        #region Field(s)
        /// <summary>
        /// Represents configured time zone
        /// </summary>
        private static readonly string curTimeZone;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Static constructor
        /// </summary>
        static TimeZoneUtility()
        {
            // In case of empty or invalid entries, take UTC as the default one
            curTimeZone = ConfigurationManager.AppSettings[AppSettingsKeys.TimeZone];
            if (string.IsNullOrEmpty(curTimeZone))
            {
                curTimeZone = "UTC";
            }
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Returns the current time stamp
        /// </summary>
        /// <returns>Current time stamp</returns>
        public static DateTime GetCurrentTimestamp()
        {
            switch (curTimeZone)
            {
                case "UTC":
                    {
                        return DateTime.UtcNow;
                    }
                case "Local":
                    {
                        return DateTime.Now;
                    }
                default:
                    {
                        return DateTime.UtcNow;
                    }
            }
        }
        #endregion
    }
}
