using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Externals.Utilities
{
    public class AllowHex : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string val = reader.Value.ToString();
            var isHex = val != null && val.Contains("0x");
            var isNegative = val != null && val.Contains("-");

            if (isNegative)
            {
                var format = new NumberFormatInfo();
                format.NegativeSign = "-";
                return val == null ? 0 : int.Parse(val, format);
            }
            if (isHex)
            {
                return val == null ? 0 : int.Parse(isHex ? val.Substring(2) : val, isHex ? NumberStyles.HexNumber : NumberStyles.None);
            }

            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

    public class JsonUtils
    {
    }
}