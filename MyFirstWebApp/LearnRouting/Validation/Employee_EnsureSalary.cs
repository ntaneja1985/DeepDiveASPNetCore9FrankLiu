using LearnRouting.Models;
using System.ComponentModel.DataAnnotations;

namespace LearnRouting.Validation
{
    public class Employee_EnsureSalary : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var employee = validationContext.ObjectInstance as Employee;
            if (employee != null && !string.IsNullOrEmpty(employee.Position) &&
                employee.Position.Equals("Manager",StringComparison.OrdinalIgnoreCase)
                )
            {
                if(employee.Salary < 100000)
                {
                    return new ValidationResult("A manager's salary has to be greater than 100000");
                }
            }
            return ValidationResult.Success;
        }
    }
}
