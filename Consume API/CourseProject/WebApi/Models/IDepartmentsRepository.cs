
namespace WebApi.Models
{
    public interface IDepartmentsRepository
    {
        void AddDepartment(Department? Department);
        bool DeleteDepartment(Department? Department);
        bool DepartmentExists(int departmentId);
        Department? GetDepartmentById(int id);
        List<Department> GetDepartments(string? filter = null);
        bool UpdateDepartment(Department? Department);
    }
}