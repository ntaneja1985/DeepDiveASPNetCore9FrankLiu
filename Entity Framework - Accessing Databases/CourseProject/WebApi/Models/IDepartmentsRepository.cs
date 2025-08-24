
namespace WebApi.Models
{
    public interface IDepartmentsRepository
    {
        void AddDepartment(Department? department);
        bool DeleteDepartment(Department? department);
        bool DepartmentExists(int departmentId);
        Department? GetDepartmentById(int id);
        List<Department> GetDepartments(string? filter = null);
        bool UpdateDepartment(Department? department);
    }
}