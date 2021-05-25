using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeavyEngine.Logging {
    public class FileLogger : ILogger {
        public ConsoleColor LogColor { get; set;}
        public ConsoleColor ErrorColor { get; set; }
        public ConsoleColor FatalColor { get; set; }
        public ConsoleColor InfoColor { get; set; }
        public ConsoleColor WarningColor { get; set; }
        public string LogFilePath { get; set; }

        public void Log(string message) => Append(message);
        public void Log(string message, object context) => Append(message, context);
        public void LogError(string message) => Append($"[ERROR] {message}");
        public void LogError(string message, object context) => Append($"[ERROR] {message}", context);
        public void LogFatal(string message) => Append($"[FATAL ERROR] {message}");
        public void LogFatal(string message, object context) => Append($"[FATAL ERROR] {message}", context);
        public void LogInfo(string message) => Append($"[INFO] {message}");
        public void LogInfo(string message, object context) => Append($"[INFO] {message}", context);
        public void LogWarning(string message) => Append($"[WARNING] {message}");
        public void LogWarning(string message, object context) => Append($"[WARNING] {message}", context);

        private void Append(string message) {
            using var sw = File.AppendText(LogFilePath);

            sw.WriteLine(message);
        }

        private void Append(string messge, object context) {
            using var sw = File.AppendText(LogFilePath);

            sw.WriteLine($"[{context.GetType().Name}] {messge}");
        }
    }
}
