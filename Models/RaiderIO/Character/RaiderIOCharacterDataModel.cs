// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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

    public class Guild
    {
        public string name { get; set; }
        public string realm { get; set; }
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

    public class RaidProgression
    {
        [JsonProperty("aberrus-the-shadowed-crucible")]
        public AberrusTheShadowedCrucible aberrustheshadowedcrucible { get; set; }

        [JsonProperty("amirdrassil-the-dreams-hope")]
        public AmirdrassilTheDreamsHope amirdrassilthedreamshope { get; set; }

        [JsonProperty("vault-of-the-incarnates")]
        public VaultOfTheIncarnates vaultoftheincarnates { get; set; }
    }

    public class RaiderIOCharacterDataModel
    {
        public string name { get; set; }
        public string race { get; set; }
        [JsonProperty("class")]
        public string char_class { get; set; }
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
        public RaidProgression raid_progression { get; set; }
        public Guild guild { get; set; }
    }

    public class VaultOfTheIncarnates
    {
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

}