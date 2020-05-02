using System.Collections.Specialized;
using Anna.Request;
using Externals.Resources;

namespace VirtualServer.Requests.Application
{
    public class Init : WebRequest
    {
        public override string RequestType => "/app/init";
        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            WriteXml(requestContext, WebResources.INIT);
        }
    }
}