using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ImageService : IImageService
    {
        public async Task<string[]> SaveImages(List<IFormFile> images, string type = ".png", string location = "wwwroot/post/")
        {
            var names = new string[images.Count];
            for (int i = 0; i < images.Count; i++)
            {
                names[i] = Guid.NewGuid().ToString("N") + type;
                using var file = File.Create(location + names[i]);
                await images[i].CopyToAsync(file);
            }
            return names;
        }

        public async Task<string> SaveImage(IFormFile image, string type = ".png", string location = "wwwroot/post/")
        {
            var name = Guid.NewGuid().ToString("N") + type;
            using var file = File.Create(location + name);
            await image.CopyToAsync(file);
            return name;
        }
    }
}
