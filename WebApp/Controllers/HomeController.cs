using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Pathoschild.Http.Client;
using System.Diagnostics;
using System.Reflection;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        #region Object Declarations
        private readonly FluentClient client;
        private readonly CharacterLookupModel charModel;
        private readonly OverviewModel overviewModel;
        private readonly IAppCache cache;
        private readonly IConfiguration _configuration;
        private readonly Stopwatch stopwatch;

        readonly Dictionary<int, int> MythicKeystoneValues = [];
        readonly Dictionary<string, string> ApiPayload = [];
        readonly Dictionary<string, string> AccessTokenPayload = [];

        private readonly string? clientId;
        private readonly string? clientSecret;
        #endregion

        #region Constructor
        public HomeController(IConfiguration configuration)
        {
            stopwatch = new();
            cache = new CachingService();
            client = new();
            charModel = new();
            overviewModel = new();
            _configuration = configuration;
            MythicKeystoneValues = new()
            {
            { 2, 454 },
            { 3, 457 },
            { 4, 460 },
            { 5, 460 },
            { 6, 463 },
            { 7, 463 },
            { 8, 467 },
            { 9, 467 },
            { 10, 470 },
            { 11, 470 },
            { 12, 473 },
            { 13, 473 },
            { 14, 473 },
            { 15, 476 },
            { 16, 476 },
            { 17, 476 },
            { 18, 480 },
            { 19, 480 },
            { 20, 483 }
        };
            ApiPayload = new()
            {
                ["region"] = "us",
                ["namespace"] = "dynamic-us",
                ["locale"] = "en_US"
            };
            AccessTokenPayload = new()
            {
                [":region"] = "us",
                ["grant_type"] = "client_credentials"
            };
            clientId = _configuration.GetValue<string>("ClientId");
            clientSecret = _configuration.GetValue<string>("ClientSecret");
        }
        #endregion

        #region Views

        public async Task<IActionResult> Index()
        {
            
            if (AppConstants.AccessToken is null) { await PostAccessToken(); }
            
            
            RaiderIOMythicPlusAffixesModel affixData = await GetMythicPlusAffixes();
            WoWTokenPriceModel tokenData = await GetWoWToken();

            overviewModel.AffixData = affixData;
            overviewModel.WoWTokenData = tokenData;
            return View(overviewModel);

        }

        public IActionResult CharacterLookup()
        {          
            return View();
        }

        #endregion

        #region API Requests (Get)

        private async Task<WoWTokenPriceModel> GetWoWToken()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string cacheKey = $"GetWoWToken_{methodName}";

            return await cache.GetOrAddAsync(cacheKey, async () =>
            {
                IResponse ResponseData = await client
                .GetAsync("https://us.api.blizzard.com/data/wow/token/index")
                .WithOptions(ignoreHttpErrors: true)
                .WithArguments(ApiPayload)
                .WithBearerAuthentication(AppConstants.AccessToken.access_token);

                WoWTokenPriceModel ResponseDataModel = await ResponseData.As<WoWTokenPriceModel>();

                // The price of wow token is, for some reason, returned with unnecessary 0's at the end (e.g. 360541000)
                // So here I divide it by 10000 to remove the last 3, then add comma separators for visibility.
                // This will need to be changed in the extremely unlikely event that the token reaches prices of over 1 million or less than 100 thousand.
                ResponseDataModel.price /= 10000;

                return ResponseDataModel;
            }, TimeSpan.FromMinutes(5));
            
        }      
      
        public async Task<RaiderIOMythicPlusAffixesModel> GetMythicPlusAffixes()
        {           
            string cacheKey = $"MythicPlusAffixes";

            return await cache.GetOrAddAsync(cacheKey, async () =>
            {
                IResponse ResponseData = await client
                .GetAsync("https://raider.io/api/v1/mythic-plus/affixes")
                .WithArgument("region", "us");

                RaiderIOMythicPlusAffixesModel ResponseDataModel = await ResponseData.As<RaiderIOMythicPlusAffixesModel>();
                return ResponseDataModel;

            }, TimeSpan.FromMinutes(20));
           
        }
        
        public async Task<IActionResult> GetCharacter(string name, string realm)
        {
            string cacheKey = $"GetCharacter_{name}_{realm}";

            return await cache.GetOrAddAsync(cacheKey, async () =>
            {
                IResponse ResponseData = await client
                    .GetAsync("https://raider.io/api/v1/characters/profile")
                    .WithArgument("region", "us")
                    .WithArgument("name", name)
                    .WithArgument("realm", realm)
                    .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs");

                RaiderIOCharacterDataModel response = await ResponseData.As<RaiderIOCharacterDataModel>();

                charModel.MythicKeystoneValues = MythicKeystoneValues;
                charModel.RaiderIOCharacterData = response;

                List<int> intList = await GetDungeonVaultSlots(response, charModel.MythicKeystoneValues);
                charModel.DungeonVaultSlots = intList;
                string color = await GetClassColor();

                charModel.classColor = color;

                return View("CharacterLookup", charModel);


            }, TimeSpan.FromMinutes(15));
        }
   
        #endregion

        #region API Requests (Post)
        
        public async Task<AccessTokenModel> PostAccessToken()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            string cacheKey = $"MythicPlusAffixes_{methodName}";
            return await cache.GetOrAddAsync(cacheKey, async () =>
            {
                IResponse Response = await client
               .PostAsync("https://oauth.battle.net/oauth/token")
               .WithBody(p => p.FormUrlEncoded(AccessTokenPayload))
               .WithBasicAuthentication(clientId, clientSecret);

                AccessTokenModel ResponseData = await Response.As<AccessTokenModel>();
                AppConstants.AccessToken = ResponseData;
                return ResponseData;
            }, TimeSpan.FromMinutes(120));
           
        }

        #endregion

        #region Functions
        private Task<List<int>> GetDungeonVaultSlots(RaiderIOCharacterDataModel characterData, Dictionary<int, int> mythicKeystoneValues)
        {
            List<int> intList = [];
            foreach (var run in characterData.mythic_plus_weekly_highest_level_runs)
            {
                if (mythicKeystoneValues.TryGetValue(run.mythic_level, out int value))
                {
                    intList.Add(value);
                }
            }

            intList = [.. intList.OrderByDescending(x => x)];
            return Task.FromResult(intList);
        }

        public async Task<string> Test(string name)
        {
            return string.IsNullOrEmpty(name) ? "Name is null or empty" : "Name is not null or empty";
        }

        private Task<string> GetClassColor()
        {
            Dictionary<string, string> classMap = new()
            {
                { "Warrior", "brown"},
                { "Hunter", "olive"},
                { "Mage", "aqua"},
                { "Rogue", "yellow"},
                { "Priest", "white" },
                { "Warlock", "purple" },
                { "Paladin", "pink" },
                { "Druid", "orange"},
                { "Shaman", "blue" },
                { "Monk", "green" },
                { "Demon Hunter", "purple" },
                { "Death Knight", "red" },
                { "Evoker", "green" }
            };
            string color = classMap.TryGetValue(charModel.RaiderIOCharacterData.char_class, out string? value) ? value : "black";

            return Task.FromResult(color);
        }
        #endregion

        #region Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }
        #endregion
    }
}
