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
    public class ProductImageProfile: Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImage, ProductImageDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductVariantId, opt => opt.MapFrom(src => src.ProductVariantId))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.IsFeatured))
                .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => src.AltText));


            // Map from ProductImageDTO to ProductImage(for creating)
                CreateMap<ProductImageDTO, ProductImage>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id for creation
                    .ForMember(dest => dest.Url, opt => opt.Ignore()) // Will be set after upload
                    .ForMember(dest => dest.ProductVariantId, opt => opt.MapFrom(src => src.ProductVariantId))
                    .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.IsFeatured))
                    .ForMember(dest => dest.AltText, opt => opt.MapFrom(src => src.AltText))
                    .ForMember(dest => dest.ProductVariant, opt => opt.Ignore()); // Ignore navigation property
        }
    }
}
