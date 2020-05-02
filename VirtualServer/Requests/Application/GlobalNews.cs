using System.Collections.Specialized;
using System.Text;
using Anna.Request;
using Externals.Resources;

namespace VirtualServer.Requests.Application
{
    public class GlobalNews : WebRequest
    {
        public override string RequestType => "/app/globalNews";
        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            WriteJSON(requestContext, Encoding.UTF8.GetBytes(WebResources.GLOBAL_NEWS));
        }
    }
}