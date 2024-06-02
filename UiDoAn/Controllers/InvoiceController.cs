using Microsoft.AspNetCore.Mvc;
using UiDoAn.Models.Domain;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IAuthenticateService authenticateService;
        private readonly IInvoiceService invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceController(IAuthenticateService authenticateService, IInvoiceService invoiceService, IHttpContextAccessor httpContextAccessor)
        {
            this.authenticateService = authenticateService;
            this.invoiceService = invoiceService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> GetCurrentUserInvoices()
        {
            // Lấy id của người dùng từ session
            var userId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            // Gọi hàm GetInvoicesByCustomerId từ service với id của người dùng
            var invoices = await invoiceService.GetInvoicesByCustomerId(userId);

            // Kiểm tra kết quả và trả về view
            if (invoices.IsSuccess)
            {
                return View(invoices.Data);
            }
            else
            {
                // Xử lý trường hợp không thành công
               // Trả về view _Error và truyền thông điệp lỗi
                return View("_Error", invoices.Message);
            }
        }

    }
}
