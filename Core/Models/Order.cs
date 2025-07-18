using Core.Enums;
using Core.Interfaces;
namespace Core.Models
{
    public class Order: IDeletable
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
        public bool IsDeleted { get; set; }
    }
}
