using System.Collections.Specialized;
using Anna.Request;
using Externals.Resources;

namespace VirtualServer.Requests.Character
{
    public class CharacterList : WebRequest
    {
        public override string RequestType => "/char/list";
        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            WriteXml(requestContext, WebResources.GUEST_ACCOUNT);
        }
    }
}