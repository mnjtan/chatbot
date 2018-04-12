using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotLibrary.Library;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotLibrary.Service.Controllers
{
    [EnableCors("allowAll")]
    [Produces("application/json")]
    [Route("api/Score")]
    public class ScoreController : Controller
    {
        private SportsDataReader sportData = new SportsDataReader();

        // GET: api/Score/{date} ?season={season} & teams={team1,team2,...}
        [HttpGet("{date}")]
        //gets all game scores for given date
        public async Task<IEnumerable<ScoreViewModel>> GetAsync(string date, string season = "2017-playoff", string teams = "")
        {
            string options = "";
            options += "fordate=" + date + "&";

            if (teams != "")
            {
                options += "team=" + teams;
            }

            return await Task.Run(() => sportData.RequestGameScore(season, "scoreboard", options));

        }

        // GET: api/Score ?season={season} & teams={team1,team2,...}
        [HttpGet]
        //get all game scores for given team during given season
        public async Task<IEnumerable<ScoreViewModel>> GetAsync(string season = "2017-playoff", string teams = "")
        {
            string options = "";

            if (teams != "")
            {
                options += "team=" + teams;
            }

            return await Task.Run(() => sportData.RequestTeamScores(season, "scoreboard", options));

        }

    }
}
