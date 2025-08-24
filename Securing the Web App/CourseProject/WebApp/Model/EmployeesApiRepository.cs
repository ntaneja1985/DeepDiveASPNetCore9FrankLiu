using System.Text.Json;
using System.Text;
using WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Model
{
    public class EmployeesApiRepository : IEmployeesApiRepository
    {
        private readonly IHttpClientFactory httpClientFactory;

        public EmployeesApiRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task AddEmployeeAsync(Employee? employee)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var employeeJson = new StringContent(
                JsonSerializer.Serialize(employee),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PostAsync("/employees", employeeJson);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DeleteEmployeeAsync(Employee? employee)
        {
            if (employee is null) return false;

            var client = httpClientFactory.CreateClient("ApiEndpoints");



            var response = await client.DeleteAsync($"/employees/{employee.Id}");
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> EmployeeExistsAsync(int employeeId)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"employees/{employeeId}/exists");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return bool.Parse(responseString);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"employees/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Employee>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<List<Employee>> GetEmployeesAsync(string? filter = null, int? departmentId = null)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"employees/search?filter={filter??string.Empty}&departmentId={departmentId??0}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<Employee>>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (employees is null) employees = new List<Employee>();

            return employees;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee? employee)
        {
            if (employee is null) return false;

            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var employeeJson = new StringContent(
                JsonSerializer.Serialize(employee),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PutAsync($"/employees/{employee.Id}", employeeJson);
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
