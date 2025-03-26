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
        private readonly EmployeeTaskEntities DbContext;

        public TaskRepository(EmployeeTaskEntities dbContext)
        {
            DbContext = dbContext;
        }

        //internal void AddTaskWithADO(Task task)
        //{
        //    throw new NotImplementedException();
        //}

        // Get all tasks from the database
        //public IEnumerable<TaskModel> GetAllTasks()
        //{
        //    var tasks = new List<TaskModel>();

        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("SELECT * FROM Tasks", connection))
        //            {
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var task = new TaskModel
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                            Title = reader.GetString(reader.GetOrdinal("Title")),
        //                            Assignee = reader.GetString(reader.GetOrdinal("Assignee")),
        //                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
        //                            Category = reader.GetString(reader.GetOrdinal("Category")),
        //                            Description = reader.GetString(reader.GetOrdinal("Description")),
        //                            Assignor = reader.GetString(reader.GetOrdinal("Assignor")),
        //                            UploadedDocs = reader.IsDBNull(reader.GetOrdinal("UploadedDocs")) ? 0 : reader.GetInt32(reader.GetOrdinal("UploadedDocs"))
        //                        };
        //                        tasks.Add(task);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error fetching tasks.", ex);
        //    }

        //    return tasks;
        //}

        // Get a specific task by ID from the database
        //public TaskModel GetTaskById(int id)
        //{
        //    TaskModel task = null;

        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id", connection))
        //            {
        //                command.Parameters.AddWithValue("@Id", id);

        //                using (var reader = command.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        task = new TaskModel
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                            Title = reader.GetString(reader.GetOrdinal("Title")),
        //                            Assignee = reader.GetString(reader.GetOrdinal("Assignee")),
        //                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
        //                            Category = reader.GetString(reader.GetOrdinal("Category")),
        //                            Description = reader.GetString(reader.GetOrdinal("Description")),
        //                            Assignor = reader.GetString(reader.GetOrdinal("Assignor")),
        //                            UploadedDocs = reader.IsDBNull(reader.GetOrdinal("UploadedDocs")) ? 0 : reader.GetInt32(reader.GetOrdinal("UploadedDocs"))
        //                        };
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error fetching task by ID.", ex);
        //    }

        //    return task;
        //}

        // Add a new task to the database
        public void AddTaskWithADO(TaskModel task)
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined in the config file.");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                     connection.Open();
                    using (var command = new SqlCommand("WA_Insert_Tasks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@title", task.Title);
                        //command.Parameters.AddWithValue("@assignees", task.Assignee);
                        command.Parameters.AddWithValue("@dueDate", task.DueDate);
                        command.Parameters.AddWithValue("@category", task.Category);
                        command.Parameters.AddWithValue("@description", task.Description);
                        //command.Parameters.AddWithValue("@assignor", task.Assignor);
                        command.Parameters.AddWithValue("@uploadedDocs", task.UploadedDocs);

                         command.ExecuteNonQueryAsync();
                    }
            }
            
        }

        // Update an existing task by ID
        public void UpdateTaskWithADO(int id, TaskModel task)
        {
            try
            {
                using (var connection = new SqlConnection("DefaultConnection"))
                {
                    connection.Open();
                    using (var command = new SqlCommand("WA_Update_Tasks", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@title", task.Title);
                        command.Parameters.AddWithValue("@dueDate", task.DueDate);
                        command.Parameters.AddWithValue("@category", task.Category);
                        command.Parameters.AddWithValue("@description", task.Description);
                        //command.Parameters.AddWithValue("@uploadeddocs", task.UploadedDocs > 0 ? task.UploadedDocs : (object)DBNull.Value);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating task in database.", ex);
            }
        }

        // Delete a task by ID
        //public void DeleteTask(int id)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("WA_Delete_Tasks", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;

        //                command.Parameters.AddWithValue("@id", id);

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error deleting task from database.", ex);
        //    }
        //}
    }
}
