using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class InvoiceDTO
    {
        public string InvoiceNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
        public PaymentProvider PaymentMethod { get; set; }

        public List<InvoiceItemDTO> Items { get; set; }
    }
}
