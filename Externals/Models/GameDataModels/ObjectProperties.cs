using Newtonsoft.Json;

namespace Externals.Models.GameDataModels
{
    public class ObjectProperties
    {
        [JsonProperty("type")] public int Type { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        public string Class { get; set; }
        public string Description { get; set; }
        public AnimatedTexture AnimatedTexture { get; set; }
        public string HitSound { get; set; }
        public string DeathSound { get; set; }
        public string Player { get; set; }
        public double BloodProb { get; set; }
        public int[] SlotTypes { get; set; }
        public int[] Equipment { get; set; }
        public MaxHitPoints MaxHitPoints { get; set; }
        public MaxMagicPoints MaxMagicPoints { get; set; }
        public Attack Attack { get; set; }
        public Defense Defense { get; set; }
        public Speed Speed { get; set; }
        public Dexterity Dexterity { get; set; }
        public HpRegen HpRegen { get; set; }
        public MpRegen MpRegen { get; set; }
        public LevelIncrease[] LevelIncrease { get; set; }
        public int UnlockCost { get; set; }
    }

    public class AnimatedTexture
    {
        public string File { get; set; }
        public string Index { get; set; }
    }

    public class MaxHitPoints
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class MaxMagicPoints
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class Attack
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class Defense
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class Speed
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class Dexterity
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class HpRegen
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class MpRegen
    {
        [JsonProperty("_")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }

    public class LevelIncrease
    {
        [JsonProperty("_")] public string Stat { get; set; }
        [JsonProperty("min")] public int StartingValue { get; set; }
        [JsonProperty("max")] public int Max { get; set; }
    }
}