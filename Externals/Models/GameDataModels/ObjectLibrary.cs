using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Externals.Utilities;

namespace Externals.Models.GameDataModels
{
    public class ObjectLibrary
    {
        public static Dictionary<string, int> IdToType { get; set; }
        public static Dictionary<int, ObjectProperties> Properties { get; set; }
        public static Dictionary<int, string> TypeToId { get; set; }

        public static ObjectProperties GetProperties(int type)
        {
            if (Properties.TryGetValue(type, out var props))
                return props;

            return null;
        }

        public static int IdToObjectType(string id)
        {
            if (!IdToType.TryGetValue(id, out int type))
            {
                LoggingUtils.LogWarningIfDebug($"{id} Not found");
                return -1;
            }

            return type;
        }

        public static ObjectProperties GetProperties(string id)
        {
            if (IdToType.TryGetValue(id, out var props))
                return GetProperties(props);

            return null;
        }

        public static void Initialize()
        {
            IdToType = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            Properties = new Dictionary<int, ObjectProperties>();
            TypeToId = new Dictionary<int, string>();
        }

        public static void ParseXml(XElement elem)
        {
            int type = elem.IntAtr("type");
            string id = elem.StringAtr("id");

            Properties[type] = new ObjectProperties(elem, id, type);
            IdToType[id] = type;
            TypeToId[type] = id;
        }

        public static void Unload()
        {
            Properties.Clear();
            IdToType.Clear();
            TypeToId.Clear();

            LoggingUtils.LogIfDebug("ObjectLibrary unloaded.");
        }
    }
}