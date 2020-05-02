using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using Externals.Utilities;
using Newtonsoft.Json;

namespace Externals.Models.GameDataModels
{
    public class ObjectLibrary
    {
        public static Dictionary<string, ObjectProperties> IdToProperties { get; set; }
        public static Dictionary<string, int> IdToType { get; set; }
        public static Dictionary<int, ObjectProperties> TypeToProperties { get; set; }
        public static Dictionary<int, string> TypeToId { get; set; }

        public static ObjectProperties GetProperties(string id)
        {
            if (!IdToProperties.TryGetValue(id, out ObjectProperties props))
            {
                LoggingUtils.LogIfDebug($"{id} not found...");
                return null;
            }

            return props;
        }
        public static ObjectProperties GetProperties(int type)
        {
            if (!TypeToProperties.TryGetValue(type, out ObjectProperties props))
            {
                LoggingUtils.LogIfDebug($"{type} not found...");
                return null;
            }

            return props;
        }

        public static void Initialize()
        {
            IdToProperties = new Dictionary<string, ObjectProperties>(StringComparer.InvariantCultureIgnoreCase);
            IdToType = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            TypeToProperties = new Dictionary<int, ObjectProperties>();
            TypeToId = new Dictionary<int, string>();
        }

        public static void ParseJson(string data)
        {
            var json = JsonConvert.DeserializeObject<ObjectProperties[]>(data, new JsonSerializerSettings()
            {
                Culture = CultureInfo.InvariantCulture,
                Converters = {new AllowHex()},
            });
            foreach (var obj in json)
            {
                IdToProperties[obj.Id] = obj;
                IdToType[obj.Id] = obj.Type;

                TypeToProperties[obj.Type] = obj;
                TypeToId[obj.Type] = obj.Id;
            }
        }
    }
}