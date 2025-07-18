using Core.Enums;
using Core.Models;

namespace Core.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItemDTO> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime DeliveredAt { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
        public ShippingAddress ShippingAddress { get; set; } = default!;
    }
}
