using AutoMapper;
using Blog_Project.Application.DTOs;
using Blog_Project.Domain.Models;

namespace Blog_Project.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<RegisterRequestDto, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<ApplicationUser, UserSummaryDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ApplicationUser, MyProfileResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts));

            CreateMap<ApplicationUser, PublicProfileResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts));

            CreateMap<UpdateProfileDto, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
