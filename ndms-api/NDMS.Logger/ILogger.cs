using System;

namespace NDMS.Logger
{
    public interface ILogger
    {
        /// <summary>
        /// Logs a text message with particular log level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logLevel">Desired log level</param>
        void Log(string message, LogLevelEnum logLevel);

        /// <summary>
        /// Logs an exception with a message and log level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="logLevel">Desired log level</param>
        void Log(string message, Exception ex, LogLevelEnum logLevel);
    }    
}
