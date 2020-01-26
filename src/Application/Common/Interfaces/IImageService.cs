using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<string[]> SaveImages(List<IFormFile> images, string type = ".png", string location = "wwwroot/post/");
        Task<string> SaveImage(IFormFile image, string type = ".png", string location = "wwwroot/post/");
    }
}
