using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<int> GetTotalPlayers(string leagueCode);
    }
}
