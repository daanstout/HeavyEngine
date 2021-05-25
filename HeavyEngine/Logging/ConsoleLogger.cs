using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Logging {
    public class ConsoleLogger : ILogger {
        public ConsoleColor LogColor { get; set; } = ConsoleColor.White;
        public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
        public ConsoleColor FatalColor { get; set; } = ConsoleColor.DarkMagenta;
        public ConsoleColor InfoColor { get; set; } = ConsoleColor.Green;
        public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;
        public string LogFilePath { get; set; }

        public void Log(string message) => Log(LogColor, message);
        public void Log(string message, object context) => Log(LogColor, message, context);
        public void LogError(string message) => Log(ErrorColor, $"[ERROR] {message}");
        public void LogError(string message, object context) => Log(ErrorColor, $"[ERROR] {message}", context);
        public void LogFatal(string message) => Log(FatalColor, $"[FATAL ERROR] {message}");
        public void LogFatal(string message, object context) => Log(FatalColor, $"[FATAL ERROR] {message}", context);
        public void LogInfo(string message) => Log(InfoColor, $"[INFO] {message}");
        public void LogInfo(string message, object context) => Log(InfoColor, $"[INFO] {message}", context);
        public void LogWarning(string message) => Log(WarningColor, $"[WARNING] {message}");
        public void LogWarning(string message, object context) => Log(WarningColor, $"[WARNING] {message}", context);

        private void Log(ConsoleColor color, string message) {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        private void Log(ConsoleColor color, string message, object context) {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{context.GetType().Name}] {message}");
        }
    }
}
