using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotData.Service.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotData.Service.Controllers
{
    [EnableCors("allowAll")]
    [Produces("application/json")]
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private DataReader dataReader { get; set; } = new DataReader();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/values/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
