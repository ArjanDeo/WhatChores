using DataAccess.Tables;
using Models.BattleNet.Public.Character;
using Models.RaiderIO.Character;
using Models.WhatChores.API;

namespace Models.WhatChores
{
    public class CharacterLookupModel
    {
      //  public Dictionary<int, int>? MythicKeystoneValues { get; set; }
        public RaiderIOCharacterDataModel? RaiderIOCharacterData { get; set; }
        public List<int>? DungeonVaultSlots { get; set; }
        public List<RaidEncounter>? RaidBossesKilledThisWeek { get; set; }
        public string? classColor { get; set; }
        public List<CharacterMediaModel>? CharacterMedia { get; set; }
       // public bool SuccessfullyRetrievedCharacter { get; set; }
       // public List<tbl_USRealms> RealmNames { get; set; }

     
    }
}
