using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<NotificationVm>> UserNotifications(string userId, DateTime skip, CancellationToken token);
    }
}
