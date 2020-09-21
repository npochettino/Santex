using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Api
{
    public interface IApiService
    {
        Task GetCompetitionByCode(string leagueCode);
    }
}
