using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class TaskRepository 
    {
        private readonly string _connectionString;

        public TaskRepository(EmployeeTaskEntities employeeTaskEntities)
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // Add a new task to the database
        public void AddTaskWithADO(TaskModel task)
        {
            //try
            //{
                using (var connection = new SqlConnection(_connectionString))
                {
                     connection.Open();
                    using (var command = new SqlCommand("WA_Insert_Tasks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@title", task.Title);
                        command.Parameters.AddWithValue("@assignees", task.Assignee);
                        command.Parameters.AddWithValue("@dueDate", task.DueDate);
                        command.Parameters.AddWithValue("@category", task.Category);
                        command.Parameters.AddWithValue("@description", task.Description);
                        command.Parameters.AddWithValue("@assignor", task.Assignor);
                        command.Parameters.AddWithValue("@uploadedDocs", task.UploadedDocs);

                         command.ExecuteNonQueryAsync();
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error inserting task into database.", ex);
            //}
        }

    }
}
