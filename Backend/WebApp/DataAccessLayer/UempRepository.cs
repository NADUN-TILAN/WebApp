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
                    using (SqlCommand cmd = new SqlCommand("WA_Select_Users_Info", conn))
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
    }
}
