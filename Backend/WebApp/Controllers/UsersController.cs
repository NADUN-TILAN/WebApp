using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [System.Web.Http.RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService(new UserRepository(new EmployeeTaskEntities()));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("add")]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user data");

            _userService.AddUser(user);
            return Ok("User added successfully");
        }

        private IHttpActionResult Ok(string v)
        {
            throw new NotImplementedException();
        }
    }
}