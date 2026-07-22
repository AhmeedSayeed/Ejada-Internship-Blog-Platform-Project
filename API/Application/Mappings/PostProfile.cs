using AutoMapper;
using API.Application.DTOs;
using API.Domain.Models;

namespace API.Application.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
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
        .ForCtorParam("LastUpdatedAt",
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

        CreateMap<PostImage, GetPostImageDto>();
        CreateMap<PostImage, PostImageDto>();

    }
}
