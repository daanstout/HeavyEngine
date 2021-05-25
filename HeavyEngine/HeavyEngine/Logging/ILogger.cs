using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Logging {
    public interface ILogger {
        ConsoleColor LogColor { get; set; }
        ConsoleColor ErrorColor { get; set; }
        ConsoleColor FatalColor { get; set; }
        ConsoleColor InfoColor { get; set; }
        ConsoleColor WarningColor { get; set; }
        string LogFilePath { get; set; }

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
