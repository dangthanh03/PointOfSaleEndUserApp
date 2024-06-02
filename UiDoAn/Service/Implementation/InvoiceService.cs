using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Service.Implementation
{
    public class InvoiceService: IInvoiceService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettingHandler _apiSettingHandler;

        public InvoiceService(HttpClient httpClient, ApiSettingHandler apiSettingHandler)
        {
            _httpClient = httpClient;
            _apiSettingHandler = apiSettingHandler;
        }

        public async Task<Result<List<InvoiceDTO>>> GetInvoicesByCustomerId(string UserId)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();
                var response = await _httpClient.GetAsync($"{apiConfig.BaseUrl}Invoice/User/{UserId}");
                response.EnsureSuccessStatusCode();
                var invoices = await response.Content.ReadAsAsync<List<InvoiceDTO>>();
                return Result<List<InvoiceDTO>>.Success(invoices);
            }
            catch (Exception ex)
            {
                return Result<List<InvoiceDTO>>.Fail($"Failed to get invoices for customer ID {UserId}: {ex.Message}");
            }
        }
    }
}
