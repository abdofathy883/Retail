using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface I_InvoiceService
    {
        Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId);
        Task<InvoiceDTO> GetInvoiceByIdAsync(Guid invoiceId);
        Task<InvoiceDTO> GetInvoiceByOrderIdAsync(Guid orderId);
    }
}
