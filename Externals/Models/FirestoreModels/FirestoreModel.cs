using System.Threading.Tasks;
using Externals.Database;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    public abstract class FirestoreModel
    {
        public virtual async Task CreateDocumentAsync(VirtualServerDatabase db) { }
        public virtual async Task CreateDocumentAsync(FirestoreDb db) { }

        public virtual void GetDocument(VirtualServerDatabase db) { }
        public virtual void CreateDocument(VirtualServerDatabase db) { }
        public virtual void UpdateDocument(VirtualServerDatabase db) { }
        public abstract void UpdateDocument(FirestoreDb db);
        public abstract WriteResult CreateDocument(FirestoreDb db);
        public abstract DocumentReference GetDocument(FirestoreDb db);
        public abstract void UpdateFieldAsync(FirestoreDb db, string field, object data);
    }
}