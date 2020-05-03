using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Externals.Models.FirestoreModels;
using Google.Cloud.Firestore;

namespace Externals.Utilities
{
    public static class FirestoreUtils
    {
        public static T[] DocumentToArray<T>(this CollectionReference c) => c.GetSnapshot().Select(x => x.ConvertTo<T>()).ToArray();
        public static List<T> DocumentToList<T>(this CollectionReference c) => c.GetSnapshot().Select(x => x.ConvertTo<T>()).ToList();

        public static QuerySnapshot GetSnapshot(this Query query) => query.GetSnapshotAsync().GetAwaiter().GetResult();
        public static DocumentSnapshot GetSnapshot(this DocumentReference doc) => doc.GetSnapshotAsync().GetAwaiter().GetResult();
        public static WriteResult Create(this DocumentReference document, object documentData) => document.CreateAsync(documentData).GetAwaiter().GetResult();
        public static WriteResult Set(this DocumentReference document, object documentData) => document.SetAsync(documentData).GetAwaiter().GetResult();
        public static DocumentReference FirstDocument(this Query query)
        {
            var document = query.GetSnapshot().Documents[0];
            if (document.Exists)
                return document.Reference;

            LoggingUtils.LogWarningIfDebug($"Document not found ({document.Id})");
            return null;
        }
        public static async Task<DocumentReference> FirstDocumentAsync(this CollectionReference c)
        {
            var query = await c.GetSnapshotAsync();
            var document = query.Documents[0];
            if (document.Exists)
                return document.Reference;

            LoggingUtils.LogWarningIfDebug($"Document not found ({document.Id})");
            return null;
        }
    }
}