using Newtonsoft.Json;

namespace Models.RaiderIO.Character
{    
    public class AberrusTheShadowedCrucible
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class Affix
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string wowhead_url { get; set; }
    }

    public class AmirdrassilTheDreamsHope
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class AzeritePower
    {
        public int id { get; set; }
        public Spell spell { get; set; }
        public int tier { get; set; }
    }

    public class Back
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Chest
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<AzeritePower> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public string tier { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Corruption
    {
        public int added { get; set; }
        public int resisted { get; set; }
        public int total { get; set; }
        public int cloakRank { get; set; }
        public List<object> spells { get; set; }
    }

    public class Feet
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Finger1
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Finger2
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Gear
    {
        public DateTime updated_at { get; set; }
        public double item_level_equipped { get; set; }
        public int artifact_traits { get; set; }
        public Corruption corruption { get; set; }
        public Items items { get; set; }
    }

    public class Guild
    {
        public string name { get; set; }
        public string realm { get; set; }
    }

    public class Hands
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public string tier { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Head
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Items
    {
        public Head head { get; set; }
        public Neck neck { get; set; }
        public Shoulder shoulder { get; set; }
        public Back back { get; set; }
        public Chest chest { get; set; }
        public Waist waist { get; set; }
        public Wrist wrist { get; set; }
        public Hands hands { get; set; }
        public Legs legs { get; set; }
        public Feet feet { get; set; }
        public Finger1 finger1 { get; set; }
        public Finger2 finger2 { get; set; }
        public Trinket1 trinket1 { get; set; }
        public Trinket2 trinket2 { get; set; }
        public Mainhand mainhand { get; set; }
        public Offhand offhand { get; set; }
    }

    public class Legs
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public string tier { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Mainhand
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class MythicPlusWeeklyHighestLevelRun
    {
        public string dungeon { get; set; }
        public string short_name { get; set; }
        public int mythic_level { get; set; }
        public DateTime completed_at { get; set; }
        public int clear_time_ms { get; set; }
        public int par_time_ms { get; set; }
        public int num_keystone_upgrades { get; set; }
        public int map_challenge_mode_id { get; set; }
        public int zone_id { get; set; }
        public double score { get; set; }
        public List<Affix> affixes { get; set; }
        public string url { get; set; }
    }

    public class Neck
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Offhand
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class RaidProgression
    {
        [JsonProperty("aberrus-the-shadowed-crucible")]
        public AberrusTheShadowedCrucible aberrustheshadowedcrucible { get; set; }

        [JsonProperty("amirdrassil-the-dreams-hope")]
        public AmirdrassilTheDreamsHope amirdrassilthedreamshope { get; set; }

        [JsonProperty("vault-of-the-incarnates")]
        public VaultOfTheIncarnates vaultoftheincarnates { get; set; }
    }
    public class All
    {
        public double score { get; set; }
        public string color { get; set; }
    }

    public class Dps
    {
        public double score { get; set; }
        public string color { get; set; }
    }

    public class Healer
    {
        public int score { get; set; }
        public string color { get; set; }
    }

    public class MythicPlusScoresBySeason
    {
        public string season { get; set; }
        public Scores scores { get; set; }
        public Segments segments { get; set; }
    }

    public class RaiderIOCharacterDataModel
    {
        public string name { get; set; }
        public string race { get; set; }
        [JsonProperty("class")]
        public string char_class { get; set; } // reserved keyword 'class' manually changed to char class
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public int achievement_points { get; set; }
        public int honorable_kills { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public string profile_banner { get; set; }
        public List<MythicPlusWeeklyHighestLevelRun> mythic_plus_weekly_highest_level_runs { get; set; }
        public List<MythicPlusScoresBySeason> mythic_plus_scores_by_season { get; set; }

        public Gear gear { get; set; }
        public RaidProgression raid_progression { get; set; }
        public Guild guild { get; set; }
    }
    public class Scores
    {
        public double all { get; set; }
        public double dps { get; set; }
        public int healer { get; set; }
        public double tank { get; set; }
        public double spec_0 { get; set; }
        public double spec_1 { get; set; }
        public double spec_2 { get; set; }
        public int spec_3 { get; set; }
    }

    public class Segments
    {
        public All all { get; set; }
        public Dps dps { get; set; }
        public Healer healer { get; set; }
        public Tank tank { get; set; }
        public Spec0 spec_0 { get; set; }
        public Spec1 spec_1 { get; set; }
        public Spec2 spec_2 { get; set; }
        public Spec3 spec_3 { get; set; }
    }

    public class Spec0
    {
        public double score { get; set; }
        public string color { get; set; }
    }

    public class Spec1
    {
        public double score { get; set; }
        public string color { get; set; }
    }

    public class Spec2
    {
        public double score { get; set; }
        public string color { get; set; }
    }

    public class Spec3
    {
        public int score { get; set; }
        public string color { get; set; }
    }

    public class Tank
    {
        public double score { get; set; }
        public string color { get; set; }
    }
    public class Shoulder
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<AzeritePower> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public string tier { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Spell
    {
        public int id { get; set; }
        public int school { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public object rank { get; set; }
    }

    public class Trinket1
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Trinket2
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<object> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class VaultOfTheIncarnates
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class Waist
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }

    public class Wrist
    {
        public int item_id { get; set; }
        public int item_level { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public int item_quality { get; set; }
        public bool is_legendary { get; set; }
        public bool is_azerite_armor { get; set; }
        public List<object> azerite_powers { get; set; }
        public Corruption corruption { get; set; }
        public List<object> domination_shards { get; set; }
        public List<int> gems { get; set; }
        public List<int> bonuses { get; set; }
    }



}