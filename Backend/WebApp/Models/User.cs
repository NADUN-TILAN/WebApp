using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string ContactNo { get; set; }     
        public string Email { get; set; }
    }
}