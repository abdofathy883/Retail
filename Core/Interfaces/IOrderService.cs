using Core.DTOs;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto);
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId); //////
        Task<OrderDTO> GetLastUndeliveredOrderByUserIdAsync(string userId); //////
        Task<OrderDTO> GetOrderByIdAsync(Guid orderId);
        //Task<bool> CancelOrderAsync(Guid orderId, string userId);
        Task<OrderDTO> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<PaymentSessionDTO> InitiatePaymentAsync(Guid orderId);
    }
}
