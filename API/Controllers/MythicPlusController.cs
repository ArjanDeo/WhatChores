using DataAccess;
using DataAccess.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/v1/mythicplus")]
    [ApiController]
    public class MythicPlusController : ControllerBase
    {
        private readonly WhatChoresDbContext _context;

        public MythicPlusController(WhatChoresDbContext context)
        {
            _context = context;
        }

        [HttpGet("keystone-vault-reward")]
        public async Task<ActionResult<object>> Get(int? keyLevel)
        {
            // Check for keyLevel less than 2
            if (keyLevel.HasValue && keyLevel < 2)
            {
                return BadRequest("Key level should be between 2 and 20.");
            }

            int originalKeyLevel = keyLevel.GetValueOrDefault(); // Store the original input keyLevel

            // Adjust keyLevel if it's greater than 20
            if (keyLevel.HasValue && keyLevel > 20)
            {
                keyLevel = 20;
            }

            if (!keyLevel.HasValue)
            {
                // Return all key levels if keyLevel is null
                var allRewards = await _context.tbl_MythicPlusValues
                    .OrderBy(k => k.KeyLevel) // Assuming you want them ordered
                    .ToDictionaryAsync(k => k.KeyLevel.ToString(), i => i.ItemLevel.ToString());

                return Ok(allRewards);
            }
            else
            {
                // Query for the specific or adjusted key level
                var reward = await _context.tbl_MythicPlusValues
                    .Where(k => k.KeyLevel == keyLevel)
                    .Select(k => new { Key = k.KeyLevel.ToString(), Value = k.ItemLevel.ToString() })
                    .FirstOrDefaultAsync();

                if (reward == null)
                {
                    return NotFound($"No rewards found for key level {originalKeyLevel}.");
                }

                // Return in the specified format
                var result = new Dictionary<string, string>
        {
            // Use originalKeyLevel in the response if it was greater than 20
            { originalKeyLevel > 20 ? originalKeyLevel.ToString() : reward.Key, reward.Value }
        };

                return Ok(result);
            }
        }

    }
}
