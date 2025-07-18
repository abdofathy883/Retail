using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateOrderItemDTO
    {
        public int? ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
