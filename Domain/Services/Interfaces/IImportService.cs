using Domain.Models.Outputs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IImportService
    {
        Task<ImportStatusEnum> ImportLeague(string leagueCode);
    }
}
