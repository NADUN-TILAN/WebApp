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
        public List<User> GetUserById(int id, string firstname, string lastname)
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


        // Delete 

        public bool DeleteUser(int id, string firstname, string lastname)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                throw new ArgumentException("Invalid input parameters.");
            }

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("WA_Delete_Users", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = id });
                        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 100) { Value = firstname });
                        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 100) { Value = lastname });

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Returns true if a row was deleted
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the error properly (use a logging framework)
                Console.WriteLine($"SQL Error deleting user: {sqlEx.Message}");
                throw new Exception("Database error occurred while deleting user.", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }






    }
}
