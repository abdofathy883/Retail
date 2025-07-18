using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Infrastructure.MappingProfiles
{
    internal class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>();
        }
    }
}
