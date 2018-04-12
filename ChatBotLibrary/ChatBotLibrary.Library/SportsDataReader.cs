using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        
        //request gameSchedule
        public List<GameViewModel> RequestGameSchedule(string season, string content, string options)
        {
            List<GameViewModel> games = new List<GameViewModel>();

            JObject data = Request(season, content, options).GetAwaiter().GetResult();

            var fullGameSchedule = (JObject)data.Properties().First().Value;

            if(fullGameSchedule.Properties().Count() < 2)
            {
                return games;
            }

            var gameEntryProperty = fullGameSchedule.Properties().ElementAt(1);
            JArray list = (JArray)gameEntryProperty.Value;

            //deserialize JArray to list of c# objects
            var gameList = JsonConvert.DeserializeObject<List<GameViewModel>>(list.ToString());

            

            foreach (var game in gameList)
            {
                games.Add(game);
            }
            return games;
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

            //WebResponse response = request.GetResponse();
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


        public string RequestGameSchedule2(string season, string content, string options)
        {
            JObject data = Request(season, content, options).GetAwaiter().GetResult();

            return data.ToString();
        }



        }
    }
