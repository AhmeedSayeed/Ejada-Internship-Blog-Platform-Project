using AutoMapper;
using API.Application.DTOs;
using API.Domain.Models;

namespace API.Application.Mappings
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
            // DTO -> Entity
            CreateMap<CreatePostDto, Post>();


            CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId));

            // Entity -> DTO
            CreateMap<Post, PostDto>()
            .ForCtorParam("Author",
                opt => opt.MapFrom(src => src.Author.UserName))
            .ForCtorParam("Content",
                opt => opt.MapFrom(src => src.Content))
            .ForCtorParam("Category",
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForCtorParam("LastUpdateAt",
                opt => opt.MapFrom(src => src.LastUpdatedAt));

            CreateMap<Post, PostDetailsDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))

                .ForMember(dest => dest.AuthorUserName,
                    opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorEmail,
                    opt => opt.MapFrom(src => src.Author.Email))
                .ForMember(dest => dest.AuthorProfileImageUrl,
                    opt => opt.MapFrom(src => src.Author.ProfileImageUrl))

                .ForMember(dest => dest.ReviewedByAdminUserName,
                    opt => opt.MapFrom(src => src.ReviewedByAdmin != null ? src.ReviewedByAdmin.UserName : null))
                .ForMember(dest => dest.ReviewedByAdminEmail,
                    opt => opt.MapFrom(src => src.ReviewedByAdmin != null ? src.ReviewedByAdmin.Email : null))
                .ForMember(dest => dest.ReviewedByAdminProfileImageUrl,
                    opt => opt.MapFrom(src => src.ReviewedByAdmin != null ? src.ReviewedByAdmin.ProfileImageUrl : null));


                
        }
    }
}
