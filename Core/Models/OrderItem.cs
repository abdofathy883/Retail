using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string? VariantSummary { get; set; }
    }
}
