
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
    public class DepartmentsEFRepository : IDepartmentsRepository
    {
        private readonly CompanyDbContext context;

        public DepartmentsEFRepository(CompanyDbContext context)
        {
            this.context = context;
        }

        public void AddDepartment(Department? department)
        {
            if (department == null) return;

            context.Add(department);
            context.SaveChanges();
        }

        public bool DeleteDepartment(Department? department)
        {
            if (department == null) return false;

            var dep = context.Departments?.Find(department.Id);
            if (dep == null) return false;

            context.Departments?.Remove(department);
            context.SaveChanges();
            
            return true;
        }

        public bool DepartmentExists(int departmentId)
        {
            var exists = context.Departments?.Any(x => x.Id == departmentId);

            return exists.HasValue && exists.Value;
        }

        public Department? GetDepartmentById(int id)
        {
            return context.Departments?.Find(id);
        }

        public List<Department> GetDepartments(string? filter = null)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return context.Departments?.ToList();

            return context.Departments?
                .Where(x => EF.Functions.Like(x.Name, $"%{filter}%"))
                .ToList();
        }

        public bool UpdateDepartment(Department? department)
        {
            if (department == null) return false;

            var dep = context.Departments?.Find(department.Id);
            if (dep == null) return false;

            dep.Name = department.Name;
            dep.Description = department.Description;
            dep.Email = department.Email;

            context.SaveChanges();

            return true;
        }
    }
}
