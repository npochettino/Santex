using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class PlayerService : IPlayerService
    {
        IPlayerRepository _repository;
        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int?> GetTotalPlayers(string leagueCode)
        {
            return await _repository.GetTotalPlayers(leagueCode);
        }
    }
}
