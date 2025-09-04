using System;
using System.IO;

namespace GerenciadorEstoque.Helpers
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logDirectory = "logs")
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            _logFilePath = Path.Combine(logDirectory, "operacoes_estoque.log");
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
        }
    }
}
