using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Infrastructure.MappingProfiles
{
    public class CartProfile: Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDTO>();
            CreateMap<CartItem, CartItemDTO>();
        }
    }
}
