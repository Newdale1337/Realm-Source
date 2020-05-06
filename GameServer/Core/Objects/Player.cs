using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace GameServer.Core.Objects
{
    public sealed partial class Player : BaseObject
    {
        public AccountModel Account { get; set; }

        public Player(int objType) : base(objType)
        {
            
        }

        public void Unload(FirestoreDatabase db)
        {
            Account?.UpdateDocument(db.Api);
            Account = null;

            LoggingUtils.LogIfDebug("Player has been unloaded.");
        }
    }
}