using System.Xml.Linq;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels
{
    [FirestoreData]
    public class ServerModel : FirestoreModel
    {
        [FirestoreProperty("ip-address")] public string IpAddress { get; set; }
        [FirestoreProperty("status")] public bool Status { get; set; }
        [FirestoreProperty("name")] public string Name { get; set; }
        [FirestoreProperty("lat")] public double Lat { get; set; }
        [FirestoreProperty("long")] public double Long { get; set; }
        [FirestoreProperty("usage")] public double Usage { get; set; }

        public override DocumentReference GetDocument(FirestoreDb db)
            => db.Collection("servers").WhereEqualTo("ip-address", IpAddress).FirstDocument();
        public override void UpdateFieldAsync(FirestoreDb db, string field, object data)
            => db.Collection("servers").WhereEqualTo("ip-address", IpAddress).FirstDocument().UpdateAsync(field, data);
        public override WriteResult CreateDocument(FirestoreDb db)
            => db.Collection("servers").Document().CreateAsync(this).Result;
        public override void UpdateDocument(FirestoreDb db)
            => db.Collection("servers").WhereEqualTo("ip-address", IpAddress).FirstDocument().SetAsync(this);

        public XElement ToXml()
        {
            var ret = new XElement("Server");

            ret.Add(new XElement("Name", Name));
            ret.Add(new XElement("DNS", IpAddress));
            ret.Add(new XElement("Lat", Lat));
            ret.Add(new XElement("Long", Long));
            ret.Add(new XElement("Usage", Usage));
            ret.Add(new XElement("Status", Status));

            return ret;
        }
    }
}