﻿using System.Threading.Tasks;
using Application.Tags.Queries.TopTags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    public class TagController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetTopTags() =>
            Ok(await Mediator.Send(new TopTagsQuery()));
    }
}
