using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Externals.Models.GameDataModels;
using Externals.Utilities;

namespace Externals.Resources
{
    public class GameResources
    {
        private const string XmlsPath = "Resources/Game/Objects";
        
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
            sw = null;
        }

        private static void ParseJson()
        {
            foreach (var file in new DirectoryInfo(XmlsPath).GetFiles("*.xml", SearchOption.AllDirectories))
            {
                Task.Factory.StartNew(() =>
                {
                    string data = File.ReadAllText(file.FullName);

                });
            }
        }
    }
}