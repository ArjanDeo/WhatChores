using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BattleNet.Public.Character
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class WoWCharacterMediaModelAsset
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class WoWCharacterMediaModelCharacter
    {
        public WoWCharacterMediaModelKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public WoWCharacterMediaModelRealm realm { get; set; }
    }

    public class WoWCharacterMediaModelKey
    {
        public string href { get; set; }
    }

    public class WoWCharacterMediaModelLinks
    {
        public WoWCharacterMediaModelSelf self { get; set; }
    }

    public class WoWCharacterMediaModelRealm
    {
        public WoWCharacterMediaModelKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class WoWCharacterMediaModel
    {
        public WoWCharacterMediaModelLinks _links { get; set; }
        public WoWCharacterMediaModelCharacter character { get; set; }
        public List<WoWCharacterMediaModelAsset> assets { get; set; }
    }

    public class WoWCharacterMediaModelSelf
    {
        public string href { get; set; }
    }


}
