namespace GameServer.Networking.Messaging
{
    public abstract class Message
    {
        public abstract byte MessageId { get; }

        public virtual void Write(MessageWriter wrt) { }
        public virtual void Read(MessageReader rdr) { }
        public virtual void Handle(NetworkClient client) { }

        public const byte FAILURE = 0;
        public const byte HELLO = 1;
        public const byte MAPINFO = 92;
        public const byte LOAD = 57;
        public const byte CREATE = 61;
    }
}