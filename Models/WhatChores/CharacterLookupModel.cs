using Models.RaiderIO.Character;

namespace Models.WhatChores
{
    public class CharacterLookupModel
    {
        public Dictionary<int, int> MythicKeystoneValues { get; set; }
        public RaiderIOCharacterDataModel RaiderIOCharacterData { get; set; }
        public List<int> DungeonVaultSlots { get; set; }
        public string classColor { get; set; }
    }
}
