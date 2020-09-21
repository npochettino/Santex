using Domain.Models;
using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Interfaces
{
    public interface ICompetitionRepository : IRepository<Competition>
    {
        Competition Add(Competition competition);
        Task<bool> HasCompetition(string leagueCode);
    }
}
