using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.BattleNet.Public.Character.MythicKeystone;
using Models.Constants;
using Pathoschild.Http.Client;

namespace API.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class MythicPlusController(WhatChoresDbContext context, FluentClient client) : ControllerBase
    {
        private readonly WhatChoresDbContext _context = context;       
        private readonly FluentClient _client = client;
        [HttpGet("keystone-vault-reward")]
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
        [HttpGet]
        public async Task<IActionResult> CharacterRunsLibrary(string name, string realm, string region)
        {
            WoWCharacterMythicKeystoneModel data = await _client.
                GetAsync($"https://us.api.blizzard.com/profile/wow/character/{realm}/{name}/mythic-keystone-profile")
                .WithArgument("namespace", "profile-us")
                .WithArgument("locale", "en_US")
                .WithArgument(":region", region)
                .WithBearerAuthentication(AppConstants.AccessToken.access_token)
                .As<WoWCharacterMythicKeystoneModel>();
            return Ok(data);
        }
    }
}
