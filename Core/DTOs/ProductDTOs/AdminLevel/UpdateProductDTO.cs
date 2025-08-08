using Core.Enums;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProductDTOs.AdminLevel
{
    public class UpdateProductDTO
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<Create_UpdateProductVariantDTO>? ProductVariants { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
