﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<string,string>>> GetClaims() => Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }
}