using System;
using System.Diagnostics;

namespace Externals.Utilities
{
    public class LoggingUtils
    {
        private static object lockObj = new object();
        public static bool DISPLAY_TIME { get; set; } = true;

        /// <summary>
        /// Determines whether to display the time.
        /// </summary>
        /// <returns>Datetime string.</returns>
        private static string DisplayTimeString() => DISPLAY_TIME ? $"[{DateTime.UtcNow}] " : string.Empty;

        /// <summary>
        /// Logs to the console if game is in debug mode.
        /// </summary>
        /// <param name="message"></param>
        public static void LogIfDebug(string message)
        {
#if DEBUG
            lock (lockObj)
                Console.WriteLine($"{DisplayTimeString()}{message}");
#endif
        }

        /// <summary>
        /// Logs to the console if game is in debug mode in the color red.
        /// </summary>
        /// <param name="message"></param>
        public static void LogErrorIfDebug(string message)
        {
#if DEBUG
            lock (lockObj)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{DisplayTimeString()}{message}");
                Console.ResetColor();
            }
#endif
        }

        /// <summary>
        /// Logs to the console if game is in debug mode in the color red.
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarningIfDebug(string message)
        {
#if DEBUG
            lock (lockObj)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DisplayTimeString()}{message}");
                Console.ResetColor();
            }
#endif
        }

        /// <summary>
        /// Runs an action and prints the time it took.
        /// </summary>
        /// <param name="action">The method to run.</param>
        public static void LogStopwatchIfDebug(string msg, Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            LogIfDebug($"{msg} {{{sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms}}");
        }
    }
}