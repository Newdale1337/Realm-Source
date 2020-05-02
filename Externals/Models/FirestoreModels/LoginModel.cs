﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Externals.Database;
using Externals.Resources;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class LoginModel : FirestoreModel
    {
        [FirestoreProperty("accountId")] public int AccountId { get; set; }
        [FirestoreProperty("guid")] public string Guid { get; set; }
        [FirestoreProperty("name")] public string Name { get; set; }
        [FirestoreProperty("hash")] public string Hash { get; set; }
        [FirestoreProperty("salt")] public string Salt { get; set; }
        [FirestoreProperty("creation-date")] public string CreationDate { get; set; }

        public override void CreateDocument(FirestoreDb db) => db.Collection("logins").Document().Create(this);
    }
}