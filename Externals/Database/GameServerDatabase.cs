using System;
using System.Collections.Generic;
using System.Linq;
using Externals.Models.FirestoreModels.Character;
using Externals.Models.GameDataModels;
using Externals.Utilities;

namespace Externals.Database
{
    public class GameServerDatabase : FirestoreDatabase, IDisposable
    {
        public GameServerDatabase()
        {

        }

        public CharacterModel CreateCharacter(int charType, int skinType, int accountId)
        {
            var acc = GetAccountById(accountId);
            var props = ObjectLibrary.GetProperties(charType);
            if (acc == null || props == null) return null;

            try
            {
                acc.NextCharacterId += 1;
                var chr = new CharacterModel()
                {
                    Name = acc.Name,
                    AccountId = acc.AccountId,
                    ObjectType = charType,
                    CharacterId = acc.NextCharacterId,
                    Accessory = 0,
                    IsChallenger = false,
                    Attack = 0,
                    Clothing = 0,
                    CreationDate = DateTime.UtcNow,
                    CurrentFame = 0,
                    Dead = false,
                    Defense = 0,
                    Dexterity = 0,
                    Equipment = props.Equipment,
                    Backpack = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, },
                    Experience = 0,
                    HasBackpack = false,
                    XpBoosted = false,
                    Hp = 0,
                    HpPots = 0,
                    Level = 0,
                    LootDropTimer = 0,
                    LootTierTimer = 0,
                    MaxHp = 0,
                    MaxMp = 0,
                    Mp = 0,
                    MpPots = 0,
                    Speed = 0,
                    StarBackground = true,
                    Texture = skinType,
                    Vitality = 0,
                    Wisdom = 0,
                    XpTimer = 0,
                };

                var x = chr.CreateDocument(Api);
                acc.UpdateDocument(Api);
            }
            catch (Exception e)
            {
                acc.NextCharacterId -= 1;
                LoggingUtils.LogWarningIfDebug("Character creation failed, re-setting next charId.");
            }
            return null;
        }

        public void Dispose()
        {

        }
    }
}