using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tasktower.UserService.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class UserTestController : ControllerBase
    {
        private readonly DataAccess.IUnitOfWorkFactory _unitOfWorkFactory;

        public UserTestController(DataAccess.IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        // GET: api/<UserTestController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            IEnumerable<User> user;
            using (var uow = _unitOfWorkFactory.Create())
            {
                user = await uow.UserRepository.GetAll();
            }
            return user;
        }

        // GET api/<UserTestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserTestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
