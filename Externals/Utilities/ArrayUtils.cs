using System.Globalization;
using System.Linq;

namespace Externals.Utilities
{
    public static class ArrayUtils
    {
        public static T[] Empty<T>() => new T[0];

        public static long[] FromCommaSepString64(string value) => string.IsNullOrEmpty(value) ? new long[0] : value?.Split(',').Select(_ => _.Contains("0x") ? long.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : long.Parse(_.Trim())).ToArray();
        public static int[] FromCommaSepString32(string value) => string.IsNullOrEmpty(value) ? new int[0] : value?.Split(',').Select(_ => _.Contains("0x") ? int.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : int.Parse(_.Trim())).ToArray();
        public static short[] FromCommaSepString16(string value) => string.IsNullOrEmpty(value) ? new short[0] : value?.Split(',').Select(_ => _.Contains("0x") ? short.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : short.Parse(_.Trim())).ToArray();

        public static ulong[] FromCommaSepStringU64(string value) => string.IsNullOrEmpty(value) ? new ulong[0] : value?.Split(',').Select(_ => _.Contains("0x") ? ulong.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : ulong.Parse(_.Trim())).ToArray();
        public static uint[] FromCommaSepStringU32(string value) => string.IsNullOrEmpty(value) ? new uint[0] : value?.Split(',').Select(_ => _.Contains("0x") ? uint.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : uint.Parse(_.Trim())).ToArray();
        public static ushort[] FromCommaSepStringU16(string value) => string.IsNullOrEmpty(value) ? new ushort[0] : value?.Split(',').Select(_ => _.Contains("0x") ? ushort.Parse(_.Trim().Substring(2), NumberStyles.HexNumber) : ushort.Parse(_.Trim())).ToArray();

        public static string ToCommaSepString<T>(T[] arr) => string.Join(",", arr.Select(_ => _.ToString()));
    }
}