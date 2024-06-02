using UiDoAn.Models.Domain;

namespace UiDoAn.Service.Abstract
{
    public interface IProductService
    {
        Task<Result<IEnumerable<Product>>> GetAllProducts(string searchTerm = null);
        Task<Result<Product>> GetProductById(int id);
        Task<Result<Product>> AddProduct(Product product);
        Task<Result<string>> UpdateProduct(Product product);
        Task<Result<string>> DeleteProduct(int id);

    }

}

