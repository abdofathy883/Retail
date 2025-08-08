using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProductDTOs
{
    public class ProductImageDTO
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; } = default!;
        public bool IsFeatured { get; set; }
        public string AltText { get; set; }
        public int ProductVariantId { get; set; }
    }
}
