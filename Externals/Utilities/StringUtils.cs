using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Externals.Utilities
{
    public class StringUtils
    {
        public const string POSSIBLE_CHARS = "abcdefghijklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ0123456789#&=?€@£$€";

        public static string GetRandomString(int len)
        {
            Random random = new Random();

            StringBuilder sb = new StringBuilder(len);

            for (int i = 0; i <= len; i++)
            {
                sb.Append(POSSIBLE_CHARS[random.Next(0, POSSIBLE_CHARS.Length)]);
            }

            return sb.ToString();
        }
    }
}