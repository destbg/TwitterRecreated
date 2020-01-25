using Application.Posts.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PostController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetById(long id) =>
            Ok(await Mediator.Send(new GetByIdQuery { Id = id }));
    }
}
