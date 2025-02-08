using System;
using System.Data.SqlClient;
using System.Linq;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class UserRepository
    {
        private readonly EmployeeTaskEntities DbContext;

        public UserRepository(EmployeeTaskEntities dbContext)
        {
            DbContext = dbContext;
        }

        // Using EF6 to add a new user
        public void AddUserWithEF(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        //internal void AddUser(User user)
        //{
        //    try
        //    {
        //        // Add the user to the DbSet
        //        DbContext.Users.Add(user);

        //        // Save changes to the database
        //        DbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors (e.g., log the error or throw a custom exception)
        //        Console.WriteLine("Error: " + ex.Message);
        //        throw;
        //    }
        //}

        // Using ADO.NET to execute a stored procedure
        public void AddUserWithADO(User user)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@firstName", user.FirstName),
                new SqlParameter("@middleName", user.MiddleName),
                new SqlParameter("@lastName", user.LastName),
                new SqlParameter("@department", user.Department),
                new SqlParameter("@dOB", user.DOB),
                new SqlParameter("@address", user.Address),
                new SqlParameter("@country", user.Country),
                new SqlParameter("@contactNo", user.ContactNo),
                new SqlParameter("@email", user.Email)
            };

            using (var connection = new SqlConnection("Data Source=NADUN_PC;Initial Catalog=WebAppDB;Integrated Security=True"))
            {
                connection.Open();
                var command = new SqlCommand("WA_Insert_Users", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }

        // Fetching users using EF6
        //public User GetUser(int id)
        //{
        //    return _dbContext.Users.SingleOrDefault(u => u.UserID == id);
        //}
    }
}