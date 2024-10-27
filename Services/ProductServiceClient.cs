using OrderService.Models;
using System.Text.Json;

namespace OrderService.Services
{
    public class ProductServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:7003/api/product/{productId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
