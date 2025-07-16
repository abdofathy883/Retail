using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ProductVarient: IDeletable
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int? ColorId { get; set; }
        public Color? Color { get; set; }
        public int? SizeId { get; set; }
        public Size? Size { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }
        public bool IsDeleted { get; set; }
    }
}
