using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Cache
{
    /// <summary>
    /// Defines various constants
    /// </summary>
    internal static class CacheConstants
    {
        /// <summary>
        /// App settings key for Duration of cache
        /// </summary>
        public const string DURATION = "Cache.Duration";

        /// <summary>
        /// App settings key for Mode of caching
        /// </summary>
        public const string MODE = "Cache.Mode";

        /// <summary>
        /// App settings key for the app name
        /// </summary>
        public const string APP = "Cache.App";

        /// <summary>
        /// App settings key for the configured environment
        /// </summary>
        public const string ENVIRONMENT = "Cache.Environment";
    }
}
