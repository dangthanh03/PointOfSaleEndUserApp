using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Models.ViewModel;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static string NameUser="Guest";
        private readonly IAuthenticateService _authService;
        public AuthenticateController(IAuthenticateService authenticateService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authenticateService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Logout()
        {
            // Gọi hàm Logout từ service để đăng xuất người dùng
            var result = await _authService.Logout();

            // Kiểm tra kết quả trả về từ service
            if (result.IsSuccess)
            {
                // Xóa UserProfile và UserId ra khỏi session
                _httpContextAccessor.HttpContext.Session.Remove("UserProfile");
                _httpContextAccessor.HttpContext.Session.Remove("UserId");

                NameUser = "Guest";
                // Đăng xuất thành công, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login");
            }
            else
            { // Trả về view _Error và truyền thông điệp lỗi
                return View("_Error", result.Message);
            }
        }



        public async Task<IActionResult> Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Gọi hàm Login từ service để xác thực người dùng
            var result = await _authService.Login(model);

            // Kiểm tra kết quả trả về từ service
            if (result.IsSuccess)
            {
                var userProfile = result.Data;
                _httpContextAccessor.HttpContext.Session.SetString("UserProfile", JsonConvert.SerializeObject(userProfile));
                _httpContextAccessor.HttpContext.Session.SetString("UserId", userProfile.id); // Lưu id vào session

                // Đăng nhập thành công, chuyển hướng đến trang chính
                return RedirectToAction("ProductList", "Product");
            }
            else
            {
                // Đăng nhập không thành công, hiển thị thông báo lỗi
                ViewData["ErrorMessage"] = result.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            // Get available roles
            var rolesResult = await _authService.GetAvailableRolesAsync();
            if (!rolesResult.IsSuccess)
            {
                // Handle failure to get roles
                ViewData["ErrorMessage"] = rolesResult.Message;
                return View("Error");
            }

            // Create a new instance of RegistrationModel and set available roles
            var model = new RegistrationModel
            {
                AvailableRoles = rolesResult.Data
            };

            // Pass the model to the view
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "FIll all required information";
                return View(model);
            }

            var result = await _authService.Register(model);

            if (result.IsSuccess)
            {
                return RedirectToAction("Login","Authenticate"); // Return success message or any data
            }
            else
            {
                ViewData["ErrorMessage"] = result.Message;
                return View(model);
            }
        }
    }
}
