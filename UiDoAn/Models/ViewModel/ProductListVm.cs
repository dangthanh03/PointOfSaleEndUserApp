using UiDoAn.Models.Domain;

namespace UiDoAn.Models.ViewModel
{
    public class ProductListVm
    {
        public string? SearchTerm { get; set; }
        public List<Product> Products { get; set; }
    }
}
