using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Common.Validators
{
    public static class HandlerValidators
    {
        public static string VideoLink(string content)
        {
            var split = content.Split();
            var regex = new Regex(@"^(?:https?:\/\/)?(?:www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$");
            var video = Array.Find(split, f => regex.IsMatch(f));

            if (video == null || !Uri.TryCreate(video, UriKind.RelativeOrAbsolute, out var uri))
                return null;

            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = query.AllKeys.Contains("v") ? query["v"] : uri.Segments.Last();

            return string.IsNullOrEmpty(videoId) ? null : videoId;
        }

        public static PostType GetFileTypes(List<IFormFile> files) =>
            files.Exists(f => f.ContentType.StartsWith("video"))
                ? PostType.Video
                : files.Exists(f => f.ContentType == "image/gif") ? PostType.Gif : PostType.Images;
    }
}
