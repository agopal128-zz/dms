using System;
using log4net;

namespace NDMS.Logger
{
    internal class Log4NetLogger : ILogger
    {
        /// <summary>
        /// Log4net logger instance
        /// </summary>
        protected readonly ILog log4netLogger;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Log4NetLogger()
        { 
            log4net.Config.XmlConfigurator.Configure();
            log4netLogger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        }

        /// <summary>
        /// Logs a text message with particular log level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logLevel">Desired log level</param>
        public void Log(string message, LogLevelEnum logLevel)
        {
            switch(logLevel)
            {
                case LogLevelEnum.Debug: log4netLogger.Debug(message); break;
                case LogLevelEnum.Information: log4netLogger.Info(message); break;
                case LogLevelEnum.Warning: log4netLogger.Warn(message); break;
                case LogLevelEnum.Error: log4netLogger.Error(message); break;
                case LogLevelEnum.Fatal: log4netLogger.Fatal(message); break;
            }
        }

        /// <summary>
        /// Logs an exception with a message and log level
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="logLevel">Desired log level</param>
        public void Log(string message, Exception ex, LogLevelEnum logLevel)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Debug: log4netLogger.Debug(message, ex); break;
                case LogLevelEnum.Information: log4netLogger.Info(message, ex); break;
                case LogLevelEnum.Warning: log4netLogger.Warn(message, ex); break;
                case LogLevelEnum.Error: log4netLogger.Error(message, ex); break;
                case LogLevelEnum.Fatal: log4netLogger.Fatal(message, ex); break;
            }
        }
    }
}
