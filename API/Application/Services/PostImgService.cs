using API.Domain.Constants;
using API.Domain.Models;
using Infrastructure.UnitOfWork;
using static System.Net.Mime.MediaTypeNames;

namespace API.Application.Services
{
    public class PostImgService : IPostImgService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PostImgService(IUnitOfWork unitOfWork, IFileStorageService fileStorageService,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }
        public async Task<string> UploadPostImageAsync(int userId, PostImageDto imageDto)
        {
            {
                var post = await _unitOfWork.Repository<Post, int>()
                    .GetByIdAsync(imageDto.PostId);

                if (post == null)
                    throw new Exception(ErrorMessages.PostNotFound);

                if (post.AuthorId != userId)
                    throw new Exception(ErrorMessages.UnauthorizedPostUpdate);

                if (imageDto.ImageFile == null || imageDto.ImageFile.Length == 0)
                    throw new Exception(ErrorMessages.InvalidImg);

               var imgpath = await _fileStorageService.SaveFileAsync(imageDto.ImageFile,"posts");

                var postImage = new PostImage
                {
                    PostId = imageDto.PostId,
                    ImageUrl = imgpath
                };

                await _unitOfWork.Repository<PostImage, int>()
                    .AddAsync(postImage);

                await _unitOfWork.SaveChangesAsync();

                return postImage.ImageUrl;
            }
        }
        public async Task<List<GetPostImageDto>> GetPostImagesAsync(int postId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .GetByIdAsync(postId);

            if (post == null)
                throw new Exception(ErrorMessages.PostNotFound);

            var images = await _unitOfWork.Repository<PostImage, int>()
                .FindAsync(i => i.PostId == postId);

            return _mapper.Map<List<GetPostImageDto>>(images);
        }

        public async Task DeletePostImageAsync(int imageId, int userId)
        {
            var image = await _unitOfWork.Repository<PostImage, int>()
                .GetByIdAsync(imageId, includes: p => p.Post);

            if (image == null)
                throw new Exception(ErrorMessages.InvalidImg);

            if (image.Post.AuthorId != userId)
                throw new Exception(ErrorMessages.UnauthorizedPostUpdate);

            _fileStorageService.DeleteFile(image.ImageUrl);

             _unitOfWork.Repository<PostImage, int>().Remove(image);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
