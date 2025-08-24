using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Employee
    {
        [HiddenInput]        
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Position { get; set; }
        
        public double? Salary { get; set; }

        [Display(Name="Department")]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "The department is required.")]
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
