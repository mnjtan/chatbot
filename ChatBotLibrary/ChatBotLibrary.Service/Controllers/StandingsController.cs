using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotLibrary.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotLibrary.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/Standings")]
    public class StandingsController : Controller
    {
        private SportsDataReader sportData = new SportsDataReader();

        [HttpGet] // GET: api/standings ?season="" & teamstats="," & team=""
        //returns standings of all teams seperated by Conference
        public async Task<IEnumerable<ConferenceModel>> GetAsync(string season = "2017-2018-regular", string teamstats = "W,L,WIN %,PTS/G,FG%")
        {
            var options = "teamstats=" + teamstats;

            return await Task.Run(() => sportData.RequestStandings(season, "conference_team_standings", options)); 
        }

        // GET: api/standings/{team}
        [HttpGet("{team}")]
        //returns standing for specified team
        public async Task<TeamStatsModel> Get(string team, string season = "2017-2018-regular", string teamstats = "W,L,WIN %,PTS/G,FG%")
        {
            var options = "teamstats=" + teamstats;
            options += "&team=" + team;
            return await Task.Run(() => sportData.RequestTeamStanding(season, "conference_team_standings", options));
        }
     
    }
}
