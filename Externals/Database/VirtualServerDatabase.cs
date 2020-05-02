using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Externals.Database.Returnables;
using Externals.Models.FirestoreModels;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Database
{
    public class VirtualServerDatabase : IDisposable
    {
        public FirestoreDb DatabaseApi { get; set; }
        private Random Random { get; set; }
        private List<AccountModel> CachedAccounts { get; set; }
        private List<LoginModel> CachedLogins { get; set; }

        public VirtualServerDatabase(FirestoreDatabase db)
        {
            DatabaseApi = db.Api;
            Random = new Random(Environment.TickCount);

            CachedAccounts = db.CachedAccounts;
            CachedLogins = db.CachedLogins;

            LoggingUtils.LogIfDebug("New VirtualServerDatabase created.");
        }

        public AccountModel Verify(string guid, string password)
        {
            var login = CachedLogins.FirstOrDefault(x => string.Equals(x.Guid, guid, StringComparison.InvariantCultureIgnoreCase));

            if (login == null)
                return null;

            return new AccountModel();
        }

        public string Register(string guid, string password, string name)
        {
            if (CachedLogins.Any(x => string.Equals(x.Guid, guid, StringComparison.InvariantCultureIgnoreCase)))
                return RegisterReturns.ACCOUNT_EXISTS;

            if (CachedLogins.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)))
                return RegisterReturns.NAME_EXISTS;

            if (!Regex.Match(name, "^[a-zA-Z]+$").Success)
                return RegisterReturns.NAME_EXISTS;

            int accountId = GenerateAccountId(CachedLogins);


            CreateAndSendLoginModel(guid, password, name, accountId);
            CreateAndSendAccountModel(accountId, name);

            LoggingUtils.LogIfDebug($"Creating new account {{{guid}}}");

            return RegisterReturns.SUCCESS;
        }

        private void CreateAndSendAccountModel(int accountId, string name)
        {
            var currencies = new CurrenciesModel();
            currencies.Credits = 40;
            currencies.Fame = 0;
            currencies.TotalFame = 0;
            currencies.FortuneTokens = 0;
            var account = new AccountModel
            {
                AccountId = accountId,
                Currencies = currencies,
                CreationDate = DateTime.UtcNow,
                Name = name,
                ClassStats = new List<ClassStatsModel>(),
                Admin = false,
                PetyardType = 1,
                Token = Path.GetRandomFileName(),
                BestCharFame = 0,
                Campaigns = new CaimpaignsModel[0],
                SecurityQuestions = new string[3]
            };

            account.CreateDocument(DatabaseApi);
        }

        private void CreateAndSendLoginModel(string guid, string password, string name, int accountId)
        {
            string salt = Path.GetRandomFileName();

            var login = new LoginModel()
            {
                AccountId = accountId,
                Salt = Path.GetRandomFileName(),
                Hash = GenerateHash(password + salt),
                CreationDate = DateTime.UtcNow.ToString(CultureInfo.CurrentCulture),
                Name = name,
                Guid = guid
            };

            login.CreateDocument(DatabaseApi);
            CachedLogins.Add(login);
        }

        private int GenerateAccountId(List<LoginModel> logins)
        {
            int accountId = -1;
            do
            {
                int nextId = Random.Next(int.MaxValue);
                if (logins.All(x => x.AccountId != nextId))
                    accountId = nextId;
            } while (accountId == -1);

            return accountId;
        }
        private string GenerateHash(string text) => Convert.ToBase64String(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text)));

        public void Dispose()
        {
            DatabaseApi = null;
        }
    }
}