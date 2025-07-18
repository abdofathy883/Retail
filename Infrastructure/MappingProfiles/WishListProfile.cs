using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Infrastructure.MappingProfiles
{
    public class WishListProfile: Profile
    {
        public WishListProfile()
        {
            CreateMap<WishList, WishListDTO>();
            CreateMap<WishListItem, WishListItemDTO>();
        }
    }
}
