using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Externals.Models.FirestoreModels;
using Externals.Settings;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Database
{
    public class FirestoreDatabase
    {
        public FirestoreDb Api { get; set; }
        public List<LoginModel> CachedLogins { get; set; }
        public List<AccountModel> CachedAccounts { get; set; }
        public bool Loaded { get; set; }

        public FirestoreDatabase()
        {
            LoggingUtils.LogStopwatchIfDebug("Database loaded fully loaded", () =>
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", DatabaseSettings.CREDENTIALS_PATH);
                LoggingUtils.LogIfDebug($"Database authentication completed");
                Task.Factory.StartNew(() => Cache(Api = FirestoreDb.Create(DatabaseSettings.PROJECT_ID).WithWarningLogger(LoggingUtils.LogWarningIfDebug)));
            });
        }

        private async void Cache(FirestoreDb db)
        {
            LoggingUtils.LogStopwatchIfDebug("Caching database collections done (Logins, Accounts)", () =>
            {
                CachedAccounts = db.Collection("accounts").DocumentToList<AccountModel>();
                CachedLogins = db.Collection("logins").DocumentToList<LoginModel>();
                Loaded = true;
            });
        }
    }
}