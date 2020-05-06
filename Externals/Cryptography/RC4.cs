using System;
using System.Text;

namespace Externals.Cryptography
{
    public class RC4Cipher
    {
        private byte[] EngineState { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        private byte[] WorkingKey { get; set; }

        public RC4Cipher(byte[] key)
        {
            WorkingKey = key;
            SetKey(WorkingKey);
        }

        public RC4Cipher(string hexString)
        {
            WorkingKey = HexStringToBytes(hexString);
            SetKey(WorkingKey);
        }

        public void ProcessBytes(byte[] buffer, int offset, int len) => ProcessBytes(buffer, offset, len, buffer, offset);

        public void Reset() => SetKey(WorkingKey);

        private void ProcessBytes(byte[] bytes, int inOff, int length, byte[] output, int outOff)
        {
            for (var i = 0; i < length; i++)
            {
                X = (X + 1) & 0xff;
                Y = (EngineState[X] + Y) & 0xff;

                var tmp = EngineState[X];
                EngineState[X] = EngineState[Y];
                EngineState[Y] = tmp;

                output[i + outOff] = (byte)(bytes[i + inOff] ^ EngineState[(EngineState[X] + EngineState[Y]) & 0xff]);
            }
        }

        private void SetKey(byte[] keyBytes)
        {
            WorkingKey = keyBytes;
            X = Y = 0;

            if (EngineState == null)
                EngineState = new byte[256];

            for (var i = 0; i < 256; i++)
                EngineState[i] = (byte)i;

            int i1 = 0, i2 = 0;

            for (var i = 0; i < 256; i++)
            {
                i2 = ((keyBytes[i1] & 0xff) + EngineState[i] + i2) & 0xff;

                var tmp = EngineState[i];
                EngineState[i] = EngineState[i2];
                EngineState[i2] = tmp;
                i1 = (i1 + 1) % keyBytes.Length;
            }
        }

        public byte[] HexStringToBytes(string key)
        {
            if (key.Length % 2 != 0)
                throw new ArgumentException("Invalid hex string!");

            var bytes = new byte[key.Length / 2];
            var c = key.ToCharArray();
            for (var i = 0; i < c.Length; i += 2)
                bytes[(i / 2)] = (byte)Convert.ToInt32(new StringBuilder(2).Append(c[i]).Append(c[(i + 1)]).ToString(), 16);
            return bytes;
        }
    }
}