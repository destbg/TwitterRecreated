using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Common
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly IMapper _mapper;

        public NotificationRepository(IMapper mapper, ITwitterDbContext context) : base(context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<List<NotificationVm>> UserNotifications(string userId, DateTime skip, CancellationToken token) =>
            _context.Notifications
                .Where(f => f.UserId == userId && f.CreatedOn > skip)
                .ProjectTo<NotificationVm>(_mapper.ConfigurationProvider)
                .ToListAsync(token);
    }
}
