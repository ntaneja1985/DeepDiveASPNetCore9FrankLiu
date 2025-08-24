
namespace WebApp.Models
{
    public interface IDepartmentsApiRepository
    {
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(Department? department);
        Task<bool> DeleteDepartmentAsync(Department? department);
        Task<bool> DepartmentExistsAsync(int departmentId);        
        Task<List<Department>> GetDepartmentsAsync(string? filter = null);
        Task<bool> UpdateDepartmentAsync(Department? department);
    }
}