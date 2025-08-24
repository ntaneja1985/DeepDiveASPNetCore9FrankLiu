using WebApp.Models;

namespace WebApp.Model
{
    public interface IDepartmentsApiRepository
    {
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int id);
    }

}