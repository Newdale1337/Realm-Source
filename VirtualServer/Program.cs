using System;
using System.Runtime.InteropServices;
using Externals.Database;
using Externals.Resources;

namespace VirtualServer
{
    public sealed class Program
    {
        private static SpamProtection SpamProtection { get; set; }
        private static Server VirtualServer { get; set; }

        public static void Main(string[] args)
        {
            WebResources.Load();
            //GameResources.Load();

            ApplyConsoleSettings();
            VirtualServer = new Server();
        }

        private static void ApplyConsoleSettings()
        {
            Console.Title = "Prod - VirtualServer";
        }
    }
}
