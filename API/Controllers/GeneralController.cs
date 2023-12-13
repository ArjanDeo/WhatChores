using DataAccess;
using DataAccess.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.WhatChores.API;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/v1/general")]
    [ApiController]
    public class GeneralController(WhatChoresDbContext context) : ControllerBase
    {
        private readonly WhatChoresDbContext _context = context;

        [HttpGet("realms")]
        [SwaggerOperation(
        Summary = "Gets Realms",
        Description = "Gets all the realms in the specified region",
        OperationId = "GetRealmData",
        Tags = ["General", "Get"]
        )]
        public async Task<List<RealmModel>> Get()
        {
            var realmNames = await _context.tbl_USRealms
                                          .Select(r => new RealmModel { RealmName = r.RealmName })
                                          .ToListAsync();

            return realmNames;
        }
        [HttpGet("class")]
        public async Task<ActionResult<List<tbl_ClassData>>> GetRealmNames(bool getColor)
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



    }
}
