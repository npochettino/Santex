using Domain.Data;
using Domain.Models;
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
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly SantexContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public CompetitionRepository(SantexContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> HasCompetition(string leagueCode)
        {
            return await _context.Competitions.Where(c => c.Code == leagueCode).AnyAsync();
        }

        public Competition Add(Competition competition)
        {
            var _competition = _context.Attach(competition);
            _competition.State = EntityState.Added;
            return _competition.Entity;
        }
    }
}
