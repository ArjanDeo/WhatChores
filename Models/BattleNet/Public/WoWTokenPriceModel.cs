namespace Models.BattleNet.Public
{
    public class Links
    {
        public Self self { get; set; }
    }

    public class WoWTokenPriceModel
    {
        public Links _links { get; set; }
        public long last_updated_timestamp { get; set; }
        public long price { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

}
