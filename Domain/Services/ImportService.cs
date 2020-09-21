using AutoMapper;
using Domain.Models;
using Domain.Models.Outputs;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.Api;

namespace Domain.Services
{
    public class ImportService : IImportService
    {
        private ICompetitionService _competitionService;
        private IApiService _apiService;
        private IMapper _mapper;
        private readonly IHttpClientFactory _clientFactory;
        public ImportService(ICompetitionService competitionService, IApiService apiService, IMapper mapper, IHttpClientFactory clientFactory)
        {
            _competitionService = competitionService;
            _apiService = apiService;
            _mapper = mapper;
            _clientFactory = clientFactory;
        }

        public async Task<ImportStatusEnum> ImportLeague(string leagueCode)
        {
            var exist = await _competitionService.HasCompetition(leagueCode);

            if (exist)
                return ImportStatusEnum.Already;
            else
            {
                try
                {
                    var result = GetCompetitionByCode(leagueCode);
                }
                catch (Exception)
                {
                    return ImportStatusEnum.Error;
                }
            }

            return ImportStatusEnum.Error;
        }

        private async Task<Competition> GetCompetitionByCode(string leagueCode)
        {
            var competition = new Competition();
            var teams = new List<Team>();

            var request = new HttpRequestMessage(HttpMethod.Get, "competitions/" + leagueCode + "/teams");

            var _client = _clientFactory.CreateClient("football");

            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest) return competition;
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                dynamic teamsResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());

                competition.Id = int.Parse(teamsResult.competition.id.ToString());
                competition.Name = teamsResult.competition.name;
                competition.Code = teamsResult.competition.code;
                competition.AreaName = teamsResult.competition.area.name;

                FillTeams(teams, teamsResult);
            }

            await FillSquadAsync(teams);
           // await this.competitionService.CreateFullCompetitionAsync(competition, teams);

            return competition;
        }

        private void FillTeams(List<Team> teams, dynamic teamsResult)
        {
            if (teamsResult.teams != null)
            {
                foreach (var team in teamsResult.teams)
                {
                    var newTeam = new Team()
                    {
                        AreaName = team.area.name,
                        Id = int.Parse(team.id.ToString()),
                        Name = team.name,
                        ShortName = team.shortName,
                        Email = team.email
                    };
                    teams.Add(newTeam);
                }
            }
        }

        private async Task<List<Team>> FillSquadAsync(List<Team> teams)
        {
            var listTasks = new List<Task<Team>>();

            Thread.Sleep(60000);
            for (int i = 1; i <= teams.Count; i++)
            {
                if (i % 3 == 0)
                {
                    await Task.WhenAll(listTasks);
                    listTasks.Clear();
                    Thread.Sleep(60000);
                }
                listTasks.Add(FillSquad(teams[i - 1]));
            }

            return teams;
        }

        private async Task<Team> FillSquad(Team team)
        {
            if (team == null) return null;
            if (team.Players == null) team.Players = new List<Player>();

            var request = new HttpRequestMessage(HttpMethod.Get, "teams/" + team.Id);

            var _client = _clientFactory.CreateClient("football");

            using (var response = await _client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic teamResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                    foreach (var player in teamResult.squad)
                    {
                        var newPlayer = new Player();
                        try
                        {
                            newPlayer.Id = int.Parse(player.id.ToString());
                            newPlayer.Name = player.name;
                            newPlayer.CountryOfBirth = player.countryOfBirth;
                            newPlayer.Nationality = player.nationality;
                            newPlayer.Position = player.position ?? string.Empty;
                            newPlayer.DateOfBirth = player.dateOfBirth == null ? null :
                                DateTime.Parse(player.dateOfBirth.ToString());
                            newPlayer.Team = team;

                            team.Players.Add(newPlayer);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Error on playerId: {newPlayer.Id}", e);
                        }
                    }
                    return team;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error getting team Id: {team.Id}", e);
                }
            }
        }
    }
}
