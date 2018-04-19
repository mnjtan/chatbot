using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatBot.Client.Models
{
    public class DataReader
    {
        public static async Task<User> SignIn(string email)
        {
            var client = new HttpClient();
            var url = "http://13.59.35.94/chatbotdata/api/data/" + email;
            var result = await client.GetAsync(new Uri(url));

            if(result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(content);
                return user;
            }

            return null;
        }
    }
}
