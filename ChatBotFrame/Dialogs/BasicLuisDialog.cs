using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        private const string _baseUri = "http://13.59.35.94/chatbotlibrary/api/";
        
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I don't understand what you said. Please try again.");
            context.Wait(MessageReceived);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Greeting my friend.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I don't understand what you said. Please try again.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I can help you find out the schedule and score of any games and the ranking of a team.");
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("Score.Two.Team")]
        private async Task ShowScoreTwoTeamResult(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count < 2)
            {
                await context.PostAsync("No team is found");
                context.Wait(MessageReceived);
            }
            else
            {
                string response = "";
                Score score = new Score();
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string RequestURI = _baseUri+"score/team/";
                        string team1 = "";
                        string team2 = "";
                        var va = result.Entities.FirstOrDefault().Resolution;
                        var t = JsonConvert.SerializeObject(va);
                        var _result = JsonConvert.DeserializeObject<TeamNameValue>(t);
                        team1 = _result.values.FirstOrDefault();
                        var va2 = result.Entities.LastOrDefault().Resolution;
                        var t2 = JsonConvert.SerializeObject(va2);
                        var _result2 = JsonConvert.DeserializeObject<TeamNameValue>(t2);
                        team2 = _result2.values.FirstOrDefault();
                        RequestURI += team1 +"/"+team2;
                     
                        HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                        if(responseMsg.IsSuccessStatusCode)
                        {
                            var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                            
                            score = JsonConvert.DeserializeObject<Score>(apiResponse);
                            if(score.game == null)
                            {
                                await context.PostAsync("The last game for "+ team1 +" vs " + team2 + " is not found.");
                                context.Wait(MessageReceived);
                            }
                            else
                            {
                                response += score;
                                await context.PostAsync(response);
                                context.Wait(MessageReceived);
                            }
                        }   
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync("Error. Try again.");
                    context.Wait(MessageReceived);
                }
            }
        }
        
        [LuisIntent("Score.Team")]
        private async Task ShowScoreTeamResult(IDialogContext context, LuisResult result)
        {
            //check user input entity
            if(result.Entities.FirstOrDefault() == null)
            {
                await context.PostAsync("No team is found");
                context.Wait(MessageReceived);
            }
            else
            {
                string response = "";
                Score score = new Score();
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string RequestURI = _baseUri+"score/team/";
                        string team = "";
                        var va = result.Entities.FirstOrDefault().Resolution;
                        var t = JsonConvert.SerializeObject(va);
                        var _result = JsonConvert.DeserializeObject<TeamNameValue>(t);
                        team = _result.values.FirstOrDefault();
                        RequestURI += team;
                        HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                        if(responseMsg.IsSuccessStatusCode)
                        {
                            var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                            
                            score = JsonConvert.DeserializeObject<Score>(apiResponse);
                            if(score.game == null)
                            {
                                await context.PostAsync("The last game for "+ team + " is not found.");
                                context.Wait(MessageReceived);
                            }
                            else
                            {
                                response += score;
                                await context.PostAsync(response);
                                context.Wait(MessageReceived);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync("Error. Try again.");
                    context.Wait(MessageReceived);
                }
            }
        }
        
        [LuisIntent("Score.Time")]
        private async Task SchowScoreDateResult(IDialogContext context, LuisResult result)
        {
            string response ="";
            List<Score> scores = new List<Score>();
            
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    string RequestURI = _baseUri+"score/date/";
                    string date = "";
                    string date2 ="";
                    var va = result.Entities.FirstOrDefault().Resolution;
                    var t = JsonConvert.SerializeObject(va);
                    var _result = JsonConvert.DeserializeObject<TimeValue>(t);
                    date = _result.values.FirstOrDefault().value;
                    date2 = _result.values.FirstOrDefault().value;
                    date = date.Replace("-","");
                    RequestURI += date;
                    HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                    
                    if(responseMsg.IsSuccessStatusCode)
                    {
                        var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                        scores = JsonConvert.DeserializeObject<List<Score>>(apiResponse);
                        foreach(Score s in scores)
                        {
                            response += s+"\n\n";
                        }  
                        if(response == "")
                        {
                            await context.PostAsync("No game is found on " + date2 + ".");
                            context.Wait(MessageReceived);
                        }
                        else
                        {
                            await context.PostAsync(response);
                            context.Wait(MessageReceived);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await context.PostAsync("Error. Try again.");
                context.Wait(MessageReceived); 
            }
        }
        
        [LuisIntent("Score.Date.Team")]
        private async Task ShowScoreDateTeamResult(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count != 2)
            {
                await context.PostAsync("No team or date is found");
                context.Wait(MessageReceived);
            }
            else
            {
                string response ="";
                List<Score> scores = new List<Score>();
                try
                {
                    using(HttpClient client = new HttpClient())
                    {
                        string RequestURI = _baseUri+"/score/date/";
                        var va = result.Entities.FirstOrDefault().Resolution;
                        var t = JsonConvert.SerializeObject(va);
                        var _result = JsonConvert.DeserializeObject<TimeValue>(t);
                        string date = _result.values.FirstOrDefault().value;
                        string date2 = _result.values.FirstOrDefault().value;
                        date = date.Replace("-","");
                        RequestURI += date;
                        
                        var va2 = result.Entities.LastOrDefault().Resolution;
                        string team = "";
                        var t2 = JsonConvert.SerializeObject(va2);
                        var _result2 = JsonConvert.DeserializeObject<TeamNameValue>(t2);
                        team = _result2.values.FirstOrDefault();
                        RequestURI += "&team="+ team;
                        
                        HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                        if(responseMsg.IsSuccessStatusCode)
                        {
                            var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                            scores = JsonConvert.DeserializeObject<List<Score>>(apiResponse);
                                
                            foreach(Score s in scores)
                            {
                                response += s +"\n\n";
                            }
                            
                            if(response == "")
                            {
                                response += "The team " + team + " did not play on " + date2;
                                
                            }
                            await context.PostAsync(response);
                            context.Wait(MessageReceived);
                        }
                    }
                }
                catch
                {
                    await context.PostAsync("Error. Try again.");
                    context.Wait(MessageReceived);
                }
            }
        }

        [LuisIntent("Schedule.Time")]
        private async Task ShowScheduleDateResult(IDialogContext context, LuisResult result)
        {
            string response = "";
            List<Game> games = new List<Game>();
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string RequestURI = _baseUri+"schedule/date/";
                    string date = "";
                    string date2 = "";
                    var va = result.Entities.FirstOrDefault().Resolution;
                    var t = JsonConvert.SerializeObject(va);
                    var _result = JsonConvert.DeserializeObject<TimeValue>(t);
                    date = _result.values.LastOrDefault().value;
                    date2 = _result.values.LastOrDefault().value;
                    date = date.Replace("-","");
                    RequestURI += date;
                    HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);

                    if(responseMsg.IsSuccessStatusCode)
                    {

                        var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                        games = JsonConvert.DeserializeObject<List<Game>>(apiResponse);

                        foreach(Game g in games)
                        {
                            response += g+"\n\n";
                        }
                        if(response == "")
                        {
                            await context.PostAsync("No game is found on " + date2 + ".");
                            context.Wait(MessageReceived);
                        }
                        else
                        {
                            await context.PostAsync(response);
                            context.Wait(MessageReceived);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync("Error. Try again.");
                context.Wait(MessageReceived);
            }
        }
        
        [LuisIntent("Schedule.Team")]
        private async Task ShowScheduleTeamResult(IDialogContext context, LuisResult result)
        {
            if(result.Entities.FirstOrDefault() == null)
            {
                await context.PostAsync("No team is found");
                context.Wait(MessageReceived);
            }
            else
            {
                string response = "";
                Game game = new Game();
                
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string RequestURI = _baseUri+"schedule/team/";
                        var va = result.Entities.FirstOrDefault().Resolution;
                        string team = "";
                        var t = JsonConvert.SerializeObject(va);
                        var _result = JsonConvert.DeserializeObject<TeamNameValue>(t);
                        team = _result.values.FirstOrDefault();
                        RequestURI += team;

                        HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                        if(responseMsg.IsSuccessStatusCode)
                        {
                            var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                            game = JsonConvert.DeserializeObject<Game>(apiResponse);
                            if(game != null)
                            {
                                response += game;
                            }
                            else
                            {
                                response += "The next game for "+ team + " is not found.";
                            }
                            await context.PostAsync(response);
                            context.Wait(MessageReceived);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync("Error. Try again.");
                    context.Wait(MessageReceived);
                }
            }
        }
        
        [LuisIntent("Schedule.Date.Team")]
        private async Task ShowScheduleDateTeamResult(IDialogContext context, LuisResult result)
        {
            if(result.Entities.Count != 2)
            {
                await context.PostAsync("No team or date is found");
                context.Wait(MessageReceived);
            }
            else
            {
                string response ="";
                List<Game> games = new List<Game>();
                try
                {
                    using(HttpClient client = new HttpClient())
                    {
                        string RequestURI = _baseUri+"schedule/date/";
                        var va = result.Entities.FirstOrDefault().Resolution;
                        var t = JsonConvert.SerializeObject(va);
                        var _result = JsonConvert.DeserializeObject<TimeValue>(t);
                        string date = _result.values.LastOrDefault().value;
                        string date2 = _result.values.LastOrDefault().value;
                        date = date.Replace("-","");
                        RequestURI += date;
                        
                        var va2 = result.Entities.LastOrDefault().Resolution;
                        string team = "";
                        var t2 = JsonConvert.SerializeObject(va2);
                        var _result2 = JsonConvert.DeserializeObject<TeamNameValue>(t2);
                        team = _result2.values.FirstOrDefault();
                        RequestURI += "&team="+ team;
                        
                        HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                        if(responseMsg.IsSuccessStatusCode)
                        {
                            var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                            games = JsonConvert.DeserializeObject<List<Game>>(apiResponse);
                                
                            foreach(Game g in games)
                            {
                                response += g +"\n\n";
                            }
                            
                            if(response =="")
                            {
                                response += "The team " + team + " is not playing on " + date2;
                                
                            }
                            await context.PostAsync(response);
                            context.Wait(MessageReceived);
                        }
                    }
                }
                catch(Exception ex)
                {
                    await context.PostAsync("Error. Try again.");
                    context.Wait(MessageReceived);
                }
            }
        }
        
        [LuisIntent("Rank")]
        private async Task ShowRankResult(IDialogContext context, LuisResult result)
        {
            string response = "";
            
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    string RequestURI = _baseUri + "standings/";
                    HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                    if(responseMsg.IsSuccessStatusCode)
                    {
                        var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                        List<Conference> conferences = JsonConvert.DeserializeObject<List<Conference>>(apiResponse);
                        foreach(var c in conferences)
                        {
                            response += c.ToString() + "\n";
                        }
                    }
                }
                await context.PostAsync(response);
                context.Wait(MessageReceived);
            }
            catch(Exception ex)
            {
                await context.PostAsync("Error");
                context.Wait(MessageReceived);
            }
        }
        
        [LuisIntent("Rank.Team")]
        private async Task ShowRankTeamResult(IDialogContext context, LuisResult result)
        {
            string response = "";
            try
            {
                var va = result.Entities.FirstOrDefault().Resolution;
                var t = JsonConvert.SerializeObject(va);
                var _result = JsonConvert.DeserializeObject<TeamNameValue>(t);
                string team = _result.values.FirstOrDefault();
                string RequestURI = _baseUri + "standings/"+team;
                
                using(HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMsg = await client.GetAsync(RequestURI);
                    if(responseMsg.IsSuccessStatusCode)
                    {
                        var apiResponse = await responseMsg.Content.ReadAsStringAsync();
                        TeamStats teamStats = JsonConvert.DeserializeObject<TeamStats>(apiResponse);
                        response += $"Team: {teamStats.Team.Name}, Rank: {teamStats.Rank}" +
                                    $", Game Played: {teamStats.Stats.GamesPlayed.Text} \nWins: {teamStats.Stats.Wins.Text}" +
                                    $", Losses: {teamStats.Stats.Losses.Text}, Win%: {teamStats.Stats.WinPct.Text}" +
                                    $"\nFG%: {teamStats.Stats.FgPct.Text}, PPG: {teamStats.Stats.PtsPerGame.Text}";
                        
                        await context.PostAsync(response);
                        context.Wait(MessageReceived);
                    }
                }
            }
            catch(Exception ex)
            {
                await context.PostAsync("Error");
                context.Wait(MessageReceived);
            }
        }
    }
    
    //team entity value
    public class TeamNameValue
    {
        public List<string> values {set; get;}
    }
    
    //time entity value
    public class TimeValue
    {
        public List<Value> values {set; get;}   
    }
    //value for time entity value
    public class Value
    {
        public string timex {set; get;}
        public string type {set; get;}
        public string value {set; get;}
    }
    //game model
	public class Game
	{
		public string Date {set; get;}
		public string Time {set; get;}
		public string Location {set; get;}
		public Team AwayTeam {set; get;}
		public Team HomeTeam {set; get;}
		
		public override string ToString()
		{
			return $"Date: {Date}, Time: {Time}" +
                $"\n\t Away: {AwayTeam} \n\t Home: {HomeTeam}";
		}
	}
    
    //score model
	public class Score
    {
        public Game game { get; set; }
        public int AwayScore { get; set; }
        public int HomeScore { get; set; }
        public string IsUnplayed { get; set; }
        public string IsInProgress { get; set; }
        public string IsCompleted { get; set; }
        //public QuarterSummary QuarterSummary { get; set; }
        
        public override string ToString()
        {
            return $"Date: {game.Date}, Time: {game.Time}" +
                $"\n\t Away: {game.AwayTeam} \t {AwayScore} \n\t Home: {game.HomeTeam} \t {HomeScore}";
        }
    }
    
    //team model
    public class Team
    {
	    public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        
        public override string ToString()
        {
            return $"{City} {Name}";
        }
    }
    
    //stats
    public class Stats
    {
        public Details GamesPlayed {set; get;}
        public Details Wins {set; get;}
        public Details Losses {set; get;}
        public Details WinPct {set; get;}
        public Details FgPct {set; get;}
        public Details PtsPerGame {set; get;}
        
        public override string ToString()
        {
            return $"\nGames Played: {GamesPlayed}, Wins: {Wins}, Losses: {Losses}, Win%: {WinPct} \nFG%: {FgPct}, PPG: {PtsPerGame}";
        }
    }
    
    public class Details
    {
        public string Abbreviation {set; get;}
        public string Text {set; get;}
        
        public override string ToString()
        {
            return Text;
        }
    }
    
    public class TeamStats
    {
        public Team Team {set; get;}
        public int Rank {set; get;}
        public Stats Stats {set; get;}
        
        public override string ToString()
        {
            return $"{Rank}" + "\t" + $"{Team.Name}";
        }
    }
    
    public class Conference
    {
        public string Name {set; get;}
        public List<TeamStats> TeamEntry {set; get;}
        
        public override string ToString()
        {
            return $"{Name}" + " Conference" +
            $"\n {PrintEntries()}";
        }
        
        public string PrintEntries()
        {
            string str = "";
            foreach(var team in TeamEntry)
            {
                str += $"{team.ToString()}\n";
            }
            return str;
        }
    }
    
}