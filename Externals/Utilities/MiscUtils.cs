using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace Externals.Utilities
{
    public static class MiscUtils
    {
        public static Dictionary<string, object> Serialize<T>(this T c)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (var p in c.GetType().GetProperties())
            {
                ret.Add(p.Name, p.GetValue(c));
            }

            return ret;
        }

        public static async Task RunAsync(Action a)
        {
            await Task.Run(() => { a(); });
        }
    }
}