using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BattleNet.Public.Character
{
    public class Character
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
    }

    public class Difficulty
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Encounter
    {
        public Encounter encounter { get; set; }
        public int completed_count { get; set; }
        public object last_kill_timestamp { get; set; }
    }

    public class Encounter2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Expansion
    {
        public Expansion expansion { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Expansion2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Instance
    {
        public Instance instance { get; set; }
        public List<Mode> modes { get; set; }
    }

    public class Instance2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Key
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class Mode
    {
        public Difficulty difficulty { get; set; }
        public Status status { get; set; }
        public Progress progress { get; set; }
    }

    public class Progress
    {
        public int completed_count { get; set; }
        public int total_count { get; set; }
        public List<Encounter> encounters { get; set; }
    }

    public class Realm
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class WoWCharacterRaidsModel
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public List<Expansion> expansions { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Status
    {
        public string type { get; set; }
        public string name { get; set; }
    }
}
