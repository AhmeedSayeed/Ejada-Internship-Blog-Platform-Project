using Blog_Project.Domain.Constants;
using Blog_Project.Domain.Models;
using Infrastructure.UnitOfWork;

namespace Blog_Project.Application.Interfaces
{
    public interface IPostImgService
    {
        Task<string> UploadPostImageAsync(int userID, PostImageDto imageDto);
       
    }
}
