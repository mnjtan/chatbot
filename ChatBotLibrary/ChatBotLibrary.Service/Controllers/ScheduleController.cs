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
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private SportsDataReader sportData = new SportsDataReader();

        //for all methods, season will default to current season (latest, 2018-playoff)

        [HttpGet("team/{team}")] // GET: api/schedule/team/{team} ?season=""
        //will return the next game for the specified team
        public async Task<GameViewModel> GetAsync(string team, string season = "latest")
        {
            string options = "team=" + team;

            return await Task.Run(() => sportData.RequestNextGame(season, "full_game_schedule",options));
            //return await Task.Run(() => sportData.RequestGameSchedule(season, "full_game_schedule", options));

        }


        [HttpGet("date/{date}")] // GET: api/schedule/date/{date}(YYYYMMDD) ?season=""&teams=""
        //gets all game schedules for specified date
        public async Task<IEnumerable<GameViewModel>> GetAsync(string date, string season = "latest", string teams = "")
        {
            string options = "";
            options += "fordate=" + date + "&";

            if (teams != "")
            {
                options += "team=" + teams;
            }

            return await Task.Run(() => sportData.RequestGameSchedule(season, "daily_game_schedule",options));
        }

    }
}
