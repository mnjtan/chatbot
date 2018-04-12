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
    [Route("api/Schedule")]
    public class ScheduleController : Controller
    {
        private SportsDataReader sportData = new SportsDataReader();
        
        // GET: api/Schedule ?season=""team=""
        [HttpGet]
        //defaults to current season (2017-playoff for now)
        public async Task<IEnumerable<GameViewModel>> GetAsync(string season = "2017-playoff", string teams = "")
        {
            string options = "";

            if(teams != "")
            {
                options += "team=" + teams;
            }

            return await Task.Run(() => sportData.RequestGameSchedule(season, "full_game_schedule",options));
        }
                
        // GET: api/Schedule/date(YYYYMMDD) ?season=""
        [HttpGet("{date}")]
        //gets daily_game_schedule by season
        public async Task<IEnumerable<GameViewModel>> GetAsync(string date, string season = "2017-playoff", string teams = "")
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
