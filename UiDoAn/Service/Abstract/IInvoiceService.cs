using UiDoAn.Models.Domain;
using UiDoAn.Models.DTO;
using UiDoAn.Models.ViewModel;

namespace UiDoAn.Service.Abstract
{
    public interface IInvoiceService
    {
        Task<Result<List<InvoiceDTO>>> GetInvoicesByCustomerId(string customerId);
    }
}
