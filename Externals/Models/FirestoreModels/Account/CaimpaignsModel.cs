using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class CaimpaignsModel
    {
        [FirestoreProperty("points")] public int Points { get; set; }
    }
}