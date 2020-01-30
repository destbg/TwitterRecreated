using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Auth.Commands.CurrentUser
{
    public class CurrentUserHandler : IRequestHandler<CurrentUserCommand, AuthUserVm>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public CurrentUserHandler(ICurrentUserService currentUser, IMapper mapper)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<AuthUserVm> Handle(CurrentUserCommand request, CancellationToken cancellationToken) =>
            Task.FromResult(_mapper.Map<AuthUserVm>(_currentUser.User));
    }
}
