using System;
using System.Data.Entity;

namespace WebApp.Models
{
    public partial class EmployeeTaskEntities : DbContext
    {
        public EmployeeTaskEntities()
            : base("metadata=res://*/EmployeeTaskModel.csdl|res://*/EmployeeTaskModel.ssdl|res://*/EmployeeTaskModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=NADUN_PC;initial catalog=WebAppDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;") // Uses the connection string in Web.config
        {
        }

        public virtual DbSet<User> Users { get; set; }  // Map Users table

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
