using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CartItem: IDeletable
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = default!;
        public int? ProductVariantId { get; set; }
        public ProductVarient? ProductVarient { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceSnapshot { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
