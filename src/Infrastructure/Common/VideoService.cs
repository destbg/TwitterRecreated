using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common
{
    public class VideoService : IVideoService
    {
        private readonly Cloudinary _cloudinary;

        public VideoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
        }

        public async Task<string> CreateVideo(IFormFile video)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(video.FileName, video.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString("N"),
                Format = video.ContentType.Split('/')[1]
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUri.ToString();
        }
    }
}
