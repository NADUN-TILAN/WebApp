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

        [HttpGet]
        [Route("details/{id}/{firstname}/{lastname}")]
        public IHttpActionResult GetUserById(int id, string firstname, string lastname)
        {
            var user = uempRepository.GetUserById(id, firstname, lastname);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IHttpActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (updatedUser == null) return BadRequest("Invalid user data");

            try
            {
                userService.UserUpdateCRUD(id, updatedUser);
                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // Delete user
        [HttpDelete]
        [Route("{id}/{firstname}/{lastname}")]
        public IHttpActionResult DeleteUser(int id, string firstname, string lastname)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                return BadRequest("Invalid user parameters.");
            }

            try
            {
                bool result = uempRepository.DeleteUser(id, firstname, lastname);

                if (!result)
                {
                    return NotFound(); // 404 if user was not found/deleted
                }

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An error occurred while deleting the user.", ex));
            }
        }




        // End Point
    }
}