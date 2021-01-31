using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tasktower.UserService.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserTestController : ControllerBase
    {
        private readonly DataAccess.IUnitOfWork _unitOfWork;

        public UserTestController(DataAccess.IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<UserTestController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            IEnumerable<User> user;
            user = await _unitOfWork.UserRepo.GetAll();
            return user;
        }

        // GET api/<UserTestController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            User user;
            if (!Guid.TryParse(id, out Guid uuid))
            {
                throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
            }
            
            var userCache = _unitOfWork.UserCache;

            user = await userCache.Get(uuid.ToString());
            if (user == null) {
                user = await _unitOfWork.UserRepo.GetById(uuid);
                if (user == null)
                {
                    throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
                }
                _ = userCache.SetIfNotExists(uuid.ToString(), user, TimeSpan.FromSeconds(60));
            }

            return user;
        }
        [HttpGet("err")]
        public void ThrowErr()
        {
            throw new ApplicationException("handle this");
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
