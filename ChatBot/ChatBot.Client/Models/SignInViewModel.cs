using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Client.Models
{
    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public User User { get; set; }
    }
}
