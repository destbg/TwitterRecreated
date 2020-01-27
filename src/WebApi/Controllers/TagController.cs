using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class TagController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetTopTags() =>
            Ok(Array.Empty<object>());
    }
}
