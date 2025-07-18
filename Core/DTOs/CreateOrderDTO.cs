using Core.Enums;
using Core.Models;

namespace Core.DTOs
{
    public class CreateOrderDTO
    {
        public string UserId { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; } = new();
        public ShippingAddress ShippingAddress { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }
}
