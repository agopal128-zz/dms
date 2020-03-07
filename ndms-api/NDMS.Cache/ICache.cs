using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Cache
{
    /// <summary>
    /// Interface which defines various methods to operate on a cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieves a generic item from the cache with the specified key
        /// </summary>
        /// <typeparam name="T">Type of cached object to retrieve</typeparam>
        /// <param name="key">Key of the item</param>
        /// <returns>Object retrieved</returns>
        T GetItem<T>(string key) where T : class;

        /// <summary>
        /// Inserts or updates an item to cache for the default configured time
        /// </summary>
        /// <typeparam name="T">Type of the object to insert or update</typeparam>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Object to add/update to cache</param>
        /// <param name="overwrite">Flag to overwrite or not if already present with the key</param>
        /// <returns>True, if successfully added/updated</returns>
        bool PutItem<T>(string key, object value, bool overwrite = true);

        /// <summary>
        /// Inserts or updates an item to cache for the mentioned time span
        /// </summary>
        /// <typeparam name="T">Type of the object to insert or update</typeparam>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Object to add/update to cache</param>
        /// <param name="timeSpan">Amount of time for which object needs to be persisted</param>
        /// <param name="overwrite">Flag to overwrite or not if already present with the key</param>
        /// <returns>True, if successfully added/updated</returns>
        bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true);

        /// <summary>
        /// Invalidates an item from the cache with the supplied key
        /// </summary>
        /// <typeparam name="T">Type of object which is cached</typeparam>
        /// <param name="key">Key of the object</param>
        /// <returns>True, if operation is succeeded</returns>
        bool Invalidate<T>(string key);

        /// <summary>
        /// Invalid all objects of type T
        /// </summary>
        /// <typeparam name="T">Type of objects which needs to be invalidated</typeparam>
        /// <returns>True, if operation is succeeded</returns>
        bool Invalidate<T>();

        /// <summary>
        /// Invalidate all objects
        /// </summary>
        /// <returns>True, if operation is succeeded</returns>
        bool Invalidate();
    }
}
