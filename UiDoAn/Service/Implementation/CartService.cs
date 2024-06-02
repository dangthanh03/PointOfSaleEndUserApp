using Newtonsoft.Json;
using UiDoAn.Models.Domain;
using UiDoAn.Models.ViewModel;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Service.Implementation
{
    public class CartService:ICartService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettingHandler _apiSettingHandler;

        public CartService(HttpClient httpClient, ApiSettingHandler apiSettingHandler)
        {
            _httpClient = httpClient;
            _apiSettingHandler = apiSettingHandler;
        }

        public async Task<Result<List<ProductCartVm>>> CheckCart(string userId)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.GetAsync(apiConfig.BaseUrl + $"Cart/CheckCart?Id={userId}");
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadAsAsync<List<ProductCartVm>>();
                return Result<List<ProductCartVm>>.Success(products);
            }
            catch (Exception ex)
            {
                return Result<List<ProductCartVm>>.Fail("An error occurred while fetching cart: " + ex.Message);
            }
        }
        public async Task<Result<string>> AddToCart(int productId, int quantity,string userId)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + $"Cart/AddToCart/{productId}?quantity={quantity}&userId={userId}", null);
                response.EnsureSuccessStatusCode();
                return Result<string>.Success("Product added to cart successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to add product to cart: " + ex.Message);
            }
        }

        public async Task<Result<string>> RemoveFromCart(int productId, string userId)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + $"Cart/RemoveFromCart/{productId}?userId={userId}", null);
                response.EnsureSuccessStatusCode();
                return Result<string>.Success("Product removed from cart successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to remove product from cart: " + ex.Message);
            }
        }

        public async Task<Result<string>> Buy(string userId)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + $"Cart/Buy?userId={userId}", null);
                response.EnsureSuccessStatusCode();
                return Result<string>.Success("Purchase completed successfully");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to complete purchase: " + ex.Message);
            }
        }

    }
}
