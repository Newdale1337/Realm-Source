using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Externals.Models.FirestoreModels;
using Externals.Models.FirestoreModels.Character;
using Externals.Models.XmlModels;
using Externals.Settings;
using Externals.Utilities;
using Google.Cloud.Firestore;
using Grpc.Core;

namespace Externals.Database
{
    public class FirestoreDatabase
    {
        public FirestoreDb Api { get; set; }
        public List<LoginModel> CachedLogins { get; set; }
        public List<AccountModel> CachedAccounts { get; set; }
        public List<ServerModel> CachedServers { get; set; }
        private DbListeners Listeners { get; set; }
        public bool Loaded { get; set; }

        public FirestoreDatabase()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", DatabaseSettings.CREDENTIALS_PATH);
            Api = FirestoreDb.Create(DatabaseSettings.PROJECT_ID).WithWarningLogger(LoggingUtils.LogWarningIfDebug);

            Task.Factory.StartNew(() => DbListeners.Hook(this));
            Task.Factory.StartNew(() => Cache(Api));
        }

        public List<CharacterModel> GetAliveCharacters(int accountId)
        {
            return Api.Collection("characters").DocumentToList<CharacterModel>().Where(x => x.Dead == false && x.AccountId == accountId).ToList();
        }

        public AccountModel Verify(string guid, string password)
        {
            var login = CachedLogins.FirstOrDefault(x => string.Equals(x.Guid, guid, StringComparison.InvariantCultureIgnoreCase));

            if (login == null)
                return null;

            if (!login.Hash.Equals(GenerateHash(password + login.Salt)))
                return null;

            return GetAccountById(login.AccountId);
        }

        protected string GenerateHash(string text) => Convert.ToBase64String(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text)));

        protected AccountModel GetAccountById(int id)
            => Api.Collection("accounts").WhereEqualTo("account-id", id).FirstDocument().GetSnapshot().ConvertTo<AccountModel>();


        private async void Cache(FirestoreDb db)
        {
            CachedLogins = db.Collection("logins").DocumentToList<LoginModel>();
            Loaded = true;
        }
    }
}