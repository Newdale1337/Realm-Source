using System.Collections.Specialized;
using System.Text;
using Anna.Request;

namespace VirtualServer.Requests
{
    public sealed class Crossdomain : WebRequest
    {
        public override string RequestType => "/crossdomain.xml";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            var buffer = Encoding.UTF8.GetBytes(@"<cross-domain-policy>
<allow-access-from domain=""*""/>
</cross-domain-policy>");

            Write(requestContext, buffer);
        }
    }
}
