using System.Diagnostics;
using System.IO;
using Externals.Models.GameDataModels;
using Externals.Utilities;

namespace Externals.Resources
{
    public class GameResources
    {
        private static string OBJECTS_PATH = "Resources/Game/Objects";
        
        private static void Initialize()
        {
            ObjectLibrary.Initialize();
        }

        public static void Load()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Initialize();
            ParseJson();
            LoggingUtils.LogIfDebug($"Finished parsing game resources in {{{sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}}}");
        }

        private static void ParseJson()
        {
            foreach (var file in new DirectoryInfo(OBJECTS_PATH).GetFiles("*.json", SearchOption.AllDirectories))
            {
                string data = File.ReadAllText(file.FullName);

                ObjectLibrary.ParseJson(data);
            }
        }
    }
}