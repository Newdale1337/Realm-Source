using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Externals.Models.FirestoreModels;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Database
{
    public class DbListeners
    {
        private static FirestoreDatabase Database { get; set; }

        public static Action<ServerModel> ServerQuit;
        public static Action<ServerModel> ServerStart;

        public static async void Hook(FirestoreDatabase db)
        {
            Database = db;
            Database.CachedServers = new List<ServerModel>();

            ServerStart += model =>
            {
                var server = Database.CachedServers.FirstOrDefault(x => x.IpAddress == model.IpAddress);
                if (server != null) server.Status = true;
            };
            ServerQuit += model =>
            {
                var server = Database.CachedServers.FirstOrDefault(x => x.IpAddress == model.IpAddress);
                if (server != null) server.Status = false;
            };

            Hook("servers", document =>
            {
                var server = document.ConvertTo<ServerModel>();

                if (Database.CachedServers.All(x => x.IpAddress != server.IpAddress))
                    Database.CachedServers.Add(server);

                if (!server.Status)
                    ServerQuit?.Invoke(server);
                if (server.Status)
                    ServerStart?.Invoke(server);
            });
        }

        private static void Hook(string name, Action<DocumentSnapshot> query)
        {
            foreach (var doc in Database.Api.Collection(name).GetSnapshot().Documents)
                doc.Reference.Listen(query);
        }
    }
}