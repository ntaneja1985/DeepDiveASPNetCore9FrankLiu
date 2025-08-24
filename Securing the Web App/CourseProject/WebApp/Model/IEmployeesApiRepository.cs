using WebApp.Models;

namespace WebApp.Model
{
    public interface IEmployeesApiRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee? employee);
        Task<bool> DeleteEmployeeAsync(Employee? employee);
        Task<bool> EmployeeExistsAsync(int employeeId);        
        Task<List<Employee>> GetEmployeesAsync(string? filter = null, int? departmentId = null);
        Task<bool> UpdateEmployeeAsync(Employee? employee);
    }
}
