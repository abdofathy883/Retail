using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Invoice: IAuditable, IDeletable
    {
        public Guid Id { get; set; }
        public Guid OrderID { get; set; }
        public Order Order { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }

        public ShippingAddress ShippingAddress { get; set; } = new ShippingAddress();
        public PaymentProvider PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
