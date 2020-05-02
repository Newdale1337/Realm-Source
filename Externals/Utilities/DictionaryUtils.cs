using System.Collections;

namespace Externals.Utilities
{
    public static class DictionaryUtils
    {
        /// <summary>
        /// Safely adds a object to the dictionary without causing any errors.
        /// </summary>
        /// <returns>Dictionary</returns>
        public static IDictionary AddOrUpdate(this IDictionary dict, object key, object value)
        {
            if (!dict.Contains(key))
                dict.Add(key, value);
            else
                dict[key] = value;

            return dict;
        }
    }
}