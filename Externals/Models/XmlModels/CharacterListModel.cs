using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Models.FirestoreModels.Character;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.XmlModels
{
    public class CharacterListModel
    {
        public List<ServerModel> Servers { get; set; }
        public AccountModel Account { get; set; }
        private List<CharacterModel> AliveCharacters { get; set; }

        public CharacterListModel(AccountModel acc, VirtualServerDatabase db)
        {
            Account = acc;
            Servers = db.CachedServers;
            AliveCharacters = db.GetAliveCharacters(acc.AccountId);
        }

        public XElement ToXml()
        {
            var ret = new XElement("Chars",
                new XAttribute("nextCharId", Account.NextCharacterId),
                new XAttribute("maxNumChars", Account.CharacterCount));

            ret.Add(AliveCharacters.Select(x => x.ToXml()));
            ret.Add(Account.ToXml());
            ret.Add(new XElement("Servers", Servers.Select(x => x.ToXml())));
            ret.Add(new XElement("OwnedSkins", string.Join(",", Account.OwnedSkins)));
            ret.Add(new XElement("ClassAvailabilityList", RotmgUtils.CharacterRestricitons.Select(x => new XElement("ClassAvailability", new XAttribute("id", x.Key), x.Value))));
            LoggingUtils.LogIfDebug(ret.ToString());
            return ret;
        }
    }
}