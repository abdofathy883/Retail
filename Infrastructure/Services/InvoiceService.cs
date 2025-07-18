using AutoMapper;
using Core.DTOs;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class InvoiceService: I_InvoiceService
    {
        private readonly IGenericRepo<Invoice> invoiceRepo;
        private readonly IGenericRepo<Order> orderRepo;
        private readonly IMapper mapper;
        public InvoiceService(IGenericRepo<Invoice> repo, 
            IGenericRepo<Order> orderRepo,
            IMapper _mapper)
        {
            invoiceRepo = repo;
            this.orderRepo = orderRepo;
            mapper = _mapper;
        }

        public async Task<InvoiceDTO> GenerateInvoiceAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new Exception();
            }
            var order = await orderRepo.GetByIdAsync(orderId);
            if (order is null || order.Status != Core.Enums.OrderStatus.Paid)
            {
                throw new Exception();
            }
            var invoice = new Invoice
            {
                OrderID = orderId,
                ShippingFee = order.ShippingFee,
                Total = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                PaymentMethod = order.PaymentProvider,
                CreatedAt = DateTime.UtcNow
            };
            await invoiceRepo.AddAsync(invoice);
            await invoiceRepo.SaveAllAsync();
            return mapper.Map<InvoiceDTO>(invoice);
        }

        public async Task<InvoiceDTO> GetInvoiceByIdAsync(Guid invoiceId)
        {
            if (invoiceId == Guid.Empty)
            {
                throw new Exception();
            }
            var invoice = await invoiceRepo.GetByIdAsync(invoiceId)
                ?? throw new InValidObjectException("Invoice not found");

            return mapper.Map<InvoiceDTO>(invoice);
        }

        public Task<InvoiceDTO> GetInvoiceByOrderIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateInvoiceNumberAsync()
        {
            var last = ""; // await invoiceRepo.GetLastAsync();
            var nextNumber = ""; // last != null ? ExtractNumber(last.InvoiceNumber) + 1 : 1;
            return $"INV-{nextNumber:D6}";
        }
    }
}
