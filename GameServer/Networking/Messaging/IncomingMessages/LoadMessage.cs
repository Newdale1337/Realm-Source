namespace GameServer.Networking.Messaging.IncomingMessages
{
    public class LoadMessage : Message
    {
        public override byte MessageId => LOAD;

        private int CharId { get; set; }
        private bool IsArena { get; set; }
        private bool IsChallenger { get; set; }

        public override void Read(MessageReader rdr)
        {
            CharId = rdr.ReadInt32();
            IsArena = rdr.ReadBoolean();
            IsChallenger = rdr.ReadBoolean();
        }

        public override void Handle(NetworkClient client)
        {
            var db = client.Database;

            if (client.Account != null)
            {
                
            }
        }
    }
}