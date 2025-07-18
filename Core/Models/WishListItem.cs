using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WishListItem: IDeletable
    {
        public int Id { get; set; }
        public int ProductVarientId { get; set; }
        public ProductVarient ProductVarient { get; set; } = default!;
        public int WishListId { get; set; }
        public WishList WishList { get; set; } = default!;
        public DateTime AddedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
