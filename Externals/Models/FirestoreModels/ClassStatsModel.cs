using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class ClassStatsModel
    {
        [FirestoreProperty("objectType")] public int ObjectType { get; set; }
        [FirestoreProperty("bestLevel")] public int BestLevel { get; set; }
        [FirestoreProperty("bestFame")] public int BestFame { get; set; }
    }
}