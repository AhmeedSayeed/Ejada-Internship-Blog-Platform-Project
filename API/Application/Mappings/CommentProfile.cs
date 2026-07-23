using API.Application.DTOs;
using API.Domain.Models;
using AutoMapper;

namespace API.Application.Mapping
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDto, Comment>();

            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<Comment, CommentDetailsDto>()
                .ForCtorParam("Id",
                    opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Content",
                    opt => opt.MapFrom(src => src.Content))
                .ForCtorParam("PostId",
                    opt => opt.MapFrom(src => src.PostId))
                .ForCtorParam("UserId",
                    opt => opt.MapFrom(src => src.UserId))
                .ForCtorParam("UserName",
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForCtorParam("UserProfileImageUrl",
                    opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForCtorParam("ParentCommentId",
                    opt => opt.MapFrom(src => src.ParentCommentId))
                .ForCtorParam("CreatedAt",
                    opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}