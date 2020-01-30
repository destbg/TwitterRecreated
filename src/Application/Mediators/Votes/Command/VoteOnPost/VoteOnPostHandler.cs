using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Repositories;
using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Votes.Command.VoteOnPost
{
    public class VoteOnPostHandler : IRequestHandler<VoteOnPostCommand>
    {
        private readonly IPollOptionRepository _pollOption;
        private readonly IPollVoteRepository _pollVote;
        private readonly ICurrentUserService _currentUser;
        private readonly IMainHubService _mainHub;
        private readonly IMapper _mapper;

        public VoteOnPostHandler(IPollOptionRepository pollOption, IPollVoteRepository pollVote, ICurrentUserService currentUser, IMainHubService mainHub, IMapper mapper)
        {
            _pollOption = pollOption ?? throw new ArgumentNullException(nameof(pollOption));
            _pollVote = pollVote ?? throw new ArgumentNullException(nameof(pollVote));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mainHub = mainHub ?? throw new ArgumentNullException(nameof(mainHub));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(VoteOnPostCommand request, CancellationToken cancellationToken)
        {
            var option = await _pollOption.GetById(request.OptionId, cancellationToken)
                ?? throw new NotFoundException("Option Id", request.OptionId);

            var vote = await _pollVote.GetByUserAndOption(_currentUser.User.Id, request.OptionId, cancellationToken);
            if (vote != null)
                throw new BadRequestException("You have already voted on that poll");

            vote = new PollVote
            {
                Option = option,
                User = _currentUser.User
            };
            await _pollVote.Create(vote, cancellationToken);
            await _mainHub.SendPollVote(_mapper.Map<PollVoteVm>(vote));

            return Unit.Value;
        }
    }
}
