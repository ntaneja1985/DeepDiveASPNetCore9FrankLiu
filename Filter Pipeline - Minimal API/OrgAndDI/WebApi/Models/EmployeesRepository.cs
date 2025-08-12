namespace WebApi.Models
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private List<Employee> employees = new List<Employee>
        {
            new Employee(1, "John Doe", "Engineer", 60000),
            new Employee(2, "Jane Smith", "Manager", 75000),
            new Employee(3, "Sam Brown", "Technician", 50000)
        };

        public bool EmployeeExists(int id)
        {
            return employees.Any(x => x.Id == id);
        }

        public List<Employee> GetEmployees() => employees;

        public Employee? GetEmployeeById(int id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }

        public void AddEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                int maxId = employees.Max(x => x.Id);
                employee.Id = maxId + 1;
                employees.Add(employee);
            }
        }

        public bool UpdateEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                var emp = employees.FirstOrDefault(x => x.Id == employee.Id);
                if (emp is not null)
                {
                    emp.Name = employee.Name;
                    emp.Position = employee.Position;
                    emp.Salary = employee.Salary;

                    return true;
                }
            }

            return false;
        }

        public bool DeleteEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                employees.Remove(employee);
                return true;
            }

            return false;
        }
    }
}
