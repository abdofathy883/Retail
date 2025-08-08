using AutoMapper;
using Core.DTOs.ProductDTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfiles
{
    public class ProductVariantProfile: Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.SizeId, opt => opt.MapFrom(src => src.SizeId))
                .ForMember(dest => dest.VariantImageId, opt => opt.MapFrom(src => src.VariantImageId))
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.OriginalPrice))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.SalePrice))
                .ForMember(dest => dest.WholesalePrice, opt => opt.MapFrom(src => src.WholesalePrice))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.NuOfPurchases, opt => opt.MapFrom(src => src.NuOfPurchases))
                .ForMember(dest => dest.NuOfPutInCart, opt => opt.MapFrom(src => src.NuOfPutInCart))
                .ForMember(dest => dest.NuOfPutInWishList, opt => opt.MapFrom(src => src.NuOfPutInWishList))
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.IsFeatured));
        }
    }
}
