using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;

namespace Iruka.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<EventDTO, Event>();
            CreateMap<Event, EventDTO>();

            CreateMap<Coupon, CouponDto>().ReverseMap()
                .ForMember(c => c.CreatedBy, opt => opt.Ignore())
                .ForMember(c => c.CreatedDate, opt => opt.Ignore());
        }
    }
}