using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using static Iruka.Controllers.MobileController;
using static Iruka.DAL.TransactionDal;

namespace Iruka.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MobileUserDto, ApplicationUser>()
                .ForMember(c => c.TrainingStartDate, opt => opt.Ignore());
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<EventDTO, Event>();
            CreateMap<Event, EventDTO>();

            CreateMap<Coupon, CouponDto>().ReverseMap()
                .ForMember(c => c.CreatedBy, opt => opt.Ignore())
                .ForMember(c => c.CreatedDate, opt => opt.Ignore());

            CreateMap<Transaction, TransactionDto>().ReverseMap()
                .ForMember(c => c.Coupon, opt => opt.Ignore())
                .ForMember(c => c.CreatedBy, opt => opt.Ignore())
                .ForMember(c => c.CreatedDate, opt => opt.Ignore());

            CreateMap<ApplicationUser, CustomerDataDto>();
            CreateMap<ApplicationUser, MobileUserViewModel>();
            CreateMap<Branch, BranchDto>();
        }
    }
}