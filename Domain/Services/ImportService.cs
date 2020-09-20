using Domain.Models.Outputs;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ImportService : IImportService
    {
        public async Task<ImportStatusEnum> ImportLeague(string leagueCode)
        {
            return ImportStatusEnum.Successfully;
        }
    }
}
