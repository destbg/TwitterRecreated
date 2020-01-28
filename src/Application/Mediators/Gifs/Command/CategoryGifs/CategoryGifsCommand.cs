using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Gifs.Command.CategoryGifs
{
    public class CategoryGifsCommand : IRequest<IEnumerable<GifVm>>
    {
        public string Tag { get; set; }
    }
}
