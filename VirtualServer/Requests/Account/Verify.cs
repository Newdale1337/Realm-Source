using System.Collections.Specialized;
using Anna.Request;
using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Utilities;

namespace VirtualServer.Requests.Account
{
    public class Verify : WebRequest
    {
        public override string RequestType => "/account/verify";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            AccountModel acc = null;
            if ((acc = Database.Verify(query["guid"], query["password"])) == null)
            {
                requestContext.WriteError("Bad login.");
                return;
            }

            WriteXml(requestContext, acc.ToXml().ToString());
        }
    }
}