using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<int?> GetTotalPlayers(string leagueCode);
    }
}
