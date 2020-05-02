using System.Collections.Specialized;
using Anna.Request;

namespace VirtualServer.Requests.Account
{
    public class IsEmailVerified : WebRequest
    {
        public override string RequestType => "/account/isEmailVerified";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            WriteXml(requestContext, "True");
        }
    }
}