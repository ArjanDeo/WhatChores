using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.BattleNet.OAuth;
using Models.BattleNet.Public.Character.MythicKeystone;
using Models.Constants;
using Pathoschild.Http.Client;

namespace API.Controllers
{
    [Route("api/v1/mythicplus")]
    [ApiController]
    public class MythicPlusController(WhatChoresDbContext context, FluentClient client, IConfiguration configuration) : ControllerBase
    {
        private readonly WhatChoresDbContext _context = context;
        private readonly FluentClient _client = client;
        private readonly IConfiguration _configuration = configuration;

        [HttpGet("keystoneVaultReward")]
        public async Task<ActionResult<object>> KeystoneVaultReward(int? keyLevel)
        {
            // Check for keyLevel less than 2
            if (keyLevel.HasValue && keyLevel < 2)
            {
                return BadRequest("Key level should be at least 2.");
            }

            int originalKeyLevel = keyLevel.GetValueOrDefault();

            // Adjust keyLevel if it's greater than 10
            if (keyLevel.HasValue && keyLevel > 10)
            {
                keyLevel = 10;
            }

            if (!keyLevel.HasValue)
            {
                var allRewards = await _context.tbl_MythicPlusValues
                    .OrderBy(k => k.KeyLevel)
                    .ToDictionaryAsync(k => k.KeyLevel, i => i.ItemLevel);

                return Ok(allRewards);
            }
            else
            {
                // Query for the specific or adjusted key level
                var reward = await _context.tbl_MythicPlusValues
                    .Where(k => k.KeyLevel == keyLevel)
                    .Select(k => new { Key = k.KeyLevel, Value = k.ItemLevel })
                    .FirstOrDefaultAsync();

                if (reward == null)
                {
                    return NotFound($"No rewards found for key level {originalKeyLevel}.");
                }
               
                var result = new Dictionary<int, int>
                {            
                    { originalKeyLevel > 10 ? originalKeyLevel : reward.Key, reward.Value }
                };

                return Ok(result);
            }
        }
        [HttpGet("characterRunsLibrary")]
        public async Task<IActionResult> CharacterRunsLibrary(string name, string realm, string region)
        {
            await GetNewAccessToken();

            WoWCharacterMythicKeystoneModel data = await _client.
                GetAsync($"https://us.api.blizzard.com/profile/wow/character/{realm}/{name}/mythic-keystone-profile")
                .WithArgument("namespace", "profile-us")
                .WithArgument("locale", "en_US")
                .WithArgument(":region", region)
                .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                .As<WoWCharacterMythicKeystoneModel>();
            return Ok(data);
        }
        private async Task<bool> GetNewAccessToken()
        {
            // If there is an access token already, and it has not expired yet.
            if (AppConstants.AccessToken != null && AppConstants.AccessToken.acquired_at > DateTime.UtcNow.AddSeconds(AppConstants.AccessToken.expires_in))
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
                throw new Exception(ex.Message);
            }



        }
    }
}
