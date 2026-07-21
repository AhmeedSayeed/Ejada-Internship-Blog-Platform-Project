using Blog_Project.Domain.Constants;
using Blog_Project.Domain.Models;
using Infrastructure.UnitOfWork;
using static System.Net.Mime.MediaTypeNames;

namespace Blog_Project.Application.Services
{
    public class PostImgService : IPostImgService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly LocalFileStorageService _fileStorageService;
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
    }
}
