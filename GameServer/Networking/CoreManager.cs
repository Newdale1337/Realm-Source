using System.Collections.Generic;
using System.Reflection;
using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Settings;
using Externals.Utilities;
using GameServer.Core.Worlds;

namespace GameServer.Networking
{
    public class CoreManager
    {
        private List<NetworkClient> ActiveUsers { get; set; }
        public int ActiveUsersCount { get => ActiveUsers?.Count ?? -1;}
        private object LockObject = new object();
        private ServerModel ServerModel { get; set; }
        private GameServerDatabase Database { get; set; }
        public WorldManager WorldManager { get; set; }

        public CoreManager(GameServerDatabase db)
        {
            Database = db;
            SetupServerModel();
            ActiveUsers = new List<NetworkClient>();
            WorldManager = new WorldManager();
        }

        public IWorld GetWorld(int id) => WorldManager.GetWorld(id);

        private void SetupServerModel()
        {
            ServerModel = new ServerModel() { IpAddress = GameServerSettings.IpAddress };
            ServerModel = ServerModel.GetDocument(Database.Api).GetSnapshot().ConvertTo<ServerModel>();
            if (ServerModel != null)
            {
                ServerModel.Status = true;
                ServerModel.UpdateFieldAsync(Database.Api, "status", true);
                return;
            }

            ServerModel = new ServerModel { IpAddress = GameServerSettings.IpAddress, Lat = 0, Long = 0, Name = GameServerSettings.Name, Status = true, Usage = 0 };
            ServerModel.CreateDocument(Database.Api);
        }

        public void AddClient(NetworkClient client)
        {
            lock (LockObject)
                ActiveUsers.Add(client);
            LoggingUtils.LogIfDebug($"Adding new user named {client.Account.Name}");
        }

        public void RemoveClient(NetworkClient client)
        {
            lock (LockObject)
                ActiveUsers.Remove(client);
        }

        public void Unload()
        {
            ServerModel.UpdateFieldAsync(Database.Api, "status", false);
            lock (ActiveUsers)
                for (int i = 0; i < ActiveUsers.Count; i++)
                {
                    ActiveUsers[i].Unload();
                }
        }
    }
}