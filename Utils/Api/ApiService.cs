using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Api
{
    public class ApiService : IApiService
    {
        public async Task GetCompetitionByCode(string leagueCode)
        {
            string url = string.Format("https://api.football-data.org/v2/competitions/" + leagueCode + "/teams");

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("X-Auth-Token", "60c18092ddf741a4a2cd25bd9514827f");

            var task = client.GetAsync(url);

            task.Wait();

            var response = task.Result.Content.ReadAsStringAsync();

            response.Wait();

            var result = JsonConvert.DeserializeObject<JToken>(response.Result);

        }
    }
}
