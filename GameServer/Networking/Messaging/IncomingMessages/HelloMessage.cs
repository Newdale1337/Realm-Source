using System;
using Externals.Cryptography;
using Externals.Models.FirestoreModels;
using Externals.Settings;
using Externals.Utilities;
using GameServer.Networking.Messaging.OutgoingMessages;

namespace GameServer.Networking.Messaging.IncomingMessages
{
    public class HelloMessage : Message
    {
        private string BuildVersion { get; set; }
        private int GameId { get; set; }
        private string Guid { get; set; }
        private string Password { get; set; }
        private int KeyTime { get; set; }
        private byte[] Key { get; set; }
        private string MapJson { get; set; }

        public override byte MessageId => HELLO;

        public override void Read(MessageReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            Guid = RSA.Instance.Decrypt(rdr.ReadUTF());
            Password = RSA.Instance.Decrypt(rdr.ReadUTF());
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapJson = rdr.ReadUTF32();
        }

        public override void Handle(NetworkClient client)
        {
            AccountModel acc = null;
            var db = client.Database;

            if (BuildVersion != GameServerSettings.BuildVersion)
            {
                client.SendFailure("Incorrect build.", FailureMessage.INCORRECT_VERSION);
                client.Disconnect("Client connecting with wrong build.");
                return;
            }

            if ((acc = db.Verify(Guid, Password)) == null)
            {
                client.SendFailure("Account not found.", FailureMessage.BAD_KEY);
                client.Disconnect("Client connecting to server with non existing account.");
                return;
            }

            if (client.Manager.ActiveUsersCount >= GameServerSettings.MaxClients )
            {
                client.SendFailure("Server is full.", -1);
                client.Disconnect();
                return;
            }

            var map = client.Manager.GetWorld(GameId);
            if (map == null)
            {
                client.SendFailure("Invalid world", FailureMessage.TELEPORT_REALM_BLOCK);
                client.Disconnect("Invalid world target.");
                return;
            }

            client.Account = acc;
            client.Manager.AddClient(client);

            var mapInfo = map.GetMapInfo(out ClientRandom random);
            client.ClientRandom = random;

            client.SendMessage(mapInfo);
        }
    }
}