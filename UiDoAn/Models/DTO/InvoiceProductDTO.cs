using UiDoAn.Models.Domain;

namespace UiDoAn.Models.DTO
{
    public class InvoiceProductDTO
    {
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
