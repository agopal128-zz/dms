using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NDMS.Apis.Models
{
    /// <summary>
    /// Response object which needs to send to the client by every REST API
    /// </summary>
    /// <typeparam name="T">Type of the actual </typeparam>
    public class ApiResponse<T>
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="data">Actual response object</param>
        public ApiResponse(T data):this()
        {
            Data = data;
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// The underlying response object
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Flag which says whether any error occurred 
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// List of errors if any which needs to pass to the client
        /// </summary>
        public List<string> Errors { get; private set; }
        #endregion
    }
}