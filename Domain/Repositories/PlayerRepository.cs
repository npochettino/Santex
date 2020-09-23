using Domain.Data;
using Domain.Repositories.Interfaces;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly SantexContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public PlayerRepository(SantexContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int?> GetTotalPlayers(string leagueCode)
        {
            var result = await _context.Competitions
                .Include(c => c.Teams)
                    .ThenInclude(t => t.Players)
                .Where(c => c.Code == leagueCode.ToUpper() || c.Code == leagueCode)
                .FirstOrDefaultAsync();

            if (result == null)
                return null;
                
            return result.Teams.Sum(t => t.Players.Count());
        }
    }
}
