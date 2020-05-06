using System.IO;
using Ionic.Zlib;

namespace Externals.Utilities
{
    public static class IonicUtils
    {
        public static byte[] CompressBuffer(byte[] buffer) => ZlibStream.CompressBuffer(buffer);
        public static byte[] UncompressBuffer(byte[] buffer) => ZlibStream.UncompressBuffer(buffer);
        public static string CompressString(byte[] buffer) => ZlibStream.UncompressString(buffer);
        public static string UncompressString(byte[] buffer) => ZlibStream.UncompressString(buffer);

        public static BinaryReader CreateCompressZlibStream(Stream stream) => new BinaryReader(CreateZlibStream(stream, true));
        public static BinaryReader CreateDecompressZlibStream(Stream stream) => new BinaryReader(CreateZlibStream(stream, false));
        public static ZlibStream CreateZlibStream(Stream stream, bool compress) => new ZlibStream(stream, compress ? CompressionMode.Compress : CompressionMode.Decompress);
    }
}