
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
    public class EmployeesEFRepository : IEmployeesRepository
    {
        private readonly CompanyDbContext context;

        public EmployeesEFRepository(CompanyDbContext context)
        {
            this.context = context;
        }

        public void AddEmployee(Employee? employee)
        {
            if (employee == null) return;

            context.Employees?.Add(employee);
            context.SaveChanges();
        }

        public bool DeleteEmployee(Employee? employee)
        {
            if (employee == null) return false;

            var emp = context.Employees?.Find(employee.Id);
            if (emp == null) return false;

            context.Employees?.Remove(employee);
            context.SaveChanges();

            return true;
        }

        public bool EmployeeExists(int employeeId)
        {
            var exists = context.Employees?.Any(x => x.Id == employeeId);

            return exists.HasValue && exists.Value;
        }

        public Employee? GetEmployeeById(int id)
        {
            return context.Employees?.Include(x => x.Department).First(x => x.Id == id);
        }

        public List<Employee> GetEmployees(string? filter = null, int? departmentId = null)
        {            
            if (departmentId.HasValue)
            {
                return context.Employees?.Where(x => x.DepartmentId == departmentId.Value).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(filter))
            {
                return context.Employees?
                    .Where(x => EF.Functions.Like(x.Name, $"%{filter}%"))
                    .ToList();
            }

            return context.Employees?.ToList();
        }

        public bool UpdateEmployee(Employee? employee)
        {
            if (employee == null) return false;

            var emp = context.Employees?.Find(employee.Id);
            if (emp == null) return false;

            emp.Name = employee.Name;
            emp.Position = employee.Position;
            emp.Salary = employee.Salary;
            emp.DepartmentId = employee.DepartmentId;

            context.SaveChanges();

            return true;
        }
    }
}
