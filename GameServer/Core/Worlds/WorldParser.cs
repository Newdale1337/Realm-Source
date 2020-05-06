using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Externals.Models.GameDataModels;
using Externals.Utilities;
using Newtonsoft.Json;

namespace GameServer.Core.Worlds
{
    public struct Object
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public struct Dict
    {
        [JsonProperty("ground")]
        public string Ground { get; set; }
        [JsonProperty("objs")]
        public Object[] Objs { get; set; }
        [JsonProperty("regions")]
        public Object[] Regions { get; set; }
    }

    public struct JsonData
    {
        [JsonProperty("data")]
        public byte[] Data { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("dict")]
        public Dict[] Dict { get; set; }
    }

    public sealed class MapParser
    {
        private static Dictionary<string, MapParser> MapCache { get; set; }

        public static MapParser ParseFromFile(string mapFile)
        {
            var json = mapFile;
            if (json == null)
            {
                LoggingUtils.LogWarningIfDebug($"Unknown mapFile to parse {mapFile}");
                return null;
            }

            if (MapCache == null)
                MapCache = new Dictionary<string, MapParser>();

            var setting = MapCache.FirstOrDefault(_ => _.Key == mapFile).Value;
            if (setting == null)
            {
                setting = new MapParser(mapFile);
                MapCache[mapFile] = setting;
            }
            return setting;
        }

        public int Width { get; }
        public int Height { get; }
        public Dict[,] Datas { get; }

        public MapParser(string mapFile)
        {
            var json = mapFile != null ? File.ReadAllText(mapFile) : null;
            if (json == null)
            {
                LoggingUtils.LogWarningIfDebug($"Unknown mapFile to parse {mapFile}");
                return;
            }

            var jsonData = JsonConvert.DeserializeObject<JsonData>(json);

            Width = jsonData.Width;
            Height = jsonData.Height;

            Datas = new Dict[Width, Height];
            using (var rdr = new BinaryReader(new MemoryStream(IonicUtils.UncompressBuffer(jsonData.Data)), Encoding.UTF8))
                for (var y = 0; y < jsonData.Height; y++)
                    for (var x = 0; x < jsonData.Width; x++)
                        Datas[x, y] = jsonData.Dict[IPAddress.NetworkToHostOrder(rdr.ReadInt16())];
        }

        //public void AddToMapCenter(IWorld world, double placeX, double placeY)
        //{
        //    var width = Datas.GetLength(0);
        //    var height = Datas.GetLength(1);

        //    var visibilityManager = world.VisibilityManager;

        //    var regionPoints = new List<IntPoint>();
        //    for (var x = 0; x < width; x++)
        //        for (var y = 0; y < height; y++)
        //        {
        //            var data = Datas[x, y];

        //            var tileType = GroundLibrary.IdToGroundType(data.Ground);
        //            if (tileType == 0xFF)
        //                continue;

        //            var tx = placeX + x - width / 2;
        //            var ty = placeY + y - height / 2;

        //            var tile = world.GetTile(tx, ty);

        //            var groundRegion = (GroundRegion)Enum.Parse(typeof(GroundRegion), data.Regions?[0].Id.Replace(' ', '_') ?? "None");

        //            if (tile == null)
        //            {
        //                tile = new Tile((int)tx, (int)ty, tileType);
        //                tile.GroundRegion = groundRegion;
        //                regionPoints.Add(new IntPoint((int)tx, (int)ty));
        //                world.SetTile(tile);
        //            }
        //            else
        //            {
        //                tile.TileType = tileType;
        //                if (tile.GroundRegion == GroundRegion.None)
        //                {
        //                    tile.GroundRegion = groundRegion;
        //                    regionPoints.Add(new IntPoint((int)tx, (int)ty));
        //                }
        //                world.InvalidateTile(tx, ty);
        //            }

        //            //todo implement entity creation

        //            var objectType = ObjectLibrary.IdToObjectType(data.Objs?[0].Id ?? "");

        //            var objectP = ObjectLibrary.GetProperties(objectType);

        //            if (objectP != null)
        //            {
        //                visibilityManager.SetBlocksSight(tx, ty, objectP.BlocksSight);
        //                visibilityManager.SetEnemyOccupySquare(tx, ty, objectP.EnemyOccupySquare);
        //                visibilityManager.SetFullOccupy(tx, ty, objectP.FullOccupy);
        //                visibilityManager.SetOccupySquare(tx, ty, objectP.OccupySquare);

        //                if (objectP.IsStatic)
        //                {
        //                    tile.ObjectId = world.GetNextObjectId();
        //                    tile.ObjectType = objectP.ObjectType;
        //                }
        //                else
        //                    world.CreateNewObject(objectType, tx + 0.5f, ty + 0.5f);
        //            }
        //            visibilityManager.SetWalkable(tx, ty, !tile.NoWalk);
        //        }
        //    world.UpdateRegions(regionPoints);
        //}
    }
}