using System;
using System.Runtime.Serialization;

namespace NDMS.DataAccess
{
    /// <summary>
    /// Exception which will be thrown when Data Access layer fails to
    /// perform the requested operation
    /// </summary>
    [Serializable]
    public class DataAccessException : Exception, ISerializable
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessException()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="message">Error message inside exception</param>
        public DataAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="message">Error message inside exception</param>
        /// <param name="innerException">Inner exception, which is the actual cause</param>
        public DataAccessException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor for de-serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="streamingContext">Streaming context</param>
        protected DataAccessException(SerializationInfo info, StreamingContext streamingContext)
            : base(info, streamingContext)
        {
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// The method for serialization support
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion
    }
}
