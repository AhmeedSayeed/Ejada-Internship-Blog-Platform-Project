namespace Blog_Project.Application.Mappings
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<RatingDetailsDto, Rating>();

            CreateMap<Rating, RatingDetailsDto>()
                .ForCtorParam(nameof(RatingDetailsDto.UserName),
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForCtorParam(nameof(RatingDetailsDto.UserProfileImageUrl),
                    opt => opt.MapFrom(src => src.User.ProfileImageUrl));
        }
    }
}
