using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebApp.Models;

namespace WebApp.DataAccessLayer
{
    public class UempRepository
    {
        private readonly EmployeeTaskEntities DbContext;

        public UempRepository(EmployeeTaskEntities dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<AssigneeModel> GetAssigneesFromDatabase()
        {
            var assignee = new List<AssigneeModel>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("WA_Select_Users", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                assignee.Add(new AssigneeModel
                                {                                      
                                    Assignee = (string)reader["Assignee"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignees: {ex.Message}");
                throw;
            }

            return assignee;
        }

        // UsersInfoFromDatabase
        public List<User> GetAllUsersInfoFromDatabase()
        {
            var User = new List<User>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("WA_Select_All_Users_Info", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User.Add(new User
                                {
                                    UserID = Convert.ToInt32(reader["UserID"]),
                                    FirstName = (string)reader["firstName"],
                                    MiddleName = (string)reader["middleName"],
                                    LastName = (string)reader["lastName"],
                                    Email = (string)reader["email"],
                                    ContactNo = (string)reader["contactNo"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignees: {ex.Message}");
                throw;
            }

            return User;
        }

        // User List - Details
        public List<User> GetUserById(int id)
        {
            var users = new List<User>(); // Renamed from User to users

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("WA_Select_User_Details", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Pass the id as a parameter to the stored procedure
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    // Handling DBNull values and providing a default value 
                                    FirstName = reader["firstName"] as string ?? string.Empty,
                                    MiddleName = reader["middleName"] as string ?? string.Empty,
                                    LastName = reader["lastName"] as string ?? string.Empty,
                                    Email = reader["email"] as string ?? string.Empty,
                                    ContactNo = reader["contactNo"] as string ?? string.Empty
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

            return users;
        }


    }
}
