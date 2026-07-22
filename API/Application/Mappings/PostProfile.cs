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
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore());


        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name));

        CreateMap<Post, PostDetailsDto>();
    }
}
