using NLog;
using System.IO;

namespace Io.HcxProtocol.Utils
{
    public class NlogUtils
    {
        public static void SetNlogConfig(string logFilePath, string logFileName)
        {
            if (NLog.LogManager.Configuration == null)
            {
                var config = new NLog.Config.LoggingConfiguration();

                var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
                config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);

                if (!(string.IsNullOrWhiteSpace(logFilePath) || string.IsNullOrWhiteSpace(logFileName)))
                {
                    var logfile = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(logFilePath, logFileName + ".log") };
                    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
                }

                NLog.LogManager.Configuration = config;
            }
        }

    }
}
