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
        public  IConfiguration Configuration { get; set; }
        //private string Connection;

        public DbSet<User> User { get; set; }

        public ChatBotContext()
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.dev.json").Build();

            //Connection = Configuration.GetSection("ConnectionString").Value;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //builder.UseSqlServer(Connection);
            builder.UseSqlServer(Configuration.GetSection("ConnectionString").Value);

        }

    }
}
