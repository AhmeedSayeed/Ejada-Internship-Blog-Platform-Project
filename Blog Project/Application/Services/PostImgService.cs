using Blog_Project.Domain.Constants;
using Blog_Project.Domain.Models;
using Infrastructure.UnitOfWork;
using static System.Net.Mime.MediaTypeNames;

namespace Blog_Project.Application.Services
{
    public class PostImgService : IPostImgService
    {
       
        private readonly IUnitOfWork _unitOfWork;
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
                    throw new Exception("Invalid image.");

                var extension = Path.GetExtension(imageDto.ImageFile.FileName);

                var fileName = $"{Guid.NewGuid()}{extension}";

                var folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "posts");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageDto.ImageFile.CopyToAsync(stream);
                }

                var postImage = new PostImage
                {
                    PostId = imageDto.PostId,
                    ImageUrl = $"/images/posts/{fileName}"
                };

                await _unitOfWork.Repository<PostImage, int>()
                    .AddAsync(postImage);

                await _unitOfWork.SaveChangesAsync();

                return postImage.ImageUrl;
            }
        }
    }
}
