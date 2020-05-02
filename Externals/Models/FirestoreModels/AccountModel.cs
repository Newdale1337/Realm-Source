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
        [FirestoreProperty("accountId")] public int AccountId { get; set; }
        [FirestoreProperty("admin")] public bool Admin { get; set; }
        [FirestoreProperty("creation-date")] public DateTime CreationDate { get; set; }
        [FirestoreProperty("currencies")] public CurrenciesModel Currencies { get; set; }
        [FirestoreProperty("petyard-type")] public int PetyardType { get; set; }
        [FirestoreProperty("token")] public string Token { get; set; }
        [FirestoreProperty("security-questions")] public string[] SecurityQuestions { get; set; }
        [FirestoreProperty("campaigns")] public CaimpaignsModel[] Campaigns { get; set; }
        [FirestoreProperty("class-stats")] public List<ClassStatsModel> ClassStats { get; set; }
        [FirestoreProperty("best-char-fame")] public int BestCharFame { get; set; }

        public override void CreateDocument(FirestoreDb db)
        {
            db.Collection("accounts").Document().Create(this);
        }
    }
}