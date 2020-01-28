using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Messages.Queries.Messages
{
    public class MessagesHandler : IRequestHandler<MessagesQuery, IEnumerable<MessageVm>>
    {
        private readonly IMessageRepository _message;

        public MessagesHandler(IMessageRepository message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public async Task<IEnumerable<MessageVm>> Handle(MessagesQuery request, CancellationToken cancellationToken) =>
            await _message.ChatMessages(request.ChatId, request.Skip, cancellationToken);
    }
}
