using DataAccess;
using DataAccess.Tables;
using HtmlAgilityPack;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.BattleNet.OAuth;
using Models.BattleNet.Public;
using Models.BattleNet.Public.Character;
using Models.Constants;
using Models.RaiderIO.Character;
using Models.WhatChores;
using Models.WhatChores.API;
using Newtonsoft.Json;
using Pathoschild.Http.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Controllers
{
    [Route("api/v1/general")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly WhatChoresDbContext _context;
        private readonly IAppCache _cache;
        private readonly IConfiguration _configuration;
        private readonly SettingsModel _settings;
        private readonly FluentClient _whatChoresClient;
        private readonly FluentClient _client;
        private readonly HtmlDocument _doc;
        public GeneralController(WhatChoresDbContext context, CachingService cache, IOptions<SettingsModel> settingsOptions, IConfiguration configuration, FluentClient client, HtmlDocument document)
        {
            _context = context;
            _cache = cache;
            _configuration = configuration;
            _settings = settingsOptions.Value;
            _whatChoresClient = new FluentClient(_settings.ApiUrl);
            _client = client;
            _doc = document;
        }


        [HttpGet("realms")]
        public async Task<List<RealmModel>> GetRealms()
        {
            var realmNames = await _context.tbl_USRealms
                .Select(r => new RealmModel { RealmName = r.RealmName })
                .ToListAsync();

            return realmNames;
        }
        [HttpGet("pingApi")]
        public ActionResult PingApi() => Ok("Api is active.");
        
        [HttpGet("updateCharacter")]
        public async Task<IActionResult> UpdateCharacter(string name, string realm, string region)
        {
            try
            {

                IResponse raiderIOCharacterData = await _client
                            .GetAsync("https://raider.io/api/v1/characters/profile")
                            .WithArgument("region", region)
                            .WithArgument("name", name)
                            .WithArgument("realm", realm.Replace(" ", "-"))
                            .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,mythic_plus_scores_by_season:current,guild,gear");

                tbl_Characters character = await _context.tbl_Characters.Where(c => c.Name == name && c.Realm == realm && c.Region == region).FirstOrDefaultAsync();

                if (character is null) return NotFound();

                character.CharacterData = await raiderIOCharacterData.AsString();
                character.LastUpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok();
            } catch (ApiException ex)
            {
                return ex.Status switch
                {
                    HttpStatusCode.OK => Ok(),
                    HttpStatusCode.BadRequest => BadRequest(),
                    HttpStatusCode.NotFound => NotFound(),
                    HttpStatusCode.Forbidden => Forbid(),
                    HttpStatusCode.Unauthorized => Unauthorized(),
                    HttpStatusCode.NoContent => NoContent(),
                    _ => StatusCode((int)ex.Status)
                };
        }

        }
        [HttpGet("pingCharacter")]
        public async Task<HttpStatusCode> PingCharacter(string name, string realm)
        {
            return await _cache.GetOrAddAsync($"PingCharacter_{realm}_{name}", async () =>
            {
                try
                {
                    RaiderIOCharacterDataModel data = await _client
                          .GetAsync("https://raider.io/api/v1/characters/profile")
                          .WithArgument("region", "us")
                          .WithArgument("name", name)
                          .WithArgument("realm", realm.Replace(" ", "-"))
                          .As<RaiderIOCharacterDataModel>();
                    return HttpStatusCode.OK;
                } catch (ApiException ex)
                {
                    return ex.Response.Status;
                }
            });
        }
        [HttpGet("wowtoken")]       
        public async Task<ActionResult<WoWTokenPriceModel>> GetTokenPrice()
        {
            return await _cache.GetOrAddAsync<ActionResult<WoWTokenPriceModel>>("GetToken", async () => 
            {

                await GetNewAccessToken();

                Dictionary<string, string> ApiPayload = new()
                {
                    ["region"] = "us",
                    ["namespace"] = "dynamic-us",
                    ["locale"] = "en_US"
                };

                WoWTokenPriceModel ResponseData = await _client
                    .GetAsync("https://us.api.blizzard.com/data/wow/token/index")
                    .WithOptions(ignoreHttpErrors: true)
                    .WithArguments(ApiPayload)
                    .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                    .As<WoWTokenPriceModel>();


                //   WoWTokenPriceModel ResponseDataModel = await ResponseData.As<WoWTokenPriceModel>();

                // The price of wow token is, for some reason, returned with unnecessary 0's at the end (e.g. 360541000)
                // So here I divide it by 10000 to remove the last 3.
                // This will need to be changed in the extremely unlikely event that the token reaches prices of over 1 million or less than 100 thousand.
                ResponseData.price /= 10000;

                return Ok(ResponseData);
            });
           

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

        private async Task<List<RaidEncounter>> GetCharacterRaids(string name, string realm, string region)
        {
            realm = realm.ToLower();
            name = name.ToLower();
            region = region.ToLower();
           
            await GetNewAccessToken();

            WoWCharacterRaidsModel data = await _client
                .GetAsync($"https://{region}.api.blizzard.com/profile/wow/character/{realm}/{name}/encounters/raids?namespace=profile-us&locale=en_US")
                .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                .As<WoWCharacterRaidsModel>();

            Dictionary<string, (DateTime Timestamp, string Difficulty)> lastKilledTimestamps = [];

            var responseEncounters = data.expansions
                .SelectMany(e => e.instances)
                .SelectMany(i => i.modes);

            List<string> validRaidBosses = await _context.tbl_VaultRaidBosses
                .Select(v => v.Boss)
                .ToListAsync();

            foreach (var encounter in responseEncounters)
            {
                string difficultyLevel = encounter.difficulty.name;
                foreach (var boss in encounter.progress.encounters)
                {
                    string bossName = boss.encounter.name;
                    long lastKillTimestamp = boss.last_kill_timestamp;

                    if (validRaidBosses.Contains(bossName))
                    {
                        DateTime lastKillDateTime = DateTimeOffset.FromUnixTimeMilliseconds(lastKillTimestamp).UtcDateTime;

                        if (lastKilledTimestamps.TryGetValue(bossName, out var existingEntry))
                        {
                            if (existingEntry.Timestamp < lastKillDateTime)
                            {
                                lastKilledTimestamps[bossName] = (lastKillDateTime, difficultyLevel);
                            }
                        }
                        else
                        {
                            lastKilledTimestamps[bossName] = (lastKillDateTime, difficultyLevel);
                        }
                    }
                }
            }

            List<RaidEncounter> clearedBossList = [];

            foreach (var entry in lastKilledTimestamps)
            {
                if (entry.Value.Timestamp > GetLastReset())
                {
                    clearedBossList.Add(new RaidEncounter
                    {
                        Boss = entry.Key,
                        Difficulty = entry.Value.Difficulty
                    });
                }
            }
            return clearedBossList;            
        }
        [HttpGet("character")]       
        public async Task<ActionResult<CharacterLookupModel>> GetCharacter(string name, string realm, string region)
        {
            name = name.ToLower();
            realm = realm.ToLower();
            region = region.ToLower();

            return await _cache.GetOrAddAsync<ActionResult<CharacterLookupModel>>($"GetCharacter_{name}_{region}_{realm}", async () => // Caches for default time (20 mins)
            {
                await GetNewAccessToken();

                CharacterLookupModel characterLookupModel = new();
                RaiderIOCharacterDataModel raiderIOCharacterDataModel = new();
                try
                {
                    var character = await _context.tbl_Characters.Where(x => x.Name == name && x.Realm == realm && x.Region == region).FirstOrDefaultAsync();
                    if (character is null)
                    {
                        var raiderIOCharacterData = await _client
                             .GetAsync("https://raider.io/api/v1/characters/profile")
                             .WithArgument("region", region)
                             .WithArgument("name", name)
                             .WithArgument("realm", realm.Replace(" ", "-"))
                             .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,mythic_plus_scores_by_season:current,guild,gear");

                        tbl_Characters characterModel = new tbl_Characters
                        {
                            CharacterData = await raiderIOCharacterData.AsString(),
                            CharacterId = Guid.NewGuid(),
                            CreatedAt = DateTime.UtcNow,
                            LastUpdatedAt = DateTime.UtcNow,
                            Name = name,
                            Realm = realm,
                            Region = region
                        };
                        await _context.tbl_Characters.AddAsync(characterModel);
                        await _context.SaveChangesAsync();

                        raiderIOCharacterDataModel = await raiderIOCharacterData.As<RaiderIOCharacterDataModel>();
                    }
                    else if (character.LastUpdatedAt <= DateTime.UtcNow.AddDays(-1)) // character needs updating
                    {
                        var raiderIOCharacterData = await _client
                            .GetAsync("https://raider.io/api/v1/characters/profile")
                            .WithArgument("region", region)
                            .WithArgument("name", name)
                            .WithArgument("realm", realm.Replace(" ", "-"))
                            .WithArgument("fields", "raid_progression,mythic_plus_weekly_highest_level_runs,mythic_plus_scores_by_season:current,guild,gear");

                        character.CharacterData = await raiderIOCharacterData.AsString();
                        character.LastUpdatedAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                        raiderIOCharacterDataModel = await raiderIOCharacterData.As<RaiderIOCharacterDataModel>();
                    }
                    else
                    {
                        raiderIOCharacterDataModel = JsonConvert.DeserializeObject<RaiderIOCharacterDataModel>(character.CharacterData);
                    }
                } catch (ApiException ex)
                {
                    return BadRequest($"Character not found. {ex.Message}");
                }
                characterLookupModel.RaidBossesKilledThisWeek = await GetCharacterRaids(name, realm, region);

                Dictionary<int, int> KeysData = await _whatChoresClient
                    .GetAsync($"api/v1/mythicplus/keystoneVaultReward")
                    .As<Dictionary<int, int>>();

              //  var dictionary = new Dictionary<int, int>();

             //   foreach (var kvp in KeysData)
            //    {
            //        dictionary.Add(int.Parse(kvp.Key), int.Parse(kvp.Value));
             //   }
                WoWCharacterMediaModel data = await _client
               .GetAsync($"https://{region}.api.blizzard.com/profile/wow/character/{realm}/{name}/character-media?namespace=profile-us&locale=en_US&:region=us")
               .WithBearerAuthentication(AppConstants.AccessToken.access_token)
               .As<WoWCharacterMediaModel>();

                foreach (var image in data.assets)
                {
                    CharacterMediaModel characterMediaModel = new()
                    {
                        Link = image.value,
                        Type = image.key,
                    };
                    if (characterLookupModel.CharacterMedia is null)
                    {
                        characterLookupModel.CharacterMedia = [characterMediaModel];

                    } else
                    {
                        characterLookupModel.CharacterMedia.Add(characterMediaModel);
                    }
                }

               // characterLookupModel.CharacterMedia = data;
                characterLookupModel.RaiderIOCharacterData = raiderIOCharacterDataModel;

                List<int> dungeonVaultSlotsList = await GetDungeonVaultSlots(raiderIOCharacterDataModel, KeysData);
                characterLookupModel.DungeonVaultSlots = dungeonVaultSlotsList;

                string color = await GetClassColor(characterLookupModel.RaiderIOCharacterData.char_class);
                characterLookupModel.classColor = color;

                //characterLookupModel.SuccessfullyRetrievedCharacter = true;

                return Ok(characterLookupModel);
            });
           
        }
        [HttpGet("wowNews")]
        public async Task<ActionResult<List<NewsModel>>> GetWoWNews(int limit)
        {
            return await _cache.GetOrAddAsync<ActionResult<List<NewsModel>>>($"GetWoWNews_{limit}", async () =>
            {
                var newsPage = await _client.GetAsync("https://news.blizzard.com/en-gb/world-of-warcraft").AsString();

                _doc.LoadHtml(newsPage);

                var ol = _doc.DocumentNode.SelectSingleNode("//ol[@class='ArticleList']");

                HtmlNodeCollection liNodes = ol.SelectNodes(".//li");
                List<NewsModel> newsPosts = [];

                if (liNodes is not null && limit is not 0)
                {
                    for (int node = 0; node < limit; node++ )
                    {
                        NewsModel news = new NewsModel();
                        // string title = node.SelectSingleNode(".//h2/a")?.InnerText.Trim();                    
                        string backgroundImageStyle = liNodes[node].SelectSingleNode(".//div[@class='ArticleListItem-image']")?.Attributes["style"]?.Value;
                        news.Image = backgroundImageStyle;
                        int startIndex = news.Image.IndexOf("url(") + 4;
                        int endIndex = news.Image.LastIndexOf(")");
                        string extractedUrl = news.Image[startIndex..endIndex];
                        string link = liNodes[node].SelectSingleNode(".//h2/a")?.Attributes["href"]?.Value;

                        link = link.Replace("world-of-warcraft", "news");
                        link = "https://worldofwarcraft.blizzard.com" + link;

                        news.Image = "https:" + extractedUrl;
                        news.Title = liNodes[node].SelectSingleNode(".//h2/a")?.InnerText.Trim();
                        news.Link = link;
                        news.Description = liNodes[node].SelectSingleNode(".//div[@class='ArticleListItem-description']")?.InnerText.Trim();
                        newsPosts.Add(news);
                    }
                    return Ok(newsPosts);
                } else if (liNodes is not null)
                {
                    foreach (var node in liNodes)
                    {
                        NewsModel news = new NewsModel();
                        // string title = node.SelectSingleNode(".//h2/a")?.InnerText.Trim();                    
                        string backgroundImageStyle = node.SelectSingleNode(".//div[@class='ArticleListItem-image']")?.Attributes["style"]?.Value;
                        news.Image = backgroundImageStyle;
                        int startIndex = news.Image.IndexOf("url(") + 4;
                        int endIndex = news.Image.LastIndexOf(")");
                        string extractedUrl = news.Image[startIndex..endIndex];
                        string link = node.SelectSingleNode(".//h2/a")?.Attributes["href"]?.Value;

                        link = link.Replace("world-of-warcraft", "news");
                        link = "https://worldofwarcraft.blizzard.com" + link;

                        news.Image = "https:" + extractedUrl;
                        news.Title = node.SelectSingleNode(".//h2/a")?.InnerText.Trim();
                        news.Link = link;
                        news.Description = node.SelectSingleNode(".//div[@class='ArticleListItem-description']")?.InnerText.Trim();
                        newsPosts.Add(news);
                    }
                    return Ok(newsPosts);
                   
                }
                return NotFound();
            }, DateTimeOffset.Now.AddHours(1));
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
                else if (run.mythic_level > 10)
                {
                    // If the key level is greater than 10, assign the maximum score
                    intList.Add(mythicKeystoneValues[10]);
                }
            }

            intList = [.. intList.OrderByDescending(x => x)];

            return Task.FromResult(intList);
        }
        private async Task<string> GetClassColor(string char_class)
        {                     
            List<ClassNameColor> classMap = await _whatChoresClient
              .GetAsync("api/v1/general/class?getColor=true")
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
        public static DateTime GetLastReset()
        {
            DateTime lastTuesday = DateTime.Now.AddDays(-1);
            while (lastTuesday.DayOfWeek != DayOfWeek.Tuesday)
            {
                lastTuesday = lastTuesday.AddDays(-1);
            }
            return lastTuesday;
        }
        private async Task<bool> GetNewAccessToken()
        {
            // If there is an access token already, and it has not expired yet.
            if (AppConstants.AccessToken != null && AppConstants.AccessToken.acquired_at < DateTime.UtcNow.AddSeconds(AppConstants.AccessToken.expires_in))
            {
                return true;
            }
            Dictionary<string, string> AccessTokenPayload = new()
            {
                [":region"] = "us",
                ["grant_type"] = "client_credentials"
            };
            string clientId = _configuration.GetSection("OAuthCredentials:BattleNet:ClientId").Value;
            string clientSecret = _configuration.GetSection("OAuthCredentials:BattleNet:ClientSecret").Value;
            try
            {
               AccessTokenModel Response = await _client
              .PostAsync("https://us.battle.net/oauth/token")
              .WithBody(p => p.FormUrlEncoded(AccessTokenPayload))
              .WithBasicAuthentication(clientId, clientSecret)
              .As<AccessTokenModel>();
                if (Response != null && Response.access_token != null)
                {

                    AppConstants.AccessToken = Response;
                    AppConstants.AccessToken.acquired_at = DateTime.UtcNow;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ApiException ex)
            {
                string responseText = await ex.Response.AsString();
                throw new Exception($"The API responded with HTTP {ex.Response.Status}: {responseText}");
            }



        }
    }
}
