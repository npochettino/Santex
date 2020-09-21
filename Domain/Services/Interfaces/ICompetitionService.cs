using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Interfaces
{
    public interface ICompetitionService
    {
        Task<Competition> CreateAsync(Competition competition);
        Task<bool> HasCompetition(string leagueCode);
    }
}
