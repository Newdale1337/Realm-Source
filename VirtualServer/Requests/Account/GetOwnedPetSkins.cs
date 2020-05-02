using System.Collections.Specialized;
using Anna.Request;

namespace VirtualServer.Requests.Account
{
    public class GetOwnedPetSkins : WebRequest
    {
        public override string RequestType => "/account/getOwnedPetSkins";

        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            requestContext.Respond("<PetSkins></PetSkins>");
        }
    }
}