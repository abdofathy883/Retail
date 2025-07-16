using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Create_UpdateProductVariantDTO
    {
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }
    }
}
