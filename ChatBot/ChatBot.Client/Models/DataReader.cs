using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Client.Models
{
    public class DataReader
    {
        public static async Task<User> SignIn(string email)
        {
            if(email == null || email == string.Empty)
            {
                return null;
            }
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

        public static async Task<User> RegisterUser(User user)
        {
            var check = await SignIn(user.Email);
            //if user with same email already exists, dont register and return null
            if(user == null || check != null)
            {
                return null;
            }

            var client = new HttpClient();
            var url = "http://13.59.35.94/chatbotdata/api/data/";
            var data = JsonConvert.SerializeObject(user, Formatting.Indented);
            
            var result = await client.PostAsync(new Uri(url),new StringContent(data,Encoding.UTF8,"application/json"));

            if (result.IsSuccessStatusCode)
            {
                return user;
            }

            return new User() { Name = result.ReasonPhrase};
        }
    }
}
