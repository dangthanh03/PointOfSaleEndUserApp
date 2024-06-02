using UiDoAn.Models.Domain;
using UiDoAn.Models.ViewModel;

namespace UiDoAn.Service.Abstract
{
    public interface ICartService
    {
        Task<Result<List<ProductCartVm>>> CheckCart(string userId);
        Task<Result<string>> AddToCart(int productId, int quantity, string userId);
        Task<Result<string>> RemoveFromCart(int productId, string userId);
        Task<Result<string>> Buy(string userId);
    }
}
