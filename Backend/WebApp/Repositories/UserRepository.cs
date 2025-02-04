using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class UserRepository
    {
         private readonly EmployeeTaskEntities _context;

        public UserRepository(EmployeeTaskEntities context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Database.ExecuteSqlCommand("EXEC WA_Insert_Users @firstName, @middleName, @lastName, @department, @dOB, @address, @country, @contactNo, @email",
                new SqlParameter("@firstName", user.FirstName),
                new SqlParameter("@middleName", user.MiddleName),
                new SqlParameter("@lastName", user.LastName),
                new SqlParameter("@department", user.Department),
                new SqlParameter("@dOB", user.DOB),
                new SqlParameter("@address", user.Address),
                new SqlParameter("@country", user.Country),
                new SqlParameter("@contactNo", user.ContactNo),
                new SqlParameter("@email", user.Email));
        }
    }
}