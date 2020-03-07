using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Cache
{
    /// <summary>
    /// Defines various utilities for the Cache
    /// </summary>
    internal class CacheUtils
    {
        #region Field(s)
        /// <summary>
        /// Default duration of the cache
        /// </summary>
        private static int defaultCacheDuration = 30;
        #endregion
      
        #region Private Method(s)
        /// <summary>
        /// Constructs a key prefix based on Application and Environment name
        /// </summary>
        /// <returns>Cache prefix constructed</returns>
        private static string GetCacheKeyPrefix()
        {
            // App name is a unique value associated with this application. This becomes
            // valid when cache is shared by multiple applications(e.g Azure Redis cache).
            string appName = ConfigurationManager.AppSettings[CacheConstants.APP];
            // Environment defines whether this is a Dev, QA or Production. Cache can be shared
            // between environments.            
            string environmentName = ConfigurationManager.AppSettings[CacheConstants.ENVIRONMENT];
            
            if (string.IsNullOrEmpty(appName) || string.IsNullOrEmpty(environmentName))
                throw new Exception("Application name/Environment not configured for cache");

            return string.Concat(appName, "_", environmentName);
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Static constructor
        /// </summary>
        static CacheUtils()
        {
            // Read the cache duration configured in web.config just once
            var duration = ConfigurationManager.AppSettings[CacheConstants.DURATION];
            if (!string.IsNullOrEmpty(duration))
            {
                Int32.TryParse(duration, out defaultCacheDuration);
            }
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Returns the default cache duration configured
        /// </summary>
        internal static int DefaultCacheDuration
        {
            get
            {
                return defaultCacheDuration;
            }
        }
        #endregion

        #region Internal Method(s)
        /// <summary>
        /// Returns the actual key by appending the prefixes
        /// </summary>
        /// <typeparam name="T">Type of the object to cache</typeparam>
        /// <param name="key">Input key supplied by consumer</param>
        /// <returns>Constructed key</returns>
        internal static string GetCacheKey<T>(string key)
        {            
            string typeName = typeof(T).ToString();
            string[] cacheKey = new string[] { GetCacheKeyPrefix(), "_", typeName, "_", key };
            return string.Concat(cacheKey);
        }

        /// <summary>
        /// Returns a key for a type T
        /// </summary>
        /// <typeparam name="T">Type of the object to cache</typeparam>
        /// <returns>Constructed key</returns>
        internal static string GetCacheKey<T>()
        {
            string typeName = typeof(T).ToString();
            string[] cacheKey = new string[] { GetCacheKeyPrefix(), "_", typeName };
            return string.Concat(cacheKey);
        }

        /// <summary>
        /// Returns a key with only prefixes
        /// </summary>
        /// <returns>Constructed key</returns>
        internal static string GetCacheKey()
        {
            return GetCacheKeyPrefix();
        }
        #endregion
    }
}
