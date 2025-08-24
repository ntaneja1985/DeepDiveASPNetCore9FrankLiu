using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Department>? Departments { get; set; }
        public DbSet<Employee>? Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Department>().HasData(
                new Department(1, "Sales", "Sales Department"),
                new Department(2, "Engineering", "Engineering Department"),
                new Department(3, "QA", "Quanlity Assurance")
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee(1, "John Doe", "Engineer", 60000, 1),
                new Employee(2, "Jane Smith", "Manager", 75000, 1),
                new Employee(3, "Sam Brown", "Technician", 50000, 1),
                new Employee(4, "Alice Johnson", "Analyst", 55000, 2),
                new Employee(5, "Bob Lee", "Developer", 65000, 2),
                new Employee(6, "Carol Wang", "Designer", 70000, 2),
                new Employee(7, "David Kim", "Support", 48000, 3),
                new Employee(8, "Eve Rogers", "Consultant", 72000, 3),
                new Employee(9, "Franklin Zhang", "Architect", 80000, 3),
                new Employee(10, "Grace Liu", "Coordinator", 53000, 1),
                new Employee(11, "Henry Thompson", "Specialist", 62000, 2),
                new Employee(12, "Isabelle Nguyen", "Technician", 57000, 3)
            );
        }
    }
}
