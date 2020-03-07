using System;
using System.Configuration;

namespace NDMS.Cache
{
    /// <summary>
    /// Manages the configured cache
    /// </summary>
    public class CacheManager
    {
        #region Field(s)
        /// <summary>
        /// Reference to the cache instance created as per the configuration
        /// </summary>
        private static ICache instance = null;

        /// <summary>
        /// Lock object for thread safe instance creation
        /// </summary>
        private static object instanceLock = new object();
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Default constructor. Made as private to
        /// support Singleton instance
        /// </summary>
        private CacheManager() { }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// Cache instance available
        /// </summary>
        public static ICache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = GetCacheInstance();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Instantiate the configured cache provider
        /// </summary>
        /// <returns>Cache instance created</returns>
        private static ICache GetCacheInstance()
        {
            string cache = ConfigurationManager.AppSettings[CacheConstants.MODE] as string;
            CacheTypeEnum cacheType = CacheTypeEnum.InProcess;
            Enum.TryParse<CacheTypeEnum>(cache, true, out cacheType);
            switch (cacheType)
            {
                case CacheTypeEnum.InProcess: return new InMemoryCache();

                //case CacheTypeEnum.Distributed: return new RedisCache();
                default: return new InMemoryCache();
            }
        }
        #endregion
    }
}
