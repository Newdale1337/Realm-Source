using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Anna.Request;
using Externals.Resources;
using Externals.Utilities;

namespace VirtualServer.Requests.Application
{
    public class GetLanguageStrings : WebRequest
    {
        public override string RequestType => "/app/getLanguageStrings";
        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            
            WriteJSON(requestContext, Encoding.UTF8.GetBytes(WebResources.EN_LANG_STRINGS));
        }
    }
}