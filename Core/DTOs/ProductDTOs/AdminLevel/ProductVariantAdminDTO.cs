using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProductDTOs.AdminLevel
{
    public class ProductVariantAdminDTO
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int VariantImageId { get; set; } = new();
        public decimal OriginalPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = default!;

        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
        public bool IsFeatured { get; set; }
    }
}
