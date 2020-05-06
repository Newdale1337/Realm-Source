using System.Collections.Specialized;
using System.Linq;
using Anna.Request;
using Externals.Models.FirestoreModels;
using Externals.Models.FirestoreModels.Character;
using Externals.Utilities;

namespace VirtualServer.Requests.Character
{
    public class Delete : WebRequest
    {
        public override string RequestType => "/char/delete";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            var guid = query["guid"];
            var password = query["password"];
            var charId = query["charId"];

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(password))
            {
                requestContext.WriteError("Bad login.");
                return;
            }

            AccountModel acc = null;
            if ((acc = Database.Verify(guid, password)) == null)
            {
                requestContext.WriteError("Bad login.");
                return;
            }

            if (!int.TryParse(charId, out int id))
            {
                requestContext.WriteError("Bad login.");
                return;
            }

            var chr = Database.Api.Collection("characters").WhereEqualTo("character-id", id);
            chr?.Delete();
            WriteXml(requestContext, "<Success/>");
        }
    }
}