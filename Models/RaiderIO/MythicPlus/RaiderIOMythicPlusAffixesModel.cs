namespace Models.RaiderIO.MythicPlus
{
    public class AffixDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string wowhead_url { get; set; }
    }

    public class RaiderIOMythicPlusAffixesModel
    {
        public string region { get; set; }
        public string title { get; set; }
        public string leaderboard_url { get; set; }
        public List<AffixDetail> affix_details { get; set; }
    }




}
