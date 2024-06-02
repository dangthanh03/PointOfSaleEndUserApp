using UiDoAn.Models.Domain;
using Microsoft.Extensions.Options;
using UiDoAn.Service.Abstract;
using System.Net.Http;
using System.Net.Http.Json;

namespace UiDoAn.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettingHandler _apiSettingHandler;

        public ProductService(HttpClient httpClient, ApiSettingHandler apiSettingHandler)
        {
            _httpClient = httpClient;
            _apiSettingHandler = apiSettingHandler;
        }

        public async Task<Result<IEnumerable<Product>>> GetAllProducts(string searchTerm = null)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                string apiUrl = apiConfig.BaseUrl + "Product";

                // Thêm tham số searchTerm vào URL nếu nó được cung cấp
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    apiUrl += $"?searchTerm={searchTerm}";
                }

                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                return Result<IEnumerable<Product>>.Success(products);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Product>>.Fail("An error occurred while fetching products: " + ex.Message);
            }
        }


        public async Task<Result<Product>> GetProductById(int id)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.GetAsync(apiConfig.BaseUrl + "Products/" + id);
                response.EnsureSuccessStatusCode();
                var product = await response.Content.ReadAsAsync<Product>();
                return Result<Product>.Success(product);
            }
            catch (Exception ex)
            {
                return Result<Product>.Fail("Failed to get product: " + ex.Message);
            }
        }

        public async Task<Result<Product>> AddProduct(Product product)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.PostAsJsonAsync(apiConfig.BaseUrl + "Products", product);
                response.EnsureSuccessStatusCode();
                var addedProduct = await response.Content.ReadAsAsync<Product>();
                return Result<Product>.Success(addedProduct);
            }
            catch (Exception ex)
            {
                return Result<Product>.Fail("Failed to add product: " + ex.Message);
            }
        }

        public async Task<Result<string>> UpdateProduct(Product product)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.PutAsJsonAsync(apiConfig.BaseUrl + "Products/AdjustProduct", product);
                response.EnsureSuccessStatusCode();
                return Result<string>.Success("Product updated successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to update product: " + ex.Message);
            }
        }

        public async Task<Result<string>> DeleteProduct(int id)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.DeleteAsync(apiConfig.BaseUrl + "Products/" + id);
                response.EnsureSuccessStatusCode();
                return Result<string>.Success("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to delete product: " + ex.Message);
            }
        }
    }
}
