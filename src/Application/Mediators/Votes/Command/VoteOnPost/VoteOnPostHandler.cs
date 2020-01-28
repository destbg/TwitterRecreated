using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Votes.Command.VoteOnPost
{
    public class VoteOnPostHandler : IRequestHandler<VoteOnPostCommand>
    {
        private readonly IPollOptionRepository _pollOption;
        private readonly IPollVoteRepository _pollVote;
        private readonly ICurrentUserService _currentUser;

        public VoteOnPostHandler(IPollOptionRepository pollOption, IPollVoteRepository pollVote, ICurrentUserService currentUser)
        {
            _pollOption = pollOption ?? throw new ArgumentNullException(nameof(pollOption));
            _pollVote = pollVote ?? throw new ArgumentNullException(nameof(pollVote));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<Unit> Handle(VoteOnPostCommand request, CancellationToken cancellationToken)
        {
            _ = await _pollOption.GetById(request.OptionId, cancellationToken)
                ?? throw new NotFoundException("Option Id", request.OptionId);
            var vote = await _pollVote.GetByUserAndOption(_currentUser.UserId, request.OptionId, cancellationToken);
            if (vote != null)
                throw new BadRequestException("You have already voted on that poll");
            await _pollVote.Create(new PollVote
            {
                OptionId = request.OptionId,
                UserId = _currentUser.UserId
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
