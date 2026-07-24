namespace Blog_Project.Application.Mappings
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<LikeDetailsDto, Like>();
            CreateMap<Like, LikeDetailsDto>();

        }
    }
}
