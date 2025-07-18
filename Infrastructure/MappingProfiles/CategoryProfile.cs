using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfiles
{
    internal class CategoryProfile: Profile
    {
        public CategoryProfile()
        {
            CreateMap<Core.Models.Category, Core.DTOs.CategoryDTO>();
        }
    }
}
