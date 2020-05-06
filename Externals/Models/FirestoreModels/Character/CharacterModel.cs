using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Externals.Utilities;
using Google.Cloud.Firestore;

namespace Externals.Models.FirestoreModels.Character
{
    [FirestoreData]
    public class CharacterModel : FirestoreModel
    {
        [FirestoreProperty("character-id")] public int CharacterId { get; set; }
        [FirestoreProperty("account-id")] public int AccountId { get; set; }
        [FirestoreProperty("object-type")] public int ObjectType { get; set; }
        [FirestoreProperty("level")] public int Level { get; set; }
        [FirestoreProperty("experience")] public int Experience { get; set; }
        [FirestoreProperty("current-fame")] public int CurrentFame { get; set; }
        [FirestoreProperty("equipment")] public List<int> Equipment { get; set; }
        [FirestoreProperty("backpack")] public List<int> Backpack { get; set; }
        [FirestoreProperty("max-hp")] public int MaxHp { get; set; }
        [FirestoreProperty("max-mp")] public int MaxMp { get; set; }
        [FirestoreProperty("hp")] public int Hp { get; set; }
        [FirestoreProperty("mp")] public int Mp { get; set; }
        [FirestoreProperty("attack")] public int Attack { get; set; }
        [FirestoreProperty("defense")] public int Defense { get; set; }
        [FirestoreProperty("speed")] public int Speed { get; set; }
        [FirestoreProperty("dex")] public int Dexterity { get; set; }
        [FirestoreProperty("vit")] public int Vitality { get; set; }
        [FirestoreProperty("wis")] public int Wisdom { get; set; }
        [FirestoreProperty("hp-pots")] public int HpPots { get; set; }
        [FirestoreProperty("mp-pots")] public int MpPots { get; set; }
        [FirestoreProperty("dead")] public bool Dead { get; set; }
        [FirestoreProperty("player-name")] public string Name { get; set; }
        [FirestoreProperty("tex1")] public int Clothing { get; set; }
        [FirestoreProperty("tex2")] public int Accessory { get; set; }
        [FirestoreProperty("texture")] public int Texture { get; set; }
        [FirestoreProperty("xp-boosted")] public bool XpBoosted { get; set; }
        [FirestoreProperty("xp-timer")] public int XpTimer { get; set; }
        [FirestoreProperty("loot-drop-timer")] public int LootDropTimer { get; set; }
        [FirestoreProperty("loot-tier-timer")] public int LootTierTimer { get; set; }
        [FirestoreProperty("has-backpack")] public bool HasBackpack { get; set; }
        [FirestoreProperty("is-challenger")] public bool IsChallenger { get; set; }
        [FirestoreProperty("star-background")] public bool StarBackground { get; set; }
        [FirestoreProperty("creation-date")] public DateTime CreationDate { get; set; }

        public override void UpdateDocument(FirestoreDb db)
            => db.Collection("characters")
                .WhereEqualTo("account-id", AccountId)
                .WhereEqualTo("character-id", CharacterId).FirstDocument().Set(this);
        public override WriteResult CreateDocument(FirestoreDb db)
            => db.Collection("characters").Document().Create(this);
        public override DocumentReference GetDocument(FirestoreDb db) =>
            db.Collection("characters")
                .WhereEqualTo("account-id", AccountId)
                .WhereEqualTo("character-id", CharacterId).FirstDocument();
        public override void UpdateFieldAsync(FirestoreDb db, string field, object data)
            => db.Collection("characters")
                .WhereEqualTo("account-id", AccountId)
                .WhereEqualTo("character-id", CharacterId).FirstDocument().UpdateAsync(field, data);

        public XElement ToXml()
        {
            var ret = new XElement("Char", new XAttribute("id", CharacterId));

            ret.Add(new XElement("ObjectType", ObjectType));
            ret.Add(new XElement("Level", Level));
            ret.Add(new XElement("Exp", Experience));
            ret.Add(new XElement("CurrentFame", CurrentFame));
            ret.Add(new XElement("Equipment", HasBackpack ? $"{string.Join(",", Equipment)},{string.Join(",", Backpack)}" : string.Join(",", Equipment)));
            ret.Add(new XElement("MaxHitPoints", MaxHp));
            ret.Add(new XElement("HitPoints", Hp));
            ret.Add(new XElement("MaxMagicPoints", MaxMp));
            ret.Add(new XElement("MagicPoints", Mp));
            ret.Add(new XElement("Attack", Attack));
            ret.Add(new XElement("Defense", Defense));
            ret.Add(new XElement("Speed", Speed));
            ret.Add(new XElement("Dexterity", Dexterity));
            ret.Add(new XElement("HpRegen", Vitality));
            ret.Add(new XElement("MpRegen", Wisdom));
            ret.Add(new XElement("PCStats", ""));
            ret.Add(new XElement("HealthStackCount", HpPots));
            ret.Add(new XElement("MagicStackCount", MpPots));
            ret.Add(new XElement("Dead", Dead));
            ret.Add(new XElement("casToken", "None"));
            ret.Add(new XElement("Account", new XElement("Name", Name)));
            ret.Add(new XElement("Tex1", Clothing));
            ret.Add(new XElement("Tex2", Accessory));
            ret.Add(new XElement("Texture", Texture));
            ret.Add(new XElement("XpBoosted", XpBoosted ? 1 : 0));
            ret.Add(new XElement("XpTimer", XpTimer));
            ret.Add(new XElement("LDTimer", LootDropTimer));
            ret.Add(new XElement("LTTimer", LootTierTimer));
            ret.Add(new XElement("HasBackpack", HasBackpack ? 1 : 0));
            ret.Add(new XElement("IsChallenger", IsChallenger ? 1 : 0));
            ret.Add(new XElement("StarBackground", StarBackground ? 1 : 0));
            ret.Add(new XElement("CreationDate", $"{CreationDate:M, Y}"));

            return ret;
        }

        //<Tex1>33553146</Tex1>
        //<Tex2>32896501</Tex2>
        //<Texture>19529</Texture>
        //<XpBoosted>0</XpBoosted>
        //<XpTimer>0</XpTimer>
        //<LDTimer>0</LDTimer>
        //<LTTimer>0</LTTimer>
        //<HasBackpack>1</HasBackpack>
        //<IsChallenger>0</IsChallenger>
        //<StarBackground>0</StarBackground>
        //<CreationDate>July 09, 2019</CreationDate>
        //</Char>
    }
}