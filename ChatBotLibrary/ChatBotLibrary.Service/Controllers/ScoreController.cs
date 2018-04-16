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

        [HttpGet("date/{date}")]  // GET: api/score/date/{date} ?season={season} & teams={team1,team2,...}
        //gets all game scores for given date (YYYYMMDD)
        public async Task<IEnumerable<ScoreViewModel>> GetAsync(string date, string season = "latest", string teams = "")
        {
            string options = "";
            options += "fordate=" + date + "&";

            if (teams != "")
            {
                options += "team=" + teams;
            }

            return await Task.Run(() => sportData.RequestGameScore(season, "scoreboard", options));
        }

        [HttpGet("team/{team}")] // GET: api/score/team/{team} ?season={season}
        //get all game scores for given team during given season
        public async Task<ScoreViewModel> GetAsync(string team, string season = "latest")
        {
            string options = "team=" + team;

            return await Task.Run(() => sportData.RequestLastGameScore(season, "scoreboard", options));
        }

        [HttpGet("team/{team1}/{team2}")] // GET: api/score/team/{team} ?season={season}
        //get game score for last game between team1 and team2
        public async Task<ScoreViewModel> GetScoreAsync(string team1, string team2, string season = "latest")
        {
            string options = "team=" + team1;


            return await Task.Run(() => sportData.RequestScore(season, "scoreboard", team1, team2));
        }


    }
}
