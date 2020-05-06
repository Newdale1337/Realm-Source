using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Externals.Models.GameDataModels;
using Externals.Utilities;

namespace Externals.Resources
{
    public class GameResources
    {
        private const string XmlsPath = "Resources/Game/Xmls";

        private static void Initialize()
        {
            ObjectLibrary.Initialize();
            GroundLibrary.Initialize();
        }

        public static void Load()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Initialize();
            ParseXml(sw);

            sw = null;
        }

        private static async void ParseXml(Stopwatch sw)
        {
            string[] files = Directory.GetFiles(XmlsPath, "*.xml", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                await Task.Factory.StartNew(() =>
                {
                    XElement rootElem = XElement.Load(file);
                    foreach (var e in rootElem.Elements("Object"))
                        ObjectLibrary.ParseXml(e);
                    foreach (var e in rootElem.Elements("Ground"))
                        GroundLibrary.ParseXml(e);
                });
            }
        }

        public static void Unload()
        {
            ObjectLibrary.Unload();

            LoggingUtils.LogIfDebug("GameResources unloaded.");
        }
    }
}