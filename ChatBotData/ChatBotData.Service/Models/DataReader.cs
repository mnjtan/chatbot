using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotData.Service.Models
{
    public class DataReader
    {
        private ChatBotContext db = new ChatBotContext();

        //returns list of all users in database
        public List<User> ReadUsers()
        {
            var users = db.User.ToList();
            return users;
        }

        //looks up user by email, if not found returns null
        public User FindUser(string email)
        {
            var user = db.User.Where(u => u.Email == email).FirstOrDefault();
            return user;
        }

        //looks up user by email, if not found returns null
        public User FindUserById(int id)
        {
            var user = db.User.Find(id);
            return user;
        }

        public bool InsertUser(User user)
        {
            db.User.Add(user);
            return db.SaveChanges() != 0;
        }

        public bool UpdateUser(User user)
        {
            db.User.Update(user);
            return db.SaveChanges() != 0;
        }


    }
}
