
namespace WebApp.Models
{
    public class EmployeesRepository : IEmployeesRepository
    {
        public EmployeesRepository(IDepartmentsRepository departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        private List<Employee> _employees = new List<Employee>
        {
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
        };
        private readonly IDepartmentsRepository departmentsRepository;

        public bool EmployeeExists(int employeeId)
        {
            return _employees.Any(x => x.Id == employeeId);
        }

        public List<Employee> GetEmployees(string? filter = null, int? departmentId = null)
        {
            foreach (Employee emp in _employees)
            {
                emp.Department = departmentsRepository.GetDepartmentById(emp.DepartmentId);
            }

            if (departmentId.HasValue)
            {
                return _employees.Where(x => x.DepartmentId == departmentId.Value).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(filter))
            {
                return _employees.Where(x => x.Name is not null && x.Name.ToLower().Contains(filter.ToLower())).ToList();
            }

            return _employees;
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employees.FirstOrDefault(x => x.Id == id);
        }

        public void AddEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                int maxId = _employees.Max(x => x.Id);
                employee.Id = maxId + 1;
                _employees.Add(employee);
            }
        }

        public bool UpdateEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                var emp = _employees.FirstOrDefault(x => x.Id == employee.Id);
                if (emp is not null)
                {
                    emp.Name = employee.Name;
                    emp.Position = employee.Position;
                    emp.Salary = employee.Salary;
                    emp.DepartmentId = employee.DepartmentId;

                    return true;
                }
            }

            return false;
        }

        public bool DeleteEmployee(Employee? employee)
        {
            if (employee is not null)
            {
                _employees.Remove(employee);
                return true;
            }

            return false;
        }
    }
}
