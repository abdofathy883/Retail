using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int WishListId { get; set; }
        public WishList WishList { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
