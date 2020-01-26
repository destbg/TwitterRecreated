using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IVideoService
    {
        Task<string> CreateVideo(IFormFile video, string location = "wwwroot/post/");
    }
}
