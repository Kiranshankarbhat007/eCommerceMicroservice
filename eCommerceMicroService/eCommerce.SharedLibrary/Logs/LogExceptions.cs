using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Logs
{
    public static class LogExceptions
    {
        public static void LogException(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebug(ex.Message);
        }

        private static void LogToFile(string message) => Log.Information(message);

        private static void LogToConsole(string message) => Log.Warning(message);

        private static void LogToDebug(string message) => Log.Debug(message);
    }
}
