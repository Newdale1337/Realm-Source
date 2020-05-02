using System.Collections.Specialized;
using Anna.Request;
using Externals.Database;
using Externals.Database.Returnables;
using Externals.Utilities;

namespace VirtualServer.Requests.Account
{
    public class Register : WebRequest
    {
        public override string RequestType => "/account/register";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            using (VirtualServerDatabase db = new VirtualServerDatabase(Database))
            {
                string register = db.Register(query["newGUID"], query["newPassword"], query["name"]);

                if (register == RegisterReturns.ACCOUNT_EXISTS || register == RegisterReturns.NAME_EXISTS)
                    requestContext.WriteError(register);
                else
                    requestContext.Respond("<Success/>");
            }
        }
    }
}