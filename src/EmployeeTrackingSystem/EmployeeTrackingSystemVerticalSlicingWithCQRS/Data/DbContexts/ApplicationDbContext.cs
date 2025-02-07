using EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.Data.DbContexts
{
    public partial class ApplicationDbContext : DbContext
    {

        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }




    }
}
