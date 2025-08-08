using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProductDTOs.AdminLevel
{
    public class Create_UpdateProductVariantDTO
    {
        public int? Id { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }
        public ProductImageDTO VariantImage { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFeatured { get; set; }
    }
}
