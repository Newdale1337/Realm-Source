using System.Runtime.InteropServices;

namespace GameServer.Networking.Messaging.IncomingMessages
{
    public class CreateMessage : Message
    {
        public override byte MessageId => CREATE;

        private int ClassType { get; set; }
        private int SkinType { get; set; }
        private bool IsChallenger { get; set; }

        public override void Read(MessageReader rdr)
        {
            ClassType = rdr.ReadInt16();
            SkinType = rdr.ReadInt16();
            IsChallenger = rdr.ReadBoolean();
        }

        public override void Handle(NetworkClient client)
        {
            var db = client.Database;

            if (client.Account == null)
            {
                client.SendFailure("Account is not available.", -1);
                client.Disconnect();
                return;
            }

            if (client.Account.CharacterCount <= db.GetAliveCharacters(client.Account.AccountId).Count)
            {
                client.SendFailure("Account has no available character slots.", -1);
                client.Disconnect();
                return;
            }

            var chr = db.CreateCharacter(ClassType, SkinType, client.Account.AccountId);
        }
    }
}