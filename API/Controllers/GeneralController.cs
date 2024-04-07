﻿using Azure;
using DataAccess;
using DataAccess.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.BattleNet.OAuth;
using Models.BattleNet.Public;
using Models.Constants;
using Models.WhatChores.API;
using Pathoschild.Http.Client;
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
        [HttpGet("wowtoken")]
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
