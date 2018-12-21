using System;
using System.IO;

namespace VolumeCorrector
{
    /// <summary>
    /// Static class for simple exception logging.
    /// </summary>
    internal static class ExceptionLogger
    {
        private const string ErrorLogFilePath = ".\\error.log";

        /// <summary>
        /// Logs specified exception to "error.log" file.
        /// </summary>
        /// <param name="ex">Exception to be logged.</param>
        internal static void LogException(Exception ex)
        {
            try
            {
                using (var file = new StreamWriter(ErrorLogFilePath, false))
                {
                    file.WriteLine($"Exception message: {ex.Message}");
                    file.WriteLine("===============================================");
                    file.WriteLine("Call Stack:");
                    file.Write(ex.StackTrace);
                }
            }
            catch (Exception)
            {
                // Suppress all exceptions.
            }
        }
    }
}