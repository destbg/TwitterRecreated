using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.ViewModels;
using Domain.Entities;

namespace Application.Common.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<MessageVm>> ChatMessages(long chatId, DateTime skip, CancellationToken token);
    }
}
