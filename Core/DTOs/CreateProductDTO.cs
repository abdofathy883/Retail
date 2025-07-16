using Core.Enums;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<IFormFile>? ImageUrls { get; set; }
        public Guid CategoryId { get; set; }

        public ProductType ProductType { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public List<Create_UpdateProductVariantDTO> ProductVarients { get; set; } = new();
    }
}
