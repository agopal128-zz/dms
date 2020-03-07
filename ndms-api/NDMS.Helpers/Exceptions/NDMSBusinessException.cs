using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NDMS.Helpers
{
    /// <summary>
    /// Exception which will be thrown when business layer fails to
    /// perform the requested operation
    /// </summary>
    [Serializable]
    public class NDMSBusinessException : Exception, ISerializable
    {
        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public NDMSBusinessException()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="message">Error message inside exception</param>
        public NDMSBusinessException(string message) : base(message)
        {
            this.Errors = new List<string>();
            Errors.Add(message);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="errors">list of errors</param>
        public NDMSBusinessException(ICollection<string> errors) : base()
        {
            Errors = errors;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="message">Error message inside exception</param>
        /// <param name="innerException">Inner exception, which is the actual cause</param>
        public NDMSBusinessException(string message, Exception innerException) :
            base(message, innerException)
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Constructor for de-serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="streamingContext">Streaming context</param>
        protected NDMSBusinessException(SerializationInfo info, StreamingContext streamingContext)
            : base(info, streamingContext)
        {
            this.Errors = new List<string>();
        }
        #endregion

        #region Properties(s)
        /// <summary>
        /// Collection of custom error messages
        /// </summary>
        public ICollection<string> Errors { get; private set; }
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
