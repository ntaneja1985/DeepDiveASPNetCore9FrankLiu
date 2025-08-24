using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace WebApp.Models
{
    public class DepartmentsApiRepository : IDepartmentsApiRepository
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DepartmentsApiRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task AddDepartmentAsync(Department? department)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var departmentJson = new StringContent(
                JsonSerializer.Serialize(department),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PostAsync("/departments", departmentJson);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DeleteDepartmentAsync(Department? department)
        {
            if (department is null) return false;

            var client = httpClientFactory.CreateClient("ApiEndpoints");



            var response = await client.DeleteAsync($"/departments/{department.Id}");
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> DepartmentExistsAsync(int departmentId)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"departments/{departmentId}/exists");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return bool.Parse(responseString);
        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"departments/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Department>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<List<Department>> GetDepartmentsAsync(string? filter = null)
        {
            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var response = await client.GetAsync($"departments/{filter}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var departments = JsonSerializer.Deserialize<List<Department>>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (departments is null) departments = new List<Department>();

            return departments;
        }

        public async Task<bool> UpdateDepartmentAsync(Department? department)
        {
            if (department is null) return false;

            var client = httpClientFactory.CreateClient("ApiEndpoints");

            var departmentJson = new StringContent(
                JsonSerializer.Serialize(department),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PutAsync($"/departments/{department.Id}", departmentJson);
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
