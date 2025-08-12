
namespace WebApi.Models
{
    public interface IEmployeesRepository
    {
        void AddEmployee(Employee? employee);
        bool DeleteEmployee(Employee? employee);
        bool EmployeeExists(int id);
        Employee? GetEmployeeById(int id);
        List<Employee> GetEmployees();
        bool UpdateEmployee(Employee? employee);
    }
}