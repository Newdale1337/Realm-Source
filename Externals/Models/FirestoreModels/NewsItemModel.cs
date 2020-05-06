using System;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    //public class NewsItemModel : FirestoreModel
    //{
    //    public override DocumentReference GetDocument(FirestoreDb db)
    //        => db.Collection("news").w;

    //    public override void UpdateFieldAsync(FirestoreDb db, string field, object data)
    //        => throw new InvalidOperationException("Method doesnt support updating fields.");
    //    public override void CreateDocument(FirestoreDb db)
    //        => db.Collection("news").Document().Create(this);
    //    public override void UpdateDocument(FirestoreDb db)
    //        => db.Collection("accounts").WhereEqualTo("account-id", AccountId).FirstDocument().SetAsync(this);
    //}
}