using System;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class AccountLockModel
    {
       [FirestoreProperty("account-in-use")] public bool AccountInUse { get; set; }
       [FirestoreProperty("acquired-lock")] public DateTime AcquiredLock { get; set; }
    }
}