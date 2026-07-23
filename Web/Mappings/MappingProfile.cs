using AutoMapper;
using Contract;
using Web.ViewModel;

namespace Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, RegisterRequestDto>();
            CreateMap<LoginViewModel, LoginRequestDto>();
        }
    }
}