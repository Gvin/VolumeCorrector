using System;
using System.IO;

namespace VolumeCorrector
{
    /// <summary>
    /// Static class for simple exception logging.
    /// </summary>
    internal static class ExceptionLogger
    {
        private const string ErrorLogFolder = ".\\Logs\\";
        private const string ErrorLogFileNameTemplate = "error_{0}.log";
        private const string DateFormat = @"dd-MM-yyyy HH-mm-ss";

        /// <summary>
        /// Clears logs folder removing all log files.
        /// </summary>
        internal static void ClearLogs()
        {
            try
            {
                if (Directory.Exists(ErrorLogFolder))
                {
                    Directory.Delete(ErrorLogFolder, true);
                    Directory.CreateDirectory(ErrorLogFolder);
                }
            }
            catch (Exception)
            {
                // Suppress all exceptions.
            }

        }

        /// <summary>
        /// Logs specified exception to "error_TIME-STAMP.log" file.
        /// </summary>
        /// <param name="ex">Exception to be logged.</param>
        internal static void LogException(Exception ex)
        {
            if (!Directory.Exists(ErrorLogFolder))
            {
                Directory.CreateDirectory(ErrorLogFolder);
            }
            var logPath = Path.Combine(ErrorLogFolder, GenerateLogFileName());
            WriteExceptionToFile(logPath, ex);
        }

        private static string GenerateLogFileName()
        {
            return string.Format(ErrorLogFileNameTemplate, DateTime.Now.ToString(DateFormat));
        }

        private static void WriteExceptionToFile(string filePath, Exception exception)
        {
            try
            {
                using (var file = new StreamWriter(filePath, false))
                {
                    file.WriteLine($"Exception message: {exception.Message}");
                    file.WriteLine("===============================================");
                    file.WriteLine("Call Stack:");
                    file.Write(exception.StackTrace);
                }
            }
            catch (Exception)
            {
                // Suppress all exceptions.
            }
        }
    }
}