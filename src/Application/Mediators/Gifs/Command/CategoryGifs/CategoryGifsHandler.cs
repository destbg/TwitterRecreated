using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Gifs.Command.CategoryGifs
{
    public class CategoryGifsHandler : IRequestHandler<CategoryGifsCommand, IEnumerable<GifVm>>
    {
        private readonly IConfiguration _configuration;

        public CategoryGifsHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<GifVm>> Handle(CategoryGifsCommand request, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var responce = await client.GetStringAsync($"https://api.tenor.com/v1/search?key={_configuration.GetValue<string>("TenorKey")}&q={request.Tag}&limit=50");
            var data = JsonConvert.DeserializeObject<GifDownloadSearchJson>(responce);
            return data.Results.Select(f =>
                new GifVm
                {
                    Gif = f.Media[0].Gif.Url,
                    TinyGif = f.Media[0].TinyGif.Url
                });
        }
    }
}
