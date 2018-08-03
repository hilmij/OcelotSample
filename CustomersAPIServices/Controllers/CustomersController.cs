using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private static int _count;

        // GET api/customers
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _count++;
            Console.WriteLine($"get...{_count}");
            if (_count <= 2)
            {
                return new[] {"customer1", "customer2"};
            }

            Thread.Sleep(5000);
            _count = 0;
            return new[] { "customer1", "customer2" };
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"customer-{id}";
        }

        // POST api/customers
        [HttpPost]
        public void Post([FromBody] string customer)
        {
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string customer)
        {
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
