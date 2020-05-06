using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Externals.Utilities;

namespace Externals.Models.GameDataModels
{
    public class GroundLibrary
    {
        public static Dictionary<int, GroundProperties> Properties { get; set; }
        public static Dictionary<int, string> TypeToId { get; set; }
        public static Dictionary<string, int> IdToType { get; set; }

        public static GroundProperties GetProperties(int type)
        {
            if (!Properties.TryGetValue(type, out GroundProperties props))
            {
                LoggingUtils.LogWarningIfDebug($"{type} not found.");
            }

            return props;
        }
        public static GroundProperties GetProperties(string id)
        {
            if (!IdToType.TryGetValue(id, out int type))
            {
                LoggingUtils.LogWarningIfDebug($"{id} not found.");
            }

            return GetProperties(type);
        }

        public static void Initialize()
        {
            Properties = new Dictionary<int, GroundProperties>();
            TypeToId = new Dictionary<int, string>();
            IdToType = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        }

        public static void ParseXml(XElement elem)
        {
            int type = elem.IntAtr("type");
            string id = elem.StringAtr("id");

            Properties[type] = new GroundProperties(elem, type, id);
            IdToType[id] = type;
            TypeToId[type] = id;
        }
    }
}