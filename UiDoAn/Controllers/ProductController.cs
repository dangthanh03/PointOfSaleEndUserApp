using Microsoft.AspNetCore.Mvc;
using UiDoAn.Service.Abstract;

namespace UiDoAn.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductList(string? searchTerm)
        {
            // Gọi ProductService để lấy danh sách sản phẩm từ API
            var products = await _productService.GetAllProducts(searchTerm);
            return View(products.Data); // Trả về view chứa danh sách sản phẩm
        }
    }
}
