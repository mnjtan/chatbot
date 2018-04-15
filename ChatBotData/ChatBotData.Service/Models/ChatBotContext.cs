using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBotData.Service.Models
{
    public class ChatBotContext : DbContext
    {
        private  IConfiguration Configuration { get; set; }
        private string Connection;

        public DbSet<User> User { get; set; }

        public ChatBotContext()
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsetting.dev.json").Build();
            Connection = Configuration.GetSection("ConnectionString").Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Connection);
        }
    }
}
