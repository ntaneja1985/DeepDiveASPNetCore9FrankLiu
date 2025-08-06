using LearnRouting.Validation;

namespace LearnRouting.Models
{
    public class Employee
    {

        public Employee(int id, string name, string position, int salary)
        {
            Id = id;
            Name = name;
            Position = position;
            Salary = salary; 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        [Employee_EnsureSalary]
        public int Salary { get; set; }
    }
}
