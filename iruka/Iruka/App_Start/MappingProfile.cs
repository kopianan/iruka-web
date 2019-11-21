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
            Mapper.CreateMap<Product, ProductDTO>();
            Mapper.CreateMap<ProductDTO, Product>();
            Mapper.CreateMap<EventDTO, Event>();
            Mapper.CreateMap<Event, EventDTO>();
        }
    }
}