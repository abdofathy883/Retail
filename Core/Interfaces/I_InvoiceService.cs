using Core.DTOs;

namespace Core.Interfaces
{
    public interface I_InvoiceService
    {
        Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId);
        Task<InvoiceDTO> GetInvoiceByIdAsync(Guid invoiceId);
        Task<InvoiceDTO> GetInvoiceByOrderIdAsync(Guid orderId);
    }
}
