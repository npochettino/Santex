using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Outputs;
using Domain.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Santex.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IImportService _importService;
        public DefaultController(IPlayerService playerService, IImportService importService)
        {
            _playerService = playerService;
            _importService = importService;
        }

        [HttpGet("/import-league/{leagueCode}")]
        public async Task<IActionResult> ImportLeague(string leagueCode)
        {
            try
            {
                var result = (await _importService.ImportLeague(leagueCode));
                return result switch
                {
                    ImportStatusEnum.Successfull => StatusCode(201, "Successfully imported"),
                    ImportStatusEnum.Already => StatusCode(409, "League already imported"),
                    ImportStatusEnum.NotFound => StatusCode(404, "Not found"),
                    _ => StatusCode(504, "Server Error"),
                };
            }
            catch (Exception)
            {
                return StatusCode(504, "Server Error");
            }
        }

        [HttpGet("/total-players/{leagueCode}")]
        public async Task<IActionResult> TotalPlayers(string leagueCode)
        {
            try
            {
                var count = await _playerService.GetTotalPlayers(leagueCode);
                if (!count.HasValue) 
                    return StatusCode(404, "Not found");
                return Ok(new { total = count });
            }
            catch (Exception)
            {
                return StatusCode(504, "Server Error");
            }
        }
    }
}