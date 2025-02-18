using System;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.DataAccessLayer;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [RoutePrefix("api/users")]
    [EnableCors(origins: "*", headers: "*", methods: "*")] // Apply CORS to this controller
    public class UsersController : ApiController
    {
        private readonly UserService userService;
        private readonly UempRepository uempRepository;

        public UsersController()
        {
            userService = new UserService(new UserRepository(new EmployeeTaskEntities()));
            uempRepository = new UempRepository(new EmployeeTaskEntities());
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

        [HttpGet]
        [Route("assignees")]
        public IHttpActionResult GetAssignees()
        {
            try
            {
                var assignee = uempRepository.GetAssigneesFromDatabase(); 
                return Ok(assignee);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}