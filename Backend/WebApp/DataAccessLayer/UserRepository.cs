using System;
using System.Data.SqlClient;
using WebApp.Models;
using System.Configuration;

namespace WebApp.Repositories
{
    public class UserRepository
    {
        private readonly EmployeeTaskEntities DbContext;

        public UserRepository(EmployeeTaskEntities dbContext)
        {
            DbContext = dbContext;
        }

        public void AddUserWithADO(User user)
        {
            // Get the connection string from the configuration file
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined in the config file.");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("WA_Insert_Users", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@middleName", user.MiddleName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@department", user.Department);
                    command.Parameters.AddWithValue("@dOB", user.DOB);
                    command.Parameters.AddWithValue("@address", user.Address);
                    command.Parameters.AddWithValue("@country", user.Country);
                    command.Parameters.AddWithValue("@contactNo", user.ContactNo);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
