using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Externals.Utilities;

namespace Externals.Models.GameDataModels
{
    public class ObjectLibrary
    {

        public static Dictionary<string, ObjectProperties> IdToProperties { get; set; }
        private static Dictionary<string, int> IdToType { get; set; }
        private static Dictionary<int, ObjectProperties> TypeToProperties { get; set; }
        private static Dictionary<int, string> TypeToId { get; set; }

        public static ObjectProperties GetProperties(int type)
        {
            if (TypeToProperties.TryGetValue(type, out ObjectProperties props))
                return props;

            return null;
        }
        public static ObjectProperties GetProperties(string id)
        {
            if (IdToProperties.TryGetValue(id, out ObjectProperties props))
                return props;

            return null;
        }

        public static void Initialize()
        {
            IdToProperties = new Dictionary<string, ObjectProperties>(StringComparer.InvariantCultureIgnoreCase);
            IdToType = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            TypeToProperties = new Dictionary<int, ObjectProperties>();
            TypeToId = new Dictionary<int, string>();

            LoggingUtils.LogIfDebug("ObjectLibrary Initialized");
        }

        public static void ParseJson(XElement elem)
        {
            int type = elem.IntAtr("type");
            string id = elem.StringAtr("id");

            TypeToProperties[type] = new ObjectProperties(elem, id, type);
            IdToProperties[id] = new ObjectProperties(elem, id, type);
            IdToType[id] = type;
            TypeToId[type] = id;
        }
    }
}