using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebApp.Models;

namespace WebApp.DataAccessLayer
{
    public class TasksGetRepo
    {
        private readonly EmployeeTaskEntities DbContext;

        public TasksGetRepo(EmployeeTaskEntities dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<TaskModel> GetAllTasksInfoFromDatabase()
        {
            var tasks = new List<TaskModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("WA_Select_Tasks_Info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new TaskModel
                            {
                                Title = reader["title"]?.ToString(),
                                //Assignee = reader["assignees"]?.ToString(),
                                DueDate = reader["duedate"] != DBNull.Value ? Convert.ToDateTime(reader["duedate"]) : (DateTime?)null,
                                Category = reader["category"]?.ToString(),
                                Description = reader["description"]?.ToString(),
                                //Assignor = reader["assignor"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching tasks: {ex.Message}");
                throw;
            }

            return tasks;
        }

        // Task List - Details
        public List<TaskModel> GetTaskById(int id)
        {
            var tasks = new List<TaskModel>(); // Renamed from Task to tasks

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("WA_Select_Task_Details", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass the id as a parameter to the stored procedure
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new TaskModel
                                {
                                    // Handling DBNull values and providing a default value 
                                    Title = reader["title"] as string ?? string.Empty,
                                    DueDate = reader["duedate"] as DateTime? ?? DateTime.MinValue,
                                    Category = reader["category"] as string ?? string.Empty,
                                    Description = reader["description"] as string ?? string.Empty,
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                throw; // Optionally rethrow the exception
            }

            return tasks;
        }
    }
}
