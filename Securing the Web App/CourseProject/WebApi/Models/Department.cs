using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Department
    {
        public Department()
        {
            
        }

        public Department(int id, string name, string? description = "")
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
