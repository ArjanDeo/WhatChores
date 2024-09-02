namespace Models.BattleNet.OAuth
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AccessTokenModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string sub { get; set; }
        public DateTime acquired_at { get; set; }
    }


}
