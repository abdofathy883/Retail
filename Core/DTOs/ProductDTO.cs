using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<ProductImage>? ImageUrls { get; set; }
        public ProductType ProductType { get; set; }
        public List<ProductVarient>? ProductVarients { get; set; }
        public Guid CategoryId { get; set; }
        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
    }
}
