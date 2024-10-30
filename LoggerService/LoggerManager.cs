using Contracts.Logger;
using Entities.Enums;
using NLog;

namespace LoggerService;

public class LoggerManager() : ILoggerManager
{
    //TODO Create seperate configuration for dev logging and prod logging.

    private static ILogger logger = LogManager.GetCurrentClassLogger();

    public void LogTrace(string message) => logger.Trace(message);

    public void LogDebug(string message) => logger.Debug(message);

    public void LogInfo(string message) => logger.Info(message);

    public void LogWarning(string message) => logger.Warn(message);

    public void LogError(string message) => logger.Error(message);

    public void LogFatal(string message) => logger.Fatal(message);

    public void Log(string message, LogLevelEnum level)
    {
        switch (level)
        {
            case LogLevelEnum.Trace:
                LogTrace(message);
                break;
            case LogLevelEnum.Debug:
                LogDebug(message);
                break;
            case LogLevelEnum.Info:
                LogInfo(message);
                break;
            case LogLevelEnum.Warning:
                LogWarning(message);
                break;
            case LogLevelEnum.Error:
                LogError(message);
                break;
            case LogLevelEnum.Fatal:
                LogFatal(message);
                break;
        }
    }

}
