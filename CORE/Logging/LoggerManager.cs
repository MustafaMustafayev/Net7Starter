using WatchDog;

namespace CORE.Logging;

public class LoggerManager : ILoggerManager
{
    public void LogDebug(string message)
    {
        WatchLogger.Log(message, "Debug");
    }

    public void LogError(string message)
    {
        WatchLogger.LogError(message);
    }

    public void LogInfo(string message)
    {
        WatchLogger.Log(message);
    }

    public void LogWarn(string message)
    {
        WatchLogger.LogWarning(message);
    }
}