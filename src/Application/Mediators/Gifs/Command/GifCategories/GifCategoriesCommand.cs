using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Gifs.Command.GifCategories
{
    public class GifCategoriesCommand : IRequest<IEnumerable<GifCategoryVm>>
    {
    }
}
