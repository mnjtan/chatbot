using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotData.Service.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
        public string Phone { get; set; }

        public User()
        {
        }

        public User(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

    }
}
