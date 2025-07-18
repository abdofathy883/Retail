using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Cart: IDeletable
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? CartToken { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
