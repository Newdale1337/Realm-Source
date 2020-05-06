using System.Collections.Specialized;
using Anna.Request;
using Externals.Models.FirestoreModels;
using Externals.Models.XmlModels;
using Externals.Resources;
using Externals.Utilities;

namespace VirtualServer.Requests.Character
{
    public class CharacterList : WebRequest
    {
        public override string RequestType => "/char/list";
        public override void Handle(RequestContext requestContext, NameValueCollection query)
        {
            AccountModel acc = null;
            if ((acc = Database.Verify(query["guid"], query["password"])) == null)
            {
                WriteXml(requestContext, WebResources.GUEST_ACCOUNT);
                return;
            }

            var charList = new CharacterListModel(acc, Database);
            WriteXml(requestContext, charList.ToXml().ToString());
        }
    }
}