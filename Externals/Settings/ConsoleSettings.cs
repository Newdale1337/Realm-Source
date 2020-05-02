using System;
using System.Reflection;

namespace Externals.Settings
{
    /// <summary>
    /// Todo...
    /// </summary>
    public class ConsoleSettings
    {
        public static string Title = Console.Title;

        public static void ApplySettings()
        {
            Console.Title = Title;
        }
    }
}