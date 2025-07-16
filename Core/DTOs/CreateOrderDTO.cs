using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateOrderDTO
    {
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }
}
