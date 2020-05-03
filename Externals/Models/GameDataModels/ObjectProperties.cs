using System.Xml.Linq;
using Externals.Utilities;

namespace Externals.Models.GameDataModels
{
    public class ObjectProperties
    {
        public int Type { get; set; }
        public string Id { get; set; }
        public string DisplayId { get; set; }
        public string Class { get; set; }

        public ObjectProperties(XElement elem, string id, int type)
        {
            Id = id;
            Type = type;

            DisplayId = elem.StringElement("DisplayId");
            Class = elem.StringElement("Class");
        }
    }
}