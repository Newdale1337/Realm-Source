using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Externals.Database;
using Externals.Models.FirestoreModels;
using Externals.Utilities;

namespace Externals.Resources
{
    public class WebResources
    {
        public static string EN_LANG_STRINGS { get; set; }
        public static string INIT { get; set; }
        public static string GLOBAL_NEWS { get; set; }
        public static string GUEST_ACCOUNT { get; set; }

        public static void Load()
        {
            EN_LANG_STRINGS = File.ReadAllText("Resources/Web/Application/en.json");
            INIT = File.ReadAllText("Resources/Web/Application/Init.xml");
            GLOBAL_NEWS = File.ReadAllText("Resources/Web/Application/GlobalNews.json");
            GUEST_ACCOUNT = File.ReadAllText("Resources/Web/GuestCharList.xml");
        }
    }
}
