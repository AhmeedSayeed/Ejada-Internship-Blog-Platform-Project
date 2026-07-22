using API.Domain.Constants;
using API.Domain.Models;
using Infrastructure.UnitOfWork;

namespace API.Application.Interfaces
{
    public interface IPostImgService
    {
        Task<string> UploadPostImageAsync(int userID, PostImageDto imageDto);
       
    }
}
