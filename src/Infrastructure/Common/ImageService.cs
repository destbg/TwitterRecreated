using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
        }

        public async Task<string[]> SaveImages(List<IFormFile> images, string type = "png")
        {
            var names = new string[images.Count];
            for (var i = 0; i < images.Count; i++)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(images[i].FileName, images[i].OpenReadStream()),
                    PublicId = Guid.NewGuid().ToString("N"),
                    Format = type
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                names[i] = uploadResult.SecureUri.ToString();
            }
            return names;
        }

        public async Task<string> SaveImage(IFormFile image, string type = "png")
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString("N"),
                Format = type
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUri.ToString();
        }
    }
}
