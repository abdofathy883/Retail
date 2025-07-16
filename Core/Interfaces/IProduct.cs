using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProduct
    {
        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
    }
}
