using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Externals.Database;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class AccountModel : FirestoreModel
    {
        [FirestoreProperty("name")] public string Name { get; set; }
        [FirestoreProperty("account-id")] public int AccountId { get; set; }
        [FirestoreProperty("verified-guid")] public bool VerifiedGuid { get; set; }
        [FirestoreProperty("admin")] public bool Admin { get; set; }
        [FirestoreProperty("mod")] public bool Mod { get; set; }
        [FirestoreProperty("map-creator")] public bool MapCreator { get; set; }
        [FirestoreProperty("creation-date")] public DateTime CreationDate { get; set; }
        [FirestoreProperty("currencies")] public CurrenciesModel Currencies { get; set; }
        [FirestoreProperty("petyard-type")] public int PetYardType { get; set; }
        [FirestoreProperty("character-count")] public int CharacterCount { get; set; }
        [FirestoreProperty("owned-skins")] public List<int> OwnedSkins { get; set; }
        [FirestoreProperty("next-character-id")] public int NextCharacterId { get; set; }
        [FirestoreProperty("character-slot-price")] public int CharacterSlotPrice { get; set; }
        [FirestoreProperty("token")] public string Token { get; set; }
        [FirestoreProperty("campaigns")] public List<CaimpaignsModel> Campaigns { get; set; }
        [FirestoreProperty("class-stats")] public List<ClassStatsModel> ClassStats { get; set; }
        [FirestoreProperty("best-char-fame")] public int BestCharFame { get; set; }
        [FirestoreProperty("ip-addresses")] public List<string> IpAddresses { get; set; }
        [FirestoreProperty("account-lock")] public AccountLockModel AccountLock { get; set; }

        public override DocumentReference GetDocument(FirestoreDb db)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument();
        public override void UpdateFieldAsync(FirestoreDb db, string field, object data)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument().UpdateAsync(field, data);
        public override WriteResult CreateDocument(FirestoreDb db)
            => db.Collection("accounts").Document().Create(this);
        public override void UpdateDocument(FirestoreDb db)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument().SetAsync(this);

        public XElement ToXml()
        {
            var ret = new XElement("Account");

            ret.Add(new XElement("Name", Name));
            ret.Add(new XElement("AccountId", AccountId));

            if (RotmgUtils.GuestNames.All(x => x != Name))
                ret.Add(new XElement("NameChosen"));
            if (Admin)
                ret.Add(new XElement("Admin"));
            if (Mod)
                ret.Add(new XElement("Mod"));
            if (MapCreator)
                ret.Add(new XElement("MapEditor"));
            if (VerifiedGuid)
                ret.Add(new XElement("VerifiedEmail"));

            ret.Add(GetStats());
            ret.Add(new XElement("Credits", Currencies.Credits));
            ret.Add(new XElement("FortuneToken", Currencies.FortuneTokens));
            ret.Add(new XElement("NextCharSlotPrice", CharacterSlotPrice));
            ret.Add(new XElement("IsAgeVerified", 1));

            return ret;
        }

        private XElement GetStats()
        {
            var stats = new XElement("Stats");
            foreach (var cStats in ClassStats)
            {
                stats.Add(new XElement("ClassStats",
                    new XAttribute("objectType", cStats.ObjectType.ToString("X")),
                    new XElement("BestLevel", cStats.BestLevel),
                    new XElement("BestFame", cStats.BestFame)));
            }
            stats.Add(new XElement("BestCharFame", BestCharFame));
            stats.Add(new XElement("TotalFame", Currencies.TotalFame));
            stats.Add(new XElement("Fame", Currencies.Fame));
            return stats;
        }
    }
}