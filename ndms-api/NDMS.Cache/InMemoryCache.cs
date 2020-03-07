using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Cache
{
    /// <summary>
    /// Wraps an In-Memory cache by implementing ICache interface
    /// </summary>
    internal class InMemoryCache : ICache
    {
        #region Field(s)
        /// <summary>
        /// Underlying object cache
        /// </summary>
        private ObjectCache privateCache;

        /// <summary>
        /// Lock object to safeguard the thread safe access to the cache
        /// </summary>
        private static Object privateCacheLock = new object();
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InMemoryCache()
        {
            privateCache = MemoryCache.Default;
        }
        #endregion

        #region Private Method(s)
        /// <summary>
        /// Removes all objects whose key starts with the specified
        /// value
        /// </summary>
        /// <param name="keyValue">Key starting value</param>
        private void Flush(string keyValue)
        {
            IEnumerable<string> keys = privateCache.Where(kv => 
            kv.Key.StartsWith(keyValue)).Select(kv => kv.Key);
            Parallel.ForEach(keys, key => privateCache.Remove(key));
        }

        /// <summary>
        /// Returns the default cache policy based on default cache
        /// duration
        /// </summary>
        /// <returns>Default cache policy</returns>
        private CacheItemPolicy GetDefaultCachePolicy()
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(
                CacheUtils.DefaultCacheDuration);
            return defaultCachePolicy;
        }

        /// <summary>
        /// Returns the cache policy based on time span
        /// </summary>
        /// <param name="timeSpan">Time span</param>
        /// <returns>Cache policy</returns>
        private CacheItemPolicy GetCachePolicy(TimeSpan timeSpan)
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.Add(timeSpan);
            return defaultCachePolicy;
        }

        /// <summary>
        /// Adds/Updates an item to the configured cache
        /// </summary>
        /// <param name="key">Key of the object</param>
        /// <param name="value">Object to cache</param>
        /// <param name="policy">Cache policy associated</param>
        /// <param name="overwrite">Flag to say overwrite or not</param>
        /// <returns>True, if successful</returns>
        private bool PutItem(string key, object value, CacheItemPolicy policy, bool overwrite)
        {
            bool retValue = false;
            lock (privateCacheLock)
            {
                if (privateCache.Contains(key) && overwrite)
                {
                    privateCache.Remove(key);
                }
                if (!privateCache.Contains(key))
                {
                    privateCache.Add(key, value, policy);
                    retValue = true;
                }
            }
            return retValue;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves a generic item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">Type of cached object to retrieve</typeparam>
        /// <param name="key">Key of the item</param>
        /// <returns>Object retrieved</returns>
        public T GetItem<T>(string key) where T : class
        {
            return privateCache.Get(CacheUtils.GetCacheKey<T>(key)) as T;
        }

        /// <summary>
        /// Inserts or updates an item to cache for the default configured time
        /// </summary>
        /// <typeparam name="T">Type of the object to insert or update</typeparam>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Object to add/update to cache</param>
        /// <param name="overwrite">Flag to overwrite or not if already present with the key</param>
        /// <returns>True, if successfully added/updated</returns>
        public bool PutItem<T>(string key, object value, bool overwrite = true)
        {
            CacheItemPolicy policy = GetDefaultCachePolicy();
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
        }

        /// <summary>
        /// Inserts or updates an item to cache for the mentioned time span
        /// </summary>
        /// <typeparam name="T">Type of the object to insert or update</typeparam>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Object to add/update to cache</param>
        /// <param name="timeSpan">Amount of time for which object needs to be persisted</param>
        /// <param name="overwrite">Flag to overwrite or not if already present with the key</param>
        /// <returns>True, if successfully added/updated</returns>
        public bool PutItem<T>(string key, object value, TimeSpan timeSpan, 
            bool overwrite = true)
        {
            CacheItemPolicy policy = GetCachePolicy(timeSpan);
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
        }

        /// <summary>
        /// Invalidates an item from the cache with the supplied key
        /// </summary>
        /// <typeparam name="T">Type of object which is cached</typeparam>
        /// <param name="key">Key of the object</param>
        /// <returns>True, if operation is succeeded</returns>
        public bool Invalidate<T>(string key)
        {
            bool returnVal = false;
            key = CacheUtils.GetCacheKey<T>(key);
            lock (privateCacheLock)
            {
                if (privateCache.Contains(key))
                {
                    privateCache.Remove(key);
                    returnVal = true;
                }
            }
            return returnVal;
        }

        /// <summary>
        /// Invalid all objects of type T
        /// </summary>
        /// <typeparam name="T">Type of objects which needs to be invalidated</typeparam>
        /// <returns>True, if operation is succeeded</returns>
        public bool Invalidate<T>()
        {
            bool returnVal = false;
            string keyType = CacheUtils.GetCacheKey<T>();
            lock (privateCacheLock)
            {
                Flush(keyType);
                returnVal = true;
            }
            return returnVal;
        }

        /// <summary>
        /// Invalidate all objects
        /// </summary>
        /// <returns>True, if operation is succeeded</returns>
        public bool Invalidate()
        {
            bool returnVal = false;
            string key = CacheUtils.GetCacheKey();
            lock (privateCacheLock)
            {
                Flush(key);
                returnVal = true;
            }
            return returnVal;
        }
        #endregion
    }
}
