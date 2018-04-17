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

        [HttpGet] // GET: api/Standings ?season="" & teamstats="," & team=""
        //returns standings of all teams seperated by Conference
        public async Task<IEnumerable<ConferenceModel>> GetAsync(string season = "2017-2018-regular", string teamstats = "W,L")
        {
            var options = "";

            //for test purposes
            options += "teamstats=" + teamstats;

            return await Task.Run(() => sportData.RequestStandings(season, "conference_team_standings", options)); 
        }

        // GET: api/Standings/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
     
    }
}
