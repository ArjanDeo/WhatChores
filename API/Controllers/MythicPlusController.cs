using DataAccess;
using DataAccess.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.WhatChores.API;

namespace API.Controllers
{
    [Route("api/v1/mythicplus")]
    [ApiController]
    [Produces("application/json")]
    public class MythicPlusController : ControllerBase
    {
        private readonly WhatChoresDbContext _context;

        public MythicPlusController(WhatChoresDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the vault reward ilvl for a specific M+ key level.
        /// </summary>
        /// <param name="keyLevel"></param>
        /// <response code="200">Returns the vault reward ilvl for provided key level (all key levels shown if none specified)</response>
        /// <response code="400">If an invalid key level was entered. (NaN or below 2)</response>
        /// <response code="404">If the key level wasn't found</response>
        [HttpGet("keystone-vault-reward")]
        [ProducesResponseType(typeof(tbl_MythicPlusValues), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 404)]
        public async Task<ActionResult<List<tbl_MythicPlusValues>>> Get(int? keyLevel)
        {
            IQueryable<tbl_MythicPlusValues> query = _context.tbl_MythicPlusValues;

            // if keyLevel is NaN or less than 2
            if (keyLevel.HasValue && (keyLevel < 2))
            {
                Error error = new()
                {
                    ErrorCode = 400,
                    ErrorMessage = "Key level must be 2 or higher."
                };
                return BadRequest(error);
            }

            if (keyLevel.HasValue && keyLevel < 20)
            {
                query = query.Where(k => k.KeyLevel == keyLevel.Value);
                var data = await query.FirstOrDefaultAsync();
                return Ok(new { keyLevel, itemLevel = data.ItemLevel });

            }

            // if keyLevel is greater than 20
            else if (keyLevel.HasValue && (keyLevel > 20))
            {
                query = query.Where(k => k.KeyLevel == 20);
                var data = await query.FirstOrDefaultAsync();              
               
                return Ok(new { keyLevel, itemLevel = data.ItemLevel });
            }         

            // if there was an issue finding data in the database (very bad)
            if (query == null)
            {
                return NotFound(); 
            }

            return Ok(query);
        }

    }
}
