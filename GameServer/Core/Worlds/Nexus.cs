using System;
using Externals.Utilities;
using GameServer.Core.Objects;
using GameServer.Networking.Messaging.OutgoingMessages;

namespace GameServer.Core.Worlds
{
    public class Nexus : IWorld
    {
        public Nexus()
        {
            LoadWorld("Resources/Game/Worlds/Nexus.jm");
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public bool AllowTeleport { get; set; }
        public bool AllowDeath { get; set; }
        public bool AllowTrade { get; set; }
        public string MusicUrl { get; set; }

        private MapInfoMessage MapInfoMessage { get; set; }

        public void LoadWorld(string path)
        {
            var parser = new MapParser(path);
            Name = "Nexus";
            Width = parser.Width;
            Height = parser.Height;
            AllowDeath = false;
            AllowTeleport = false;
            

            MapInfoMessage = new MapInfoMessage(Width, Height, Name, 0, 0, AllowTeleport, true);
        }

        public MapInfoMessage GetMapInfo(out ClientRandom random)
        {
            random = new ClientRandom((uint)Environment.TickCount);
            MapInfoMessage.Seed = random.Seed;

            return MapInfoMessage;
        }
    }
}