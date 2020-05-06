using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Externals.Database;
using Externals.Settings;
using Externals.Utilities;

namespace GameServer.Networking
{
    public class ConnectionEstablisher
    {
        private IPAddress IpAddress { get; set; }
        private int Port { get; set; }
        private Socket Socket { get; set; }
        private ManualResetEvent WaitEvent { get; set; }
        private GameServerDatabase Database { get; set; }
        private CoreManager Manager { get; set; }
        public bool Unloading { get; set; }


        public ConnectionEstablisher(string[] args, ManualResetEvent waitEvent)
        {
            SetListeningConfigs(args);
            Database = new GameServerDatabase();
            Manager = new CoreManager(Database);
            WaitEvent = waitEvent;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.NoDelay = true;
            Socket.Bind(new IPEndPoint(IpAddress, Port));
            Socket.Listen(0x100);

            //0.0.0.0 means any.
            LoggingUtils.LogIfDebug($"GameServer listening at Ip {IpAddress} and Port {Port}");
        }

        private void SetListeningConfigs(string[] args)
        {
            if (args.Length > 1)
            {
                IpAddress = IPAddress.Parse(args[0]);

                if (!int.TryParse(args[1], out int port))
                    Port = GameServerSettings.Port;
                Port = port;
                return;
            }

            IpAddress = IPAddress.Any;
            Port = GameServerSettings.Port;
        }

        public void Start()
        {
            while (!Unloading)
            {
                try
                {
                    Socket.BeginAccept(OnConnection, null);
                }
                catch (Exception e)
                {
                    LoggingUtils.LogErrorIfDebug(e.ToString());
                }
                finally
                {
                    WaitEvent.WaitOne();
                }
            }
        }

        private void OnConnection(IAsyncResult ar)
        {
            if (Unloading) return;
            WaitEvent.Set();
            var client = Socket.EndAccept(ar);
            WaitEvent.Reset();

            new NetworkClient(client, Database, Manager);
        }

        public void Unload()
        {
            Unloading = true;
            WaitEvent = null;
            IpAddress = null;
            Socket.Dispose();
            Manager.Unload();

            LoggingUtils.LogIfDebug("ConnectionEstablisher has been unloaded.");
        }
    }
}