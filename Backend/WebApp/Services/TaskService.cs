using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;

        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void AddTask(TaskModel task)
        {
            _taskRepository.AddTaskWithADO(task);
        }
    }
}
