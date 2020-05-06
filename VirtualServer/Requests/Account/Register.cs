using System.Collections.Specialized;
using System.Globalization;
using Anna.Request;
using Externals.Database;
using Externals.Database.Returnables;
using Externals.Utilities;
namespace VirtualServer.Requests.Account
{
    public class Register : WebRequest
    {
        public override string RequestType => REGISTER;

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            string result = Database.Register(query["newGUID"], query["newPassword"], query["name"], requestContext.Request.GetIp());

            if (result == RegisterReturns.ACCOUNT_EXISTS || result == RegisterReturns.NAME_EXISTS)
                requestContext.WriteError(result);
            else
                requestContext.Respond($"<Success><Token>{result}</Token></Success>");
        }
    }
}