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

        //returns the next game for a specified team
        public GameModel RequestNextGameSchedule(string season, string content, string team)
        {
            //get all games for specified team
            var gamesList = RequestGameSchedule(season, content, team);

            //for test puproses
            var today = DateTime.Today;
            today = DateTime.Parse("2017-04-21");

            //only get the teams next game
            foreach(var game in gamesList)
            {
                var gameDate = DateTime.Parse(game.Date);
                if(gameDate >= DateTime.Today)
                {
                    return game;
                }
            }
            return null;
        }

        //returns score for last game played by the specified team
        public ScoreModel RequestLastGameScore(string season, string content, string team)
        {
            //get all games for specified team
            var gamesList = RequestGameSchedule(season, "full_game_schedule", team);

            //for test purposes
            var today = DateTime.Today;
            today = DateTime.Parse("2017-04-21");

            //reverse list to iterate from latest to oldest date
            gamesList.Reverse();

            //get the teams last played game
            foreach (var game in gamesList)
            {
                var gameDate = DateTime.Parse(game.Date);
                if (gameDate < DateTime.Today)
                {
                    //removing '-' from date string so that it conforms with api's required format YYYYMMDD
                    var date = Regex.Replace(game.Date, "[^0-9]", "");

                    string options = "fordate=" + date + "&";

                    var scores = RequestGameScore(season, content, options+team);
                    return scores.FirstOrDefault();
                }
            }
            return null;
        }

        //returns score for last game between team1 and team2
        public ScoreModel RequestScore(string season, string content, string team1, string team2)
        {
            //get list of games played by team1
            var gamesList = RequestGameSchedule(season, "full_game_schedule", $"team={team1}");

            //reverse list to iterate from latest to oldest date
            gamesList.Reverse();

            //find last game where team1 played against team2
            foreach(var game in gamesList)
            {
                var homeTeam = game.HomeTeam.Abbreviation;
                var awayTeam = game.AwayTeam.Abbreviation;
                var gameDate = DateTime.Parse(game.Date);

                //if played against team2 and game has passed already
                if ((homeTeam == team2.ToUpper() || awayTeam == team2.ToUpper()) && gameDate < DateTime.Today)
                {
                    //use date of game to find the score
                    //removing '-' from date string so that it conforms with api's required format YYYYMMDD
                    var date = Regex.Replace(game.Date, "[^0-9]", "");

                    string options = $"fordate={date}&team={team1}";

                    var scores = RequestGameScore(season, content, options);
                    return scores.FirstOrDefault();
                }
            }
            return null;
        }

        //request Game Schedules (options for team or date)
        public List<GameModel> RequestGameSchedule(string season, string content, string options)
        {
            List<GameModel> games = new List<GameModel>();

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
            var gameList = JsonConvert.DeserializeObject<List<GameModel>>(list.ToString());
                        
            foreach (var game in gameList)
            {
                games.Add(game);
            }
            return games;
        }

        //request game scores for a specific day
        public List<ScoreModel> RequestGameScore(string season, string content, string options)
        {
            List<ScoreModel> scores = new List<ScoreModel>();

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
            var gameScoreList = JsonConvert.DeserializeObject<List<ScoreModel>>(list.ToString());

            foreach (var score in gameScoreList)
            {
                scores.Add(score);
            }
            return scores;
        }

        //requests all game scores for a specified team
        public List<ScoreModel> RequestTeamScores(string season,string content, string teams)
        {
            List<ScoreModel> teamScoreList = new List<ScoreModel>();

            
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

        //request team standings seperated by division (options for team or date)
        public List<ConferenceModel> RequestStandings(string season, string content, string options)
        {
            List<ConferenceModel> conferenceStandings = new List<ConferenceModel>();

            JObject data = Request(season, content, options).GetAwaiter().GetResult();

            var standings = (JObject)data.Properties().First().Value;

            //if game schedule is empty, return empty list
            if (standings.Properties().Count() < 2)
            {
                return conferenceStandings;
            }

            var conferenceProperty = standings.Properties().ElementAt(1);
            JArray list = (JArray)conferenceProperty.Value;
            var str = list.ToString();
            str = Regex.Replace(str, "[#@]", "");

            //deserialize JArray to list of c# objects
            var conferenceList = JsonConvert.DeserializeObject<List<ConferenceModel>>(str);

            foreach (var conference in conferenceList)
            {
                conferenceStandings.Add(conference);
            }
            return conferenceStandings;
        }
        
        //Make request to MySportsFeed API for season and content type
        private async Task<JObject> Request(string season, string contentType, string options)
        {
            string url = baseURL + season + "/" + contentType + "." + format +"?"+ options;
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
