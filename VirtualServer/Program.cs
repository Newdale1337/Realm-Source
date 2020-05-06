using System;
using System.Runtime.InteropServices;
using Externals.Database;
using Externals.Resources;
using Externals.Utilities;

namespace VirtualServer
{
    public sealed class Program
    {
        private static SpamProtection SpamProtection { get; set; }
        private static Server VirtualServer { get; set; }

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => LoggingUtils.LogErrorIfDebug(eventArgs.ExceptionObject.ToString());
            WebResources.Load();
            GameResources.Load();

            VirtualServer = new Server();
        }

        private static void ApplyConsoleSettings()
        {
            Console.Title = "Prod - VirtualServer";
        }
    }
}
