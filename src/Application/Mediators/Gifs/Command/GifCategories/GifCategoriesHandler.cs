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

namespace Application.Gifs.Command.GifCategories
{
    public class GifCategoriesHandler : IRequestHandler<GifCategoriesCommand, IEnumerable<GifCategoryVm>>
    {
        private readonly IConfiguration _configuration;

        public GifCategoriesHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<IEnumerable<GifCategoryVm>> Handle(GifCategoriesCommand request, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            var result = await client.GetStringAsync($"https://api.tenor.com/v1/categories?key={_configuration.GetValue<string>("TenorKey")}");
            var data = JsonConvert.DeserializeObject<GifDownloadCategoryJson>(result);
            return data.Tags.Select(f => new GifCategoryVm { Image = f.Image, Tag = f.Searchterm });
        }
    }
}
