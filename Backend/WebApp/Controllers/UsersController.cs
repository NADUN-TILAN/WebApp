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

        // insert empployees
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user data");

            userService.AddUser(user);
            return Ok(new { message = "User added successfully" });
        }

        // fetching employees only
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

        // get and list all employees with info
        [HttpGet]
        [Route("informations")]
        public IHttpActionResult GetAllusersinfo()
        {
            try
            {
                var user = uempRepository.GetAllUsersInfoFromDatabase();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Get a user by ID
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetUserById(int id)
        {
            var user = uempRepository.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        
        
        // End Point
    }
}