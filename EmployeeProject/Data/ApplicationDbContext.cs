using EmployeeProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<EmployeeModel> Employees { get; set; }

    }

}
