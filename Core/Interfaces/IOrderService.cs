using Core.DTOs;
using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderDTO dto);
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId);
        Task<OrderDTO> GetOrderByIdAsync(Guid orderId);
        Task<bool> CancelOrderAsync(Guid orderId, string userId);
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<PaymentSessionDTO> InitiatePaymentAsync(Guid orderId);
    }
}
