using System.Threading.Tasks;
using Externals.Database;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    public abstract class FirestoreModel
    {
        public virtual async Task CreateDocumentAsync(FirestoreDb db) { }
        public virtual async Task CreateDocumentAsync(VirtualServerDatabase db) { }
        public virtual void CreateDocument(FirestoreDb db) { }
        public virtual void CreateDocument(VirtualServerDatabase db) { }
        public virtual void UpdateDocument(VirtualServerDatabase db) { }
        public virtual void UpdateDocument(FirestoreDb db) { }
        public virtual void GetDocument(FirestoreDb db) { }
        public virtual void GetDocument(VirtualServerDatabase db) { }
    }
}