using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Externals;
using Externals.Resources;
using Externals.Utilities;
using GameServer.Networking;

namespace GameServer
{
    class Program
    {
        private static SafeClose SafeClose { get; set; }
        private static ConnectionEstablisher ConnectionEstablisher { get; set; }
        private static ManualResetEvent WaitEvent { get; set; }
        static void Main(string[] args)
        {
            GameResources.Load();
            WaitEvent = new ManualResetEvent(false);
            SafeClose = new SafeClose(Unload,WaitEvent);
            ConnectionEstablisher = new ConnectionEstablisher(args, WaitEvent);
            ConnectionEstablisher.Start();
        }

        private static void Unload()
        {
            GameResources.Unload();
            ConnectionEstablisher.Unload();

            LoggingUtils.LogIfDebug("GameServer has been unloaded.");
        }
    }
}
