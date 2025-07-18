namespace Core.Interfaces
{
    public interface IProduct
    {
        public int NuOfPurchases { get; set; }
        public int NuOfPutInCart { get; set; }
        public int NuOfPutInWishList { get; set; }
    }
}
