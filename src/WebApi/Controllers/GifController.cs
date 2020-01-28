using System.Threading.Tasks;
using Application.Gifs.Command.CategoryGifs;
using Application.Gifs.Command.GifCategories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GifController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GifCategories() =>
            Ok(await Mediator.Send(new GifCategoriesCommand()));

        [HttpPost]
        public async Task<IActionResult> CategoryGifs(CategoryGifsCommand command) =>
            Ok(await Mediator.Send(command));
    }
}
