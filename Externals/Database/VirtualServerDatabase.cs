using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Externals.Constants;
using Externals.Database.Returnables;
using Externals.Models.FirestoreModels;
using Externals.Models.GameDataModels;
using Externals.Resources;
using Externals.Settings;
using Externals.Utilities;

namespace Externals.Database
{
    public class VirtualServerDatabase : FirestoreDatabase, IDisposable
    {
        private Random Random { get; set; }
        private List<int> Skins { get;}
        private List<ClassStatsModel> NewbieStats { get;}

        public VirtualServerDatabase()
        {
            Random = new Random(Environment.TickCount);
            Skins = GetSkins();
            NewbieStats = GetNewbieClassStats();
        }

        public AccountModel GetGuestAccount()
        {
            return new AccountModel()
            {
                AccountId = -1,
                Name = RotmgUtils.GuestNames[Random.Next(RotmgUtils.GuestNames.Length)],
                Admin = false,
                CreationDate = DateTime.UtcNow,
                Currencies = new CurrenciesModel()
                {
                    Fame = 0,
                    FortuneTokens = 0,
                    Credits = 0,
                    TotalFame = 0
                },
                Token = string.Empty,
                PetYardType = PetYardTypes.Common,
                CharacterCount = 1,
                BestCharFame = 0,
                ClassStats = NewbieStats,
                Campaigns = new List<CaimpaignsModel>(),
                AccountLock = new AccountLockModel { AccountInUse = false, },
                IpAddresses = new List<string>()
            };
        }

        public string Register(string guid, string password, string name, string ip)
        {
            if (CachedLogins.Any(x => string.Equals(x.Guid, guid, StringComparison.InvariantCultureIgnoreCase)))
                return RegisterReturns.ACCOUNT_EXISTS;

            if (CachedLogins.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)))
                return RegisterReturns.NAME_EXISTS;

            if (!Regex.Match(name, "^[a-zA-Z]+$").Success)
                return RegisterReturns.NAME_EXISTS;

            int accountId = GenerateAccountId(CachedLogins);


            CreateAndSendLoginModel(guid, password, name, accountId);
            AccountModel acc = CreateAndSendAccountModel(ip, accountId, name);

            LoggingUtils.LogIfDebug(acc.Token);
            LoggingUtils.LogIfDebug($"Creating new account {{ {guid} }}");

            return acc.Token;
        }

        private List<int> GetSkins()
        {
            var ret = new List<int>();

            foreach (var skin in ObjectLibrary.Properties.Values.Where(x => x.Skin))
                ret.Add(skin.Type);

            return ret;
        }

        private AccountModel CreateAndSendAccountModel(string ip, int accountId, string name)
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
                Admin = false,
                BestCharFame = 0,
                CharacterCount = 2,
                CharacterSlotPrice = 600,
                Mod = false,
                MapCreator = false,
                VerifiedGuid = !MutualServerSettings.NeedsGuidVerification,
                NextCharacterId = 0,
                OwnedSkins = Skins,
                ClassStats = NewbieStats,
                PetYardType = PetYardTypes.Common,
                Token = StringUtils.GetRandomString(40),
                Campaigns = new List<CaimpaignsModel>(),
                IpAddresses = new List<string> { ip },
                AccountLock = new AccountLockModel
                {
                    AccountInUse = false,
                    AcquiredLock = DateTime.UtcNow
                },
            };

            account.CreateDocument(Api);

            return account;
        }

        private List<ClassStatsModel> GetNewbieClassStats()
        {
            var stats = new List<ClassStatsModel>();

            foreach (var prop in ObjectLibrary.Properties.Values.Where(x => x.Player))
                stats.Add(new ClassStatsModel { BestFame = 0, BestLevel = 0, ObjectType = prop.Type });

            return stats;
        }

        private void CreateAndSendLoginModel(string guid, string password, string name, int accountId)
        {
            string salt = Path.GetRandomFileName();

            var login = new LoginModel()
            {
                AccountId = accountId,
                Salt = salt,
                Hash = GenerateHash(password + salt),
                CreationDate = DateTime.UtcNow.ToString(CultureInfo.CurrentCulture),
                Name = name,
                Guid = guid
            };

            login.CreateDocument(Api);
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

        public void Dispose()
        {
            Random = null;

            LoggingUtils.LogIfDebug("VirtualServerDatabase disposed.");
        }
    }
}