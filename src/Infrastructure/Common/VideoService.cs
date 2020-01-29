using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class VideoService : IVideoService
    {
        public async Task<string> CreateVideo(IFormFile video, string location = "wwwroot/post/")
        {
            var name = Guid.NewGuid().ToString("N") + "." + video.ContentType.Split('/')[1];
            using var file = File.Create(location + name);
            await video.CopyToAsync(file);
            return name;
        }
    }
}
