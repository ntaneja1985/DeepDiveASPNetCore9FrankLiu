using Microsoft.Data.SqlClient;

string connectionString = "My Connection string here";

var department = GetDepartmentById(1); // Replace 1 with the desired department ID
if (department != null)
{
    Console.WriteLine($"Department: {department.Name}, Description: {department.Description}");
}
else
{
    Console.WriteLine("Department not found.");
}

Department GetDepartmentById(int departmentId)
{
    Department department = null;

    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        var query = "SELECT Id, Name, Description FROM Departments WHERE Id = @DepartmentId";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    department = new Department
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description"))
                    };
                }
            }
        }
    }

    return department;
}
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


