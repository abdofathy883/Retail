using AutoMapper;
using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class OrderService: IOrderService
    {
        private readonly IGenericRepo<Order> orderRepo;
        private readonly IGenericRepo<Product> productRepo;
        private readonly IGenericRepo<ProductVarient> productVariantRepo;
        private readonly I_InvoiceService invoiceService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly EmailService emailService;
        private readonly IMapper mapper;
        public OrderService(IGenericRepo<Order> genericRepo, 
            UserManager<ApplicationUser> userManager,
            IGenericRepo<Product> repo,
            I_InvoiceService invoice,
            EmailService emailService,
            IGenericRepo<ProductVarient> productVariantRepo,
            IMapper _mapper)
        {
            orderRepo = genericRepo;
            this.userManager = userManager;
            productRepo = repo;
            this.invoiceService = invoice;
            this.emailService = emailService;
            this.productVariantRepo = productVariantRepo;
            mapper = _mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO newOrderDTO)
        {
            if (newOrderDTO is null)
                throw new InValidObjectException(""); 
            
            var user = await GetUserOrThrow(newOrderDTO.UserId);

            var order = new Order
            {
                UserId = newOrderDTO.UserId,
                //Items = newOrderDTO.Items,
                //TotalAmount = newOrderDTO.TotalAmount,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                PaymentProvider = newOrderDTO.PaymentProvider,
                ShippingAddress = new ShippingAddress
                {
                    Country = newOrderDTO.ShippingAddress.Country,
                    City = newOrderDTO.ShippingAddress.City,
                    PostalCode = newOrderDTO.ShippingAddress.PostalCode,
                    StreetName = newOrderDTO.ShippingAddress.StreetName,
                    BuildingBumber = newOrderDTO.ShippingAddress.BuildingBumber
                }
            };

            await orderRepo.AddAsync(order);
            await orderRepo.SaveAllAsync();

            foreach (var item in order.Items)
            {
                if (item.ProductVariantId == null)
                {
                    throw new Exception();
                }
                var variant = await productVariantRepo.GetByIdAsync(item.ProductVariantId)
                    ?? throw new InValidObjectException("");
               
                variant.Stock = variant.Stock - item.Quantity;
                productVariantRepo.Update(variant);
                await productRepo.SaveAllAsync();
            }
            await invoiceService.GenerateInvoiceAsync(order.Id);
            return mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid orderId)
        {
            var order = await orderRepo.GetByIdAsync(orderId)
                ?? throw new InValidObjectException("Order not found");

            return mapper.Map<OrderDTO>(order);
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }
            await GetUserOrThrow(userId);
            var orders = await orderRepo.FindAsync(o => o.UserId == userId)
                ?? throw new InValidObjectException("No orders found for this user");
            
            return mapper.Map<List<OrderDTO>>(orders);
        }


        public async Task<OrderDTO> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            if (orderId == Guid.Empty || string.IsNullOrEmpty(status.ToString()))
            {
                throw new Exception();
            }
            var order = await orderRepo.GetByIdAsync(orderId);
            if (order is null)
            {
                throw new Exception();
            }
            order.Status = status;
            orderRepo.Update(order);
            await orderRepo.SaveAllAsync();

            return mapper.Map<OrderDTO>(order);
        }

        public Task<PaymentSessionDTO> InitiatePaymentAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
        private async Task<ApplicationUser> GetUserOrThrow(string customerId)
        {
            var user = await userManager.FindByIdAsync(customerId);
            return user ?? throw new Exception("User not found");
        }

        public Task<OrderDTO> GetLastUndeliveredOrderByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
