using Domain.Models;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _repository;
        public CompetitionService(ICompetitionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> HasCompetition(string leagueCode)
        {
            return await _repository.HasCompetition(leagueCode);
        }

        public async Task<Competition> CreateAsync(Competition competition)
        {
            var createdCompetition = _repository.Add(competition);

            await _repository.UnitOfWork.SaveChangesAsync();

            return createdCompetition;
        }
    }
}
