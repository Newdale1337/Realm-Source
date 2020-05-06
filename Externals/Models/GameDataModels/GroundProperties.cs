using System.Xml.Linq;
using Externals.Utilities;

namespace Externals.Models.GameDataModels
{
    public class GroundProperties
    {
        public int GroundType { get; set; }
        public string GroundId { get; set; }
        public bool NoWalk { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public double Speed { get; set; }


        public GroundProperties(XElement elem, int type, string id)
        {
            GroundType = type;
            GroundId = id;
            NoWalk = elem.HasElement("NoWalk");
            MinDamage = elem.IntElement("MinDamage");
            MaxDamage = elem.IntElement("MaxDamage");
            Speed = elem.DoubleElement("Speed", 1);
        }
    }
}