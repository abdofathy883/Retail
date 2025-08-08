using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.WishListDTOs
{
    public class AddToWishListDTO
    {
        public string CustomerId { get; set; }
        public int productVarientId { get; set; }
    }
}
