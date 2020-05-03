using System;
using System.Collections.Generic;
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
        [FirestoreProperty("admin")] public bool Admin { get; set; }
        [FirestoreProperty("creation-date")] public DateTime CreationDate { get; set; }
        [FirestoreProperty("currencies")] public CurrenciesModel Currencies { get; set; }
        [FirestoreProperty("petyard-type")] public int PetYardType { get; set; }
        [FirestoreProperty("character-count")] public int CharacterCount { get; set; }
        [FirestoreProperty("token")] public string Token { get; set; }
        [FirestoreProperty("security-questions")] public string[] SecurityQuestions { get; set; }
        [FirestoreProperty("campaigns")] public List<CaimpaignsModel> Campaigns { get; set; }
        [FirestoreProperty("class-stats")] public List<ClassStatsModel> ClassStats { get; set; }
        [FirestoreProperty("best-char-fame")] public int BestCharFame { get; set; }
        [FirestoreProperty("ip-addresses")] public List<string> IpAddresses { get; set; }
        [FirestoreProperty("account-lock")] public AccountLockModel AccountLock { get; set; }

        public override DocumentReference GetDocument(FirestoreDb db)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument();
        public override void UpdateFieldAsync(FirestoreDb db, string field, object data)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument().UpdateAsync(field, data);
        public override void CreateDocument(FirestoreDb db)
            => db.Collection("accounts").Document().CreateAsync(this);
        public override void UpdateDocument(FirestoreDb db)
            => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument().SetAsync(this);
    }
}