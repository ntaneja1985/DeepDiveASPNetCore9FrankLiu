using DepartmentsAssignment.Models;
using System.Xml.Linq;

namespace DepartmentsAssignment.Repositories
{

        public static class DepartmentsRepository
        {
            private static List<Department> Departments = new List<Department>
        {
            new Department(1, "Sales", "Sales Department"),
            new Department(2, "Engineering", "Engineering Department"),
            new Department(3, "QA", "Quanlity Assurance")
        };

            public static List<Department> GetDepartments() => Departments;

            public static Department? GetDepartmentById(int id)
            {
                return Departments.FirstOrDefault(x => x.Id == id);
            }

            public static void AddDepartment(Department? Department)
            {
                if (Department is not null)
                {
                    int maxId = Departments.Max(x => x.Id);
                    Department.Id = maxId + 1;
                    Departments.Add(Department);
                }
            }

            public static bool UpdateDepartment(Department? Department)
            {
                if (Department is not null)
                {
                    var emp = Departments.FirstOrDefault(x => x.Id == Department.Id);
                    if (emp is not null)
                    {
                        emp.Name = Department.Name;
                        emp.Description = Department.Description;

                        return true;
                    }
                }

                return false;
            }

            public static bool DeleteDepartment(Department? Department)
            {
                if (Department is not null)
                {
                    Departments.Remove(Department);
                    return true;
                }

                return false;
            }
        }

}
