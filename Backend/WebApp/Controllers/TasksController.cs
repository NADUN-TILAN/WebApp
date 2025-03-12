using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApp.DataAccessLayer;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [RoutePrefix("api/tasks")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TasksController : ApiController
    {
        private readonly TaskService _taskService;
        private readonly TaskRepository taskRepository;
        private readonly TasksGetRepo tasksGetRepo;

        // Dependency Injection Constructor
        public TasksController(TaskService taskService)
        {
            _taskService = taskService;           
        }

        // Default constructor for manual instantiation
        public TasksController()
        {
            _taskService = new TaskService(new TaskRepository(new EmployeeTaskEntities()));
            taskRepository = new TaskRepository(new EmployeeTaskEntities());
            tasksGetRepo = new TasksGetRepo(new EmployeeTaskEntities());
        }

        //AddTasks
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> CreateTask()
        {
            try
            {
                // Ensure the request is multipart/form-data
                if (!Request.Content.IsMimeMultipartContent())
                    return BadRequest("Invalid content type. Expected multipart/form-data.");

                // Define the provider to handle uploaded files
                var uploadPath = HttpContext.Current.Server.MapPath("~/App_Data/uploads");
                if (!System.IO.Directory.Exists(uploadPath))
                    System.IO.Directory.CreateDirectory(uploadPath);

                var provider = new MultipartFormDataStreamProvider(uploadPath);

                // **ASYNC Await File Upload**
                await Request.Content.ReadAsMultipartAsync(provider);

                // Extract form data
                var newTask = new TaskModel
                {
                    Title = provider.FormData["title"],
                    Assignee = provider.FormData["assignee"],
                    Category = provider.FormData["category"],
                    Description = provider.FormData["description"],
                    Assignor = provider.FormData["assignor"],
                    UploadedDocs = 0 
                };

                // Validate and parse DueDate
                if (DateTime.TryParse(provider.FormData["dueDate"], out DateTime dueDate))
                {
                    newTask.DueDate = dueDate;
                }
                else
                {
                    return BadRequest("Invalid date format for DueDate.");
                }

                // **Handle file upload**
                if (provider.FileData.Count > 0)
                {
                    var file = provider.FileData[0];

                    if (file == null || file.Headers?.ContentDisposition == null || string.IsNullOrEmpty(file.LocalFileName))
                    {
                        return BadRequest("Invalid file upload.");
                    }

                    string fileName = file.Headers.ContentDisposition.FileName?.Trim('"') ?? "uploaded_file";
                    fileName = Path.GetFileName(fileName); // Prevent directory traversal attack

                    string filePath = Path.Combine(uploadPath, fileName);

                    // Ensure directory exists
                    string directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Fix: Rename file if it already exists
                    string newFilePath = filePath;
                    int counter = 1;

                    while (File.Exists(newFilePath))
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        string extension = Path.GetExtension(fileName);
                        newFilePath = Path.Combine(uploadPath, $"{fileNameWithoutExtension}_{counter}{extension}");
                        counter++;
                    }

                    try
                    {
                        // Move file to new path
                        File.Move(file.LocalFileName, newFilePath);

                        newTask.UploadedDocs = 1; // File uploaded successfully
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Error moving file: {ex.Message}");
                    }
                }

                // Add task via bussiness layer
                _taskService.AddTask(newTask);

                return Ok(new { message = "Task created successfully." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // get and list all tasks info
        [HttpGet]
        [Route("informations")]
        public IHttpActionResult GetAllTasksinfo()
        {
            try
            {
                var TaskModel = tasksGetRepo.GetAllTasksInfoFromDatabase();
                return Ok(TaskModel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("details/{id}")]
        public IHttpActionResult GetTaskById(int id)
        {
            var task = tasksGetRepo.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }


        //AssignTasksMonitoring

    }
}
