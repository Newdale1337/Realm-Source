using System;
using Externals.Utilities;

namespace GameServer.Networking.Messaging.OutgoingMessages
{
    public class MapInfoMessage : Message
    {
        public int Width { get; }
        public int Height { get; }
        public string Name { get; }
        /// <summary>
        /// Needs to be set before writing to packet.
        /// </summary>
        public uint Seed { get; set; }
        public int Background { get; }
        public int Difficulty { get; }
        public bool AllowTp { get; }
        public bool ShowDisplays { get; }
        public override byte MessageId => MAPINFO;

        public MapInfoMessage(int width, int height, string name, int background, int difficulty, bool allowTp, bool showDisplays)
        {
            Width = width;
            Height = height;
            Name = name;
            Background = background;
            Difficulty = difficulty;
            AllowTp = allowTp;
            ShowDisplays = showDisplays;
        }

        public override void Write(MessageWriter wrt)
        {
            wrt.Write(Width);
            wrt.Write(Height);
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Name);
            wrt.WriteUTF(Name);
            wrt.Write(Seed);
            wrt.Write(Background);
            wrt.Write(Difficulty);
            wrt.Write(AllowTp);
            wrt.Write(ShowDisplays);
            wrt.Write((short)0);
            wrt.Write((short)0);
        }
    }
}