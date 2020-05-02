using System.Collections.Specialized;
using Anna.Request;
using Externals.Database;

namespace VirtualServer.Requests.Account
{
    public class Verify : WebRequest
    {
        public override string RequestType => "/account/verify";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            using (VirtualServerDatabase db = new VirtualServerDatabase(Database))
            {
            }
        }
    }
}