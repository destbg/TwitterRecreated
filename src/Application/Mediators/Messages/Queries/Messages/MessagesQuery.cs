using System;
using System.Collections.Generic;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Messages.Queries.Messages
{
    public class MessagesQuery : IRequest<IEnumerable<MessageVm>>
    {
        public long ChatId { get; set; }
        public DateTime Skip { get; set; }
    }
}
