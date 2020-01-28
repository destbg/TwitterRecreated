using System;
using System.Threading.Tasks;
using Application.Notifications.Queries.UserNotifications;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly IDateTime _date;

        public NotificationController(IDateTime date)
        {
            _date = date ?? throw new ArgumentNullException(nameof(date));
        }

        [HttpGet("{skip?}")]
        public async Task<IActionResult> GetNotifications(DateTime? skip) =>
            Ok(await Mediator.Send(new UserNotificationsQuery { Skip = skip ?? _date.MinDate }));
    }
}
