using Microsoft.Extensions.Configuration;

namespace UiDoAn.Models.Domain
{
    public class ApiSettingHandler
    {
        private readonly IConfiguration _configuration;

        public ApiSettingHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApiSettings GetApiConfiguration()
        {
            var apiConfig = new ApiSettings();
            _configuration.GetSection("ApiSettings").Bind(apiConfig);
            return apiConfig;
        }
    }
}
