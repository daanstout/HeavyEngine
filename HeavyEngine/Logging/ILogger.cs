namespace HeavyEngine.Logging {
    public interface ILogger {
        void Log(string message);
        void Log(string message, object context);
        void LogInfo(string message);
        void LogInfo(string message, object context);
        void LogWarning(string message);
        void LogWarning(string message, object context);
        void LogError(string message);
        void LogError(string message, object context);
        void LogFatal(string message);
        void LogFatal(string message, object context);
    }
}
