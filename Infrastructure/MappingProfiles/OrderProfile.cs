using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace Infrastructure.MappingProfiles
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
        }
    }
}
