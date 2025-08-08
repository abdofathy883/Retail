using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProductDTOs
{
    public class ProductVariantDTO
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int VariantImageId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? WholesalePrice { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;
        public string? Barcode { get; set; }

        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
        public bool IsFeatured { get; set; }
    }
}
