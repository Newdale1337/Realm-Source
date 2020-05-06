using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class CurrenciesModel
    {
        [FirestoreProperty("credits")] public int Credits { get; set; }
        [FirestoreProperty("fortune-tokens")] public int FortuneTokens { get; set; }
        [FirestoreProperty("fame")] public int Fame { get; set; }
        [FirestoreProperty("total-fame")] public int TotalFame { get; set; }
    }
}