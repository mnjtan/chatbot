using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatBotLibrary.Library
{
    public class SportsDataReader
    {
        private string username = "mario62";
        private string password = "NBAchatbot12";
        private string baseURL = "https://api.mysportsfeeds.com/v1.2/pull/nba/";
        //private string baseURL="https://api.mysportsfeeds.com/v1.2/pull/nba/{season-name}/{request}.json?{options}"
        private string format = "json";

        
        //request Game Schedules
        public List<GameViewModel> RequestGameSchedule(string season, string content, string options)
        {
            List<GameViewModel> games = new List<GameViewModel>();

            JObject data = Request(season, content, options).GetAwaiter().GetResult();

            var gameSchedule = (JObject)data.Properties().First().Value;

            //if game schedule is empty, return empty list
            if(gameSchedule.Properties().Count() < 2)
            {
                return games;
            }

            var gameEntryProperty = gameSchedule.Properties().ElementAt(1);
            JArray list = (JArray)gameEntryProperty.Value;

            //deserialize JArray to list of c# objects
            var gameList = JsonConvert.DeserializeObject<List<GameViewModel>>(list.ToString());
                        
            foreach (var game in gameList)
            {
                games.Add(game);
            }
            return games;
        }

        //request game scores for a specific day
        public List<ScoreViewModel> RequestGameScore(string season, string content, string options)
        {
            List<ScoreViewModel> scores = new List<ScoreViewModel>();

            JObject data = Request(season, content, options).GetAwaiter().GetResult();

            var gameScores = (JObject)data.Properties().First().Value;

            //if game schedule is empty, return empty list
            if (gameScores.Properties().Count() < 2)
            {
                return scores;
            }

            var scoreEntry = gameScores.Properties().ElementAt(1);
            JArray list = (JArray)scoreEntry.Value;

            //deserialize JArray to list of c# objects
            var gameScoreList = JsonConvert.DeserializeObject<List<ScoreViewModel>>(list.ToString());

            foreach (var score in gameScoreList)
            {
                scores.Add(score);
            }
            return scores;
        }

        public List<ScoreViewModel> RequestTeamScores(string season,string content, string teams)
        {
            List<ScoreViewModel> teamScoreList = new List<ScoreViewModel>();

            
            //get which days given team played a game, save dates into a list
            var gamesList = RequestGameSchedule(season, "full_game_schedule", teams);
            
            var dateList = new List<string>();

            foreach (var game in gamesList)
            {
                //removing '-' from date string so that it conforms with api's required format YYYYMMDD
                var temp = Regex.Replace(game.Date,"[^0-9]", "");
                dateList.Add(temp);
            }

            //return dateList;

            //use list of dates to get score of games, save each score into list result
            foreach (var date in dateList)
            {
                var scoreList = RequestGameScore(season, "scoreboard", $"fordate={date}&{teams}");
                teamScoreList.AddRange(scoreList);
            }
            return teamScoreList;
        }

        //Make request to MySportsFeed API for season and content type
        private async Task<JObject> Request(string season, string contentType, string options)
        {
            string url = baseURL + season + "/" + contentType + "." + format +"?"+ options; //{pull-url};
            var credentials = Encoding.Default.GetBytes(username + ":" + password);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(credentials));
            request.Method = "GET";

            string content = string.Empty;

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            var data = JObject.Parse(content);
            return data;
        }
        
    }
}
