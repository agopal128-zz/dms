using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDMS.Cache
{
    /// <summary>
    /// Supported cache types
    /// </summary>
    internal enum CacheTypeEnum
    {
        /// <summary>
        /// Represents a caching method which is in the memory of web server
        /// </summary>
        InProcess,

        /// <summary>
        /// Represents a caching method which is distributed(Independent of web server)
        /// </summary>
        Distributed
    }
}
