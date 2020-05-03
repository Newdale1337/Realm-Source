using System;
using System.Globalization;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;

namespace Externals.Utilities
{
    public static class XElementUtils
    {
        public static int IntElement(this XElement elem, string name, int def = 0) => TryParse(elem?.Element(name)?.Value, def);
        public static int IntAtr(this XElement elem, string name, int def = 0) => TryParse(elem?.Attribute(name)?.Value, def);

        public static float FloatElement(this XElement elem, string name, float def = 0) => TryParse(elem?.Element(name)?.Value, def);
        public static float FloatAtr(this XElement elem, string name, float def = 0) => TryParse(elem?.Attribute(name)?.Value, def);

        public static double DoubleElement(this XElement elem, string name, double def = 0) => TryParse(elem?.Element(name)?.Value, def);
        public static double DoubleAtr(this XElement elem, string name, double def = 0) => TryParse(elem?.Attribute(name)?.Value, def);

        public static bool BoolElement(this XElement elem, string name, bool def = false) => TryParse(elem?.Element(name)?.Value, def);
        public static bool BoolAtr(this XElement elem, string name, bool def = false) => TryParse(elem?.Attribute(name)?.Value, def);

        public static string StringElement(this XElement elem, string name) => elem?.Element(name)?.Value ?? string.Empty;
        public static string StringAtr(this XElement elem, string name) => elem?.Attribute(name)?.Value ?? string.Empty;

        public static bool TryParse(this string val, bool def)
        {
            if (!bool.TryParse(val, out bool res))
                return def;

            return res;
        }
        public static double TryParse(this string val, double def)
        {
            if (!double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                return def;

            return res;
        }
        public static float TryParse(this string val, float def)
        {
            if (!float.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out float res))
                return def;

            return res;
        }
        public static int TryParse(this string val, int def)
        {
            var isHex = val.Contains("0x");

            if (isHex)
            {
                return Convert.ToInt32(val, 16);
            }

            if (!int.TryParse(val, out int res))
                return def;

            return res;
        }
    }
}