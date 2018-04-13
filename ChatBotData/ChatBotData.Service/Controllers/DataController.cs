using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotData.Service.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotData.Service.Controllers
{
    [EnableCors("allowAll")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private DataReader dataReader { get; set; } = new DataReader();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/data
        //[HttpGet]
        //public async Task<IEnumerable<User>> GetAllUsers()
        //{
        //    return await Task.Run(() => dataReader.ReadUsers());
        //}



        //// GET api/data/{email}
        //[HttpGet("{email}", Name = "GetUser")]
        //public async Task<User> GetUser(string email)
        //{
        //    return await Task.Run(() => dataReader.FindUser(email));
        //}

        // POST api/data
        [HttpPost] //Create
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            if (user == null)
            {
                return await Task.Run(() => BadRequest());
            }

            dataReader.InsertUser(user);

            return CreatedAtRoute("GetUser", new { email = user.Email }, user);
        }

        // PUT api/data/5
        [HttpPut("{id}")] //Update
        public async Task<IActionResult> UpdateUser(int id, [FromBody]User user)
        {
            if (user == null || user.UserId != id)
            {
                return await Task.Run(() => BadRequest());
            }
            var updatedUser = dataReader.FindUserById(id);
            if (updatedUser == null)
            {
                return await Task.Run(() => NotFound());
            }
            updatedUser.Name = user.Name;
            updatedUser.Phone = user.Phone;
            updatedUser.Team = user.Team;

            dataReader.UpdateUser(updatedUser);

            return await Task.Run(() => new NoContentResult());
        }

        // GET api/data/{id}
        //[HttpGet("{id}", Name = "GetUser")]
        //public async Task<User> GetUser(int id)
        //{
        //    return await Task.Run(() => dataReader.FindUserById(id));
        //}
    }
}
