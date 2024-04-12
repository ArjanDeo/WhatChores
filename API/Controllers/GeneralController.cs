using DataAccess;
using DataAccess.Tables;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.BattleNet.OAuth;
using Models.BattleNet.Public;
using Models.BattleNet.Public.Character;
using Models.Constants;
using Models.RaiderIO.Character;
using Models.WhatChores;
using Models.WhatChores.API;
using Pathoschild.Http.Client;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/v1/general")]
    [ApiController]
    public class GeneralController(WhatChoresDbContext context, CachingService cache) : ControllerBase
    {
        private readonly WhatChoresDbContext _context = context;
        private readonly IAppCache _cache = cache;

        [HttpGet("realms")]
        [SwaggerOperation(
        Summary = "Gets Realms",
        Description = "Gets all the realms in the specified region",
        OperationId = "GetRealmData",
        Tags = ["General", "Get"]
        )]
        public async Task<List<RealmModel>> GetRealms()
        {
            var realmNames = await _context.tbl_USRealms
                                          .Select(r => new RealmModel { RealmName = r.RealmName })
                                          .ToListAsync();

            return realmNames;
        }

        [HttpGet("wowtoken")]
        [ResponseCache(Duration = 300)] // 5 minutes
        public async Task<ActionResult<WoWTokenPriceModel>> GetTokenPrice()
        {
            FluentClient client = new();
            if (AppConstants.AccessToken == null)
            {
               Dictionary<string,string> AccessTokenPayload = new()
                {
                    [":region"] = "us",
                    ["grant_type"] = "client_credentials"
                };

                AccessTokenModel Response = await client
                  .PostAsync("https://oauth.battle.net/oauth/token")
                  .WithBody(p => p.FormUrlEncoded(AccessTokenPayload))
                  .WithBasicAuthentication("97cd06eb96aa40e498af899ccfe65129", "o28W9L8PuJdl5AkKk44VJRZuDrYOzyYS")
                  .As<AccessTokenModel>();  
                
              AppConstants.AccessToken = Response;               
            }

            Dictionary<string,string> ApiPayload = new()
            {
                ["region"] = "us",
                ["namespace"] = "dynamic-us",
                ["locale"] = "en_US"
            };

            WoWTokenPriceModel ResponseData = await client
                .GetAsync("https://us.api.blizzard.com/data/wow/token/index")
                .WithOptions(ignoreHttpErrors: true)
                .WithArguments(ApiPayload)
                .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                .As<WoWTokenPriceModel>();


            //   WoWTokenPriceModel ResponseDataModel = await ResponseData.As<WoWTokenPriceModel>();

            // The price of wow token is, for some reason, returned with unnecessary 0's at the end (e.g. 360541000)
            // So here I divide it by 10000 to remove the last 3, then add comma separators for visibility.
            // This will need to be changed in the extremely unlikely event that the token reaches prices of over 1 million or less than 100 thousand.
            ResponseData.price /= 10000;

            return Ok(ResponseData);

        }
        [HttpGet("class")]
        public async Task<ActionResult<List<tbl_ClassData>>> GetClassData(bool getColor)
        {
            if (getColor)
            {
                var realmNames = await _context.tbl_ClassData
                .Select(c => new { c.ClassName, c.ClassColor })
                .ToListAsync();

                return Ok(realmNames);
            }
            else
            {
                var realmNames = await _context.tbl_ClassData
                    .Select(c => new { c.ClassName })
                    .ToListAsync();

                return Ok(realmNames);
            }
        }
        [HttpGet("charRaids")]
        public async Task<ActionResult<WoWCharacterRaidsModel>> GetCharacterRaids(string realm, string name, string region)
        {
            return await _cache.GetOrAddAsync($"GetCharacterRaids_{name}_{region}_{realm}", async () => // Caches for default time (20 mins)
            {
                FluentClient client = new();
                if (AppConstants.AccessToken == null)
                {
                    Dictionary<string, string> AccessTokenPayload = new()
                    {
                        [":region"] = "us",
                        ["grant_type"] = "client_credentials"
                    };

                    AccessTokenModel Response = await client
                      .PostAsync("https://oauth.battle.net/oauth/token")
                      .WithBody(p => p.FormUrlEncoded(AccessTokenPayload))
                      .WithBasicAuthentication("97cd06eb96aa40e498af899ccfe65129", "o28W9L8PuJdl5AkKk44VJRZuDrYOzyYS")
                      .As<AccessTokenModel>();

                    AppConstants.AccessToken = Response;
                }
                WoWCharacterRaidsModel data = await client
                    .GetAsync($"https://{region}.api.blizzard.com/profile/wow/character/{realm}/{name}/encounters/raids")
                    .WithArgument(":region", region)
                    .WithArgument("namespace", $"profile-{region}")
                    .WithArgument("locale", "en_US")
                    .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                    .As<WoWCharacterRaidsModel>();

                return Ok(data);
            });
        }

        [HttpGet("charData")]       
        public async Task<ActionResult<CharacterLookupModel>> GetCharacterData(string name, string region, string realm)
        {
            return await _cache.GetOrAddAsync($"GetCharacterData_{name}_{region}_{realm}", async () => // Caches for default time (20 mins)
            {
                FluentClient client = new();
                CharacterLookupModel characterLookupModel = new();
                RaiderIOCharacterDataModel ResponseData = await client
                      .GetAsync("https://raider.io/api/v1/characters/profile")
                      .WithArgument("region", region)
                      .WithArgument("name", name)
                      .WithArgument("realm", realm.Replace(" ", "-"))
                      .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,mythic_plus_scores_by_season:current,guild,gear")
                      .As<RaiderIOCharacterDataModel>();

                Dictionary<string, string> KeysData = await client
                    .GetAsync("https://localhost:7031/api/v1/mythicplus/keystone-vault-reward")
                    .As<Dictionary<string, string>>();

                var dictionary = new Dictionary<int, int>();

                foreach (var kvp in KeysData)
                {
                    dictionary.Add(int.Parse(kvp.Key), int.Parse(kvp.Value));
                }

                characterLookupModel.MythicKeystoneValues = dictionary;
                characterLookupModel.RaiderIOCharacterData = ResponseData;

                List<int> intList = await GetDungeonVaultSlots(ResponseData, characterLookupModel.MythicKeystoneValues);
                characterLookupModel.DungeonVaultSlots = intList;

                string color = await GetClassColor(characterLookupModel.RaiderIOCharacterData.char_class);
                characterLookupModel.classColor = color;

                characterLookupModel.SuccessfullyRetrievedCharacter = true;

                return Ok(characterLookupModel);
            });
           
        }
        public class ClassNameColor
        {
            public string ClassName { get; set; }
            public string ClassColor { get; set; }
        }
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
        private static async Task<string> GetClassColor(string char_class)
        {
            FluentClient client = new();            
            List<ClassNameColor> classMap = await client
              .GetAsync("https://localhost:7031/api/v1/general/class?getColor=true")
              .As<List<ClassNameColor>>();
           
            string color = "black"; // default

            
            foreach (var item in classMap)
            {
                if (item.ClassName == char_class)
                {
                    color = item.ClassColor; 
                    break;
                }
            }

            return color;
        }
    }
}
