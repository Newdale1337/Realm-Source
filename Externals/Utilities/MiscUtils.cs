using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public static bool AddOrForget(this IList list, object obj)
        {
            if (!list.Contains(obj))
            {
                list.Add(obj);
                return true;
            }

            return false;
        }

        public static async Task RunAsync(Action a)
        {
            await Task.Run(() => { a(); });
        }
    }
}