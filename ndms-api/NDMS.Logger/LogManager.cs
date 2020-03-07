using System.Configuration;

namespace NDMS.Logger
{
    /// <summary>
    /// Log manager class which returns the configured logger implementation
    /// </summary>
    public class LogManager
    {
        private static ILogger instance = null;
        private static object instanceLock = new object();

        /// <summary>
        /// Private constructor
        /// </summary>
        private LogManager(){ }

        /// <summary>
        /// Returns the logger instance
        /// </summary>
        public static ILogger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if(instance == null)
                        {
                            instance = GetLoggerInstance();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Actually creates currently configured logger instance
        /// </summary>
        /// <returns>Logger instance</returns>
        private static ILogger GetLoggerInstance()
        {
            string provider = ConfigurationManager.AppSettings[LogConstants.PROVIDER] as string;        
            switch(provider)
            {
                case "log4net":
                    return new Log4NetLogger();
                default:
                    return new Log4NetLogger();
            }
        }
    }
}
