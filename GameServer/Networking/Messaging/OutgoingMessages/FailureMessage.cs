namespace GameServer.Networking.Messaging.OutgoingMessages
{
    public class FailureMessage : Message
    {
        public override byte MessageId => FAILURE;

        public const int INCORRECT_VERSION = 4;
        public const int BAD_KEY = 5;
        public const int INVALID_TELEPORT_TARGET = 6;
        public const int EMAIL_VERIFICATION_NEEDED = 7;
        public const int TELEPORT_REALM_BLOCK = 9;
        public const int WRONG_SERVER_ENTERED = 10;

        public string ErrorDescription { get; set; }
        public int ErrorId { get; set; }

        public FailureMessage(string description, int errorId)
        {
            ErrorDescription = description;
            ErrorId = errorId;
        }

        public override void Write(MessageWriter wrt)
        {
            wrt.Write(ErrorId);
            wrt.WriteUTF(ErrorDescription);
        }
    }
}