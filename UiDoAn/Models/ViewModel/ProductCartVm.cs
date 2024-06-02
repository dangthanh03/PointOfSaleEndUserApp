using UiDoAn.Models.Domain;

namespace UiDoAn.Models.ViewModel
{
    public class ProductCartVm
    {
        
        public int ProductId { get; set; }
        public Product product { get; set; }
        public decimal? Price { get; set; }
        public int OrderQuanity { get; set; }
    }
}
