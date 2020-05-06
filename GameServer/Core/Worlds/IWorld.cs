using Externals.Utilities;
using GameServer.Networking.Messaging.OutgoingMessages;

namespace GameServer.Core.Worlds
{
    public interface IWorld
    {
        int Width { get; set; }
        int Height { get; set; }
        string Name { get; set; }
        bool AllowTeleport { get; set; }
        bool AllowDeath { get; set; }
        bool AllowTrade { get; set; }
        string MusicUrl { get; set; }

        MapInfoMessage GetMapInfo(out ClientRandom random);
        void LoadWorld(string path);
    }
}