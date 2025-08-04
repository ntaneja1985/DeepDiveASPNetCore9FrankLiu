using LearnRouting.Models;

namespace LearnRouting.Repository
{
    public static class EmployeeRepository
    {
        public static List<Employee> employees = new List<Employee>
        {
            new Employee(1, "Alice", "Manager",1000),
            new Employee(2, "Bob", "Developer",2000),
            new Employee(3, "Charlie", "Developer",3000),
        };

        public static bool AddEmployee(Employee employee)
        {
            //Check if employee already exists with the same id
            if (employees.Any(x => x.Id == employee.Id))
            {
                return false;
            }
            employees.Add(employee);
            return true;
        }

        public static bool UpdateEmployee(Employee employee)
        {
            //Check if employee already exists with the same id
            if (employees.Count(x => x.Id == employee.Id) == 0)
            {
                return false;
            }
            //Remove the existing employee
            employees.RemoveAll(x => x.Id == employee.Id);
            //Add the updated employee
            employees.Add(employee);
            return true;
        }

        public static bool DeleteEmployee(int id)
        {
            //Check if employee exists with the same id
            if (employees.Count(x => x.Id == id) == 0)
            {
                return false;
            }
            //Remove the employee
            employees.RemoveAll(x => x.Id == id);
            return true;
        }

        public static Employee? GetEmployee(int id)
        {
            //Check if employee exists with the same id
            return employees.FirstOrDefault(x => x.Id == id);
        }

        public static List<Employee> GetEmployees()
        {
            //Return all employees
            return employees;
        }
    }
}
