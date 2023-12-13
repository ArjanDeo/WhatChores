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

        [HttpGet("vault-ilvl-by-keylevel")]       
        public async Task<ActionResult<object>> Get(int? keyLevel)
        {
            if (keyLevel.HasValue && (keyLevel < 2 || keyLevel > 20))
            {
                return BadRequest("Key level should be between 2 and 20.");
            }

            IQueryable<tbl_MythicPlusValues> query = _context.tbl_MythicPlusValues;

            if (keyLevel != null)
            {
                query = query.Where(k => k.KeyLevel == keyLevel);
            }

            var data = await query.ToDictionaryAsync(k => "+" + k.KeyLevel, i => i.ItemLevel);
            var formattedData = new
            {
                vaultIlvl = data,
            };

            return Ok(formattedData);
        }


    }
}
