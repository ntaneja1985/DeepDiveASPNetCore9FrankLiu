using System.Net.Http;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Model
{
    public class DepartmentsApiRepository : IDepartmentsApiRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;

        public DepartmentsApiRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.GetAsync("departments");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Department>>(_options);
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.GetAsync($"departments/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Department>(_options);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("departments", department);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.PutAsJsonAsync($"departments/{department.Id}", department);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var response = await client.DeleteAsync($"departments/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

}
