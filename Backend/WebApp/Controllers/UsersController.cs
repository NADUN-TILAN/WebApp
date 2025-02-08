using System;
using System.Web.Http;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly UserService userService;

        public UsersController()
        {
            userService = new UserService(new UserRepository(new EmployeeTaskEntities()));
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user data");

            userService.AddUser(user);
            return Ok(new { message = "User added successfully" });
        }

        private IHttpActionResult Ok(string v)
        {
            throw new NotImplementedException();
        }
    }
}