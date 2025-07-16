using Core.Enums;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Product: IAuditable, IDeletable, IProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<ProductImage>? ImageUrls { get; set; }
        public ProductType ProductType { get; set; }
        public List<ProductVarient>? ProductVarients { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
