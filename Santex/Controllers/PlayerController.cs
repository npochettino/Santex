using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain.Services;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Santex.Controllers
{
    public class PlayerController : SantexBaseController
    {
        private readonly IPlayerService _service;
        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        // GET: api/football
        [HttpGet("total-players")]
        public async Task<IActionResult> TotalPlayers(string leagueCode)
        {
            var result = await _service.GetTotalPlayers(leagueCode);
            if (result > 0)
                return Ok(new { total = result });
            return StatusCode((int)HttpStatusCode.NotFound, "Not found");
        }
    }
}