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
    public class ImportController : SantexBaseController
    {
        private readonly IImportService _service;
        public ImportController(IImportService service)
        {
            _service = service;
        }

        // GET: api/football
        [HttpGet("import-league")]
        public async Task<IActionResult> ImportLeague(string leagueCode)
        {
            var result = await _service.ImportLeague(leagueCode);

            return result switch
            {
                Domain.Models.Outputs.ImportStatusEnum.Successfully => StatusCode((int)HttpStatusCode.Created, "Successfully imported"),
                Domain.Models.Outputs.ImportStatusEnum.Already => StatusCode((int)HttpStatusCode.Conflict, "League already imported"),
                Domain.Models.Outputs.ImportStatusEnum.NotFound => StatusCode((int)HttpStatusCode.NotFound, "Not found"),
                Domain.Models.Outputs.ImportStatusEnum.Error => StatusCode((int)HttpStatusCode.GatewayTimeout, "Server Error"),
                _ => StatusCode((int)HttpStatusCode.GatewayTimeout, "Server Error"),
            };
        }
    }
}