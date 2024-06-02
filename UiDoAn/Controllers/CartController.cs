using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Models.ViewModel;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ActionResult<Result<List<ProductCartVm>>>> CheckCart()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = await _cartService.CheckCart(userId);
            if (result.IsSuccess)
            {
                return View(result.Data);
            }
            else
            {
               // Trả về view _Error và truyền thông điệp lỗi
                return View("_Error", result.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<Result<string>>> AddToCart(int productId,  int quantity)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = await _cartService.AddToCart(productId, quantity,userId);
            if (result.IsSuccess)
            {
                return RedirectToAction("CheckCart");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("CheckCart");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> RemoveFromCart(int productId)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = await _cartService.RemoveFromCart(productId,userId);
            if (result.IsSuccess)
            {
                return RedirectToAction("CheckCart");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("CheckCart");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Result<string>>> Buy()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = await _cartService.Buy(userId);
            if (result.IsSuccess)
            {
                return RedirectToAction("GetCurrentUserInvoices", "Invoice");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("CheckCart");
            }
        }
    }
}
