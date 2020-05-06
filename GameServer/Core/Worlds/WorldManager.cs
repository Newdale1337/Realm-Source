using System.Collections.Generic;
using Externals.Utilities;

namespace GameServer.Core.Worlds
{
    public class WorldManager
    {
        public const int NEXUS = -2;

        private Dictionary<int, IWorld> Worlds { get; set; }

        public WorldManager()
        {
            LoadWorldsAsync();
        }

        public async void LoadWorldsAsync()
        {
            Worlds = new Dictionary<int, IWorld>();
            Worlds.Add(-2, new Nexus());
        }

        public IWorld GetWorld(int id)
        {
            if (!Worlds.TryGetValue(id, out IWorld world))
            {
                LoggingUtils.LogWarningIfDebug("User is trying to access a world that does not exist.");
                return null;
            }

            return world;
        }
    }
}