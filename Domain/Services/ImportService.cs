using Domain.Models;
using Domain.Models.Outputs;
using Domain.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ImportService : IImportService
    {
        private readonly ICompetitionService _competitionService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly Config _config;

        public ImportService(ICompetitionService competitionService, IHttpClientFactory clientFactory, IOptionsMonitor<Config> config)
        {
            _competitionService = competitionService;
            _clientFactory = clientFactory;
            _config = config.CurrentValue;
        }

        public async Task<ImportStatusEnum> ImportLeague(string leagueCode)
        {
            var exist = await _competitionService.HasCompetition(leagueCode);

            if (exist)
                return ImportStatusEnum.Already;

            var competition = new Competition();
            var teams = new List<Team>();

            var request = new HttpRequestMessage(HttpMethod.Get, "competitions/" + leagueCode + "/teams");

            var _client = _clientFactory.CreateClient("football");

            using (HttpResponseMessage response = await _client.SendAsync(request))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest) 
                    return ImportStatusEnum.NotFound;
                
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                dynamic teamsResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());

                competition.IdService = int.Parse(teamsResult.competition.id.ToString());
                competition.Name = teamsResult.competition.name;
                competition.Code = teamsResult.competition.code;
                competition.AreaName = teamsResult.competition.area.name;

                GetTeams(teams, teamsResult);
            }

            var listTasks = new List<Task<Team>>();
            foreach (var team in teams)
            {
                Thread.Sleep(_config.IntervalsMs);
                listTasks.Add(GetPlayers(team));
            }

            competition.Teams = teams;

            await _competitionService.CreateAsync(competition);

            return ImportStatusEnum.Successfull;
        }

        private void GetTeams(List<Team> teams, dynamic teamsResult)
        {
            if (teamsResult.teams != null)
            {
                foreach (var team in teamsResult.teams)
                {
                    var newTeam = new Team()
                    {
                        AreaName = team.area.name,
                        IdService = int.Parse(team.id.ToString()),
                        Name = team.name,
                        ShortName = team.shortName,
                        Email = team.email
                    };
                    teams.Add(newTeam);
                }
            }
        }

        private async Task<Team> GetPlayers(Team team)
        {
            if (team == null) return null;
            if (team.Players == null) team.Players = new List<Player>();

            var request = new HttpRequestMessage(HttpMethod.Get, "teams/" + team.IdService);

            var _client = _clientFactory.CreateClient("football");

            using var response = await _client.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                dynamic teamResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                foreach (var player in teamResult.squad)
                {
                    var newPlayer = new Player
                    {
                        IdService = int.Parse(player.id.ToString()),
                        Name = player.name,
                        CountryOfBirth = player.countryOfBirth,
                        Nationality = player.nationality,
                        Position = player.position ?? string.Empty,
                        DateOfBirth = player.dateOfBirth == null ? null :
                        DateTime.Parse(player.dateOfBirth.ToString()),
                        Team = team
                    };

                    team.Players.Add(newPlayer);
                }
                return team;
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting team Id: {team.IdService}", e);
            }
        }
    }
}
