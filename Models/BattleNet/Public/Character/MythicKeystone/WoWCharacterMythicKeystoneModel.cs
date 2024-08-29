using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BattleNet.Public.Character.MythicKeystone
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Character
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
    }

    public class Color
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public double a { get; set; }
    }

    public class CurrentMythicRating
    {
        public Color color { get; set; }
        public double rating { get; set; }
    }

    public class CurrentPeriod
    {
        public Period period { get; set; }
    }

    public class Key
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Period
    {
        public Key key { get; set; }
        public int id { get; set; }
    }

    public class Realm
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class WoWCharacterMythicKeystoneModel
    {
        public Links _links { get; set; }
        public CurrentPeriod current_period { get; set; }
        public List<Season> seasons { get; set; }
        public Character character { get; set; }
        public CurrentMythicRating current_mythic_rating { get; set; }
    }

    public class Season
    {
        public Key key { get; set; }
        public int id { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }


}
