using DataAccess.Tables;
using Models.BattleNet.Public.Character;
using Models.RaiderIO.Character;

namespace Models.WhatChores
{
    public class CharacterLookupModel
    {
        public Dictionary<int, int>? MythicKeystoneValues { get; set; }
        public RaiderIOCharacterDataModel? RaiderIOCharacterData { get; set; }
        public List<int>? DungeonVaultSlots { get; set; }
        public string? classColor { get; set; }
        public WoWCharacterMediaModel CharacterMedia { get; set; }
        public bool SuccessfullyRetrievedCharacter { get; set; }
        public List<tbl_USRealms> RealmNames { get; set; }

     
    }
}
