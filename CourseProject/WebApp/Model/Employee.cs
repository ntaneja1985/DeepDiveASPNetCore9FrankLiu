using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Position { get; set; }
        
        public double? Salary { get; set; }
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }

        public Employee()
        {            
        }

        public Employee(int id, string name, string position, double salary, int departmentId)
        {
            Id = id;
            Name = name;
            Position = position;
            Salary = salary;
            DepartmentId = departmentId;
        }
    }
}
