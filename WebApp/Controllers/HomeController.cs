using DataAccess;
using IdentityModel.Client;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.BattleNet.OAuth;
using Models.BattleNet.Public;
using Models.Constants;
using Models.RaiderIO.Character;
using Models.RaiderIO.MythicPlus;
using Models.WhatChores;
using Pathoschild.Http.Client;

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

        private readonly Dictionary<string, string> ApiPayload = [];
        private readonly Dictionary<string, string> AccessTokenPayload = [];
        private readonly string? clientId;
        private readonly string? clientSecret;

        private readonly WhatChoresDbContext _context;
        #endregion

        #region Constructor
        public HomeController(IConfiguration configuration, WhatChoresDbContext context)
        {
            _context = context;
            cache = new CachingService();
            client = new();
            charModel = new();
            overviewModel = new();
            _configuration = configuration;
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
           var realms = (from RealmName in _context.tbl_USRealms select RealmName).ToList();
            charModel.RealmNames = realms;
            return View(charModel);
        }

        #endregion

        #region API Requests (Get)

        private async Task<WoWTokenPriceModel> GetWoWToken()
        {
            return await cache.GetOrAddAsync("GetWoWToken", async () =>
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
            }, TimeSpan.FromMinutes(60));
            
        }      
      
        public async Task<RaiderIOMythicPlusAffixesModel> GetMythicPlusAffixes()
        {
            return await cache.GetOrAddAsync("MythicPlusAffixes", async () =>
            {
                IResponse ResponseData = await client
                .GetAsync("https://raider.io/api/v1/mythic-plus/affixes")
                .WithArgument("region", "us");

                RaiderIOMythicPlusAffixesModel ResponseDataModel = await ResponseData.As<RaiderIOMythicPlusAffixesModel>();
                return ResponseDataModel;

            }, TimeSpan.FromMinutes(60));
           
        }
        
        public async Task<IActionResult> GetCharacter(string name, string realm)
        {
            string cacheKey = $"GetCharacter_{name}_{realm}";

            return await cache.GetOrAddAsync(cacheKey, async () =>
            {
                try 
                {
                    IResponse ResponseData = await client
                        .GetAsync("https://raider.io/api/v1/characters/profile")
                        .WithArgument("region", "us")
                        .WithArgument("name", name)
                        .WithArgument("realm", realm.Replace(" ", "-"))
                        .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,guild");


                    RaiderIOCharacterDataModel response = await ResponseData.As<RaiderIOCharacterDataModel>();

                    charModel.MythicKeystoneValues = await _context.tbl_MythicPlusValues.ToDictionaryAsync(i => i.KeyLevel, i => i.ItemLevel); 
                    charModel.RaiderIOCharacterData = response;

                    List<int> intList = await GetDungeonVaultSlots(response, charModel.MythicKeystoneValues);                                
                    charModel.DungeonVaultSlots = intList;
                    string color = await GetClassColor();

                    charModel.classColor = color;

                    return View("CharacterLookup", charModel);
                }
                catch
                {
                    charModel.FailedToGetCharacter = true;
                    return View("CharacterLookup", charModel);
                }
            }, TimeSpan.FromMinutes(15));
        }
   
        #endregion

        #region API Requests (Post)
        

      
        public async Task<AccessTokenModel> PostAccessToken()
        {           
            string cacheKey = $"PostAccessToken";
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

        private static Task<List<int>> GetDungeonVaultSlots(RaiderIOCharacterDataModel characterData, Dictionary<int, int> mythicKeystoneValues)
        {
            List<int> intList = [];

            foreach (var run in characterData.mythic_plus_weekly_highest_level_runs)
            {
                if (mythicKeystoneValues.TryGetValue(run.mythic_level, out int value))
                {
                    intList.Add(value);
                }
                else if (run.mythic_level > 20)
                {
                    // If the key level is greater than 20, assign the maximum score
                    intList.Add(mythicKeystoneValues[20]);
                }
            }

            intList = [.. intList.OrderByDescending(x => x)];

            return Task.FromResult(intList);
        }


    public async Task<string> Test(string name)
        {
            return string.IsNullOrEmpty(name) ? "Name is null or empty" : "Name is not null or empty";
        }

        private async Task<string> GetClassColor()
        {
            Dictionary<string, string> classMap = await _context.tbl_ClassData.ToDictionaryAsync(i => i.ClassName, i => i.ClassColor);

            string color = classMap.TryGetValue(charModel.RaiderIOCharacterData.char_class, out string? value) ? value : "black";

            return color;
        }
        #endregion

        #region API Page
        public IActionResult Api()
        {
            return View();
        }
        #endregion
    }
}
