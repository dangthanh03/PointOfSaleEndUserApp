using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Models.ViewModel;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Service.Implementation
{
    public class AuthenticateService : IAuthenticateService

    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettingHandler _apiSettingHandler;

        public AuthenticateService(HttpClient httpClient, ApiSettingHandler apiSettingHandler)
        {
            _httpClient = httpClient;
            _apiSettingHandler = apiSettingHandler;
        }

        public async Task<Result<UserProfileVm>> Login(LoginModel model)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();

                // Gửi yêu cầu đăng nhập đến API
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + "UserAuthenticate/Login", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                // Kiểm tra xem phản hồi có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize dữ liệu phản hồi thành UserProfileVm
                    var userProfile = JsonConvert.DeserializeObject<UserProfileVm>(responseData);

                    // Trả về kết quả thành công với dữ liệu UserProfileVm
                    return Result<UserProfileVm>.Success(userProfile);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Xử lý trường hợp BadRequest
                    string errorResponse = await response.Content.ReadAsStringAsync();

                    // Trả về kết quả không thành công với thông tin lỗi
                    return Result<UserProfileVm>.Fail("Login failed: " + errorResponse);
                }
                else
                {
                    // Xử lý trường hợp lỗi không xác định
                    return Result<UserProfileVm>.Fail("An unexpected error occurred while logging in");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi gọi API
                return Result<UserProfileVm>.Fail("An error occurred while processing the request.");
            }
        }




        public async Task<Result<string>> Logout()
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();

                // Gửi yêu cầu Logout đến API
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + "UserAuthenticate/Logout", null);

                // Kiểm tra xem phản hồi có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi nếu cần
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Trả về kết quả thành công
                    return Result<string>.Success(responseData);
                }
                else
                {
                    // Xử lý trường hợp lỗi
                    return Result<string>.Fail("Logout failed: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi gọi API
                return Result<string>.Fail("An error occurred while processing the request.");
            }
        }


        public async Task<Result<string>> Register(RegistrationModel model)
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();

                // Chuyển đối tượng model sang định dạng JSON
                var jsonModel = JsonConvert.SerializeObject(model);

                // Gửi yêu cầu đăng ký đến API
                var response = await _httpClient.PostAsync(apiConfig.BaseUrl + "UserAuthenticate/Register", new StringContent(jsonModel, Encoding.UTF8, "application/json"));

                // Kiểm tra xem phản hồi có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi nếu cần
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Trả về kết quả thành công
                    return Result<string>.Success(responseData);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Xử lý trường hợp BadRequest
                    string errorResponse = await response.Content.ReadAsStringAsync();

                    // Trả về kết quả không thành công với thông tin lỗi
                    return Result<string>.Fail("Register failed: " + errorResponse);
                }
                else
                {
                    // Xử lý trường hợp lỗi không xác định
                    return Result<string>.Fail("An unexpected error occurred while registering");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi gọi API
                return Result<string>.Fail("An error occurred while processing the request.");
            }
        }

        public async Task<Result<UserProfileVm>> GetCurrentUserInfoAsync()
        {
            try
            {
                var apiConfig = _apiSettingHandler.GetApiConfiguration();

                // Gửi yêu cầu lấy thông tin người dùng hiện tại đến API
                var response = await _httpClient.GetAsync(apiConfig.BaseUrl + "UserAuthenticate/current-user");

                // Kiểm tra xem phản hồi có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung phản hồi nếu cần
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Chuyển đổi dữ liệu từ chuỗi JSON sang đối tượng UserProfileVm
                    var userProfile = JsonConvert.DeserializeObject<UserProfileVm>(responseData);

                    // Trả về kết quả thành công
                    return Result<UserProfileVm>.Success(userProfile);
                }
                else
                {
                    // Xử lý trường hợp lỗi
                    return Result<UserProfileVm>.Fail("Failed to get current user information: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi gọi API
                return Result<UserProfileVm>.Fail("An error occurred while processing the request.");
            }
        }
        public async Task<Result<List<string>>> GetAvailableRolesAsync()
        {
            try
            {

                var apiConfig = _apiSettingHandler.GetApiConfiguration();

                // Gửi yêu cầu lấy thông tin người dùng hiện tại đến API
                var response = await _httpClient.GetAsync(apiConfig.BaseUrl + "UserAuthenticate/GetAvailableRoles");

 
                if (response.IsSuccessStatusCode)
                {
                    var roles = await response.Content.ReadAsAsync<List<string>>();
                    return Result<List<string>>.Success(roles);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result<List<string>>.Fail(errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return Result<List<string>>.Fail("An error occurred while retrieving available roles.");
            }
        }



    }
}
