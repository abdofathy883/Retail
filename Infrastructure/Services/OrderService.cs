using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class OrderService: IOrderService
    {
        private readonly IGenericRepo<Order> orderRepo;
        private readonly IGenericRepo<Product> productRepo;
        private readonly I_InvoiceService invoiceService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly EmailService emailService;
        public OrderService(IGenericRepo<Order> genericRepo, 
            UserManager<ApplicationUser> userManager,
            IGenericRepo<Product> repo,
            I_InvoiceService invoice,
            EmailService emailService)
        {
            orderRepo = genericRepo;
            this.userManager = userManager;
            productRepo = repo;
            this.invoiceService = invoice;
            this.emailService = emailService;
        }

        public Task<bool> CancelOrderAsync(Guid orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> CreateOrderAsync(string userId, CreateOrderDTO newOrderDTO)
        {
            if (string.IsNullOrEmpty(userId) || newOrderDTO is null)
            {
                throw new Exception();   
            }
            await GetUserOrThrow(userId);

            var order = new Order
            {
                UserId = newOrderDTO.UserId,
                Items = newOrderDTO.Items,
                TotalAmount = newOrderDTO.TotalAmount,
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
                if (item.ProductId == null)
                {
                    throw new Exception();
                }
                var product = await productRepo.GetByIdAsync(item.ProductId);
                if (product is null)
                {
                    throw new Exception();
                }
                product.Stock = product.Stock - item.Quantity;
                productRepo.Update(product);
                await productRepo.SaveAllAsync();
            }
            await invoiceService.GenerateInvoiceAsync(order.Id);
            return order;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new Exception();
            }
            var order = await orderRepo.GetByIdAsync(orderId);
            if (order is null)
            {
                throw new Exception();
            }
            return new OrderDTO
            {
                UserId = order.UserId,
                Items = new List<OrderItemDTO>
                {
                    
                },
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = new ShippingAddress
                {
                    Country = order.ShippingAddress.Country,
                    City = order.ShippingAddress.City,
                    PostalCode = order.ShippingAddress.PostalCode,
                    StreetName = order.ShippingAddress.StreetName,
                    BuildingBumber = order.ShippingAddress.BuildingBumber
                }
            };
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }
            await GetUserOrThrow(userId);
            var orders = await orderRepo.FindAsync(o => o.UserId == userId);
            if (orders is null)
            {
                throw new Exception();
            }
            return new List<OrderDTO>
            {

            };
        }

        public Task<PaymentSessionDTO> InitiatePaymentAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
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
            return await orderRepo.SaveAllAsync();
        }

        private async Task<ApplicationUser> GetUserOrThrow(string customerId)
        {
            var user = await userManager.FindByIdAsync(customerId);
            return user ?? throw new Exception("User not found");
        }
    }
}
