using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.CurrentUser
{
    public class CurrentUserHandler : IRequestHandler<CurrentUserCommand, AuthUserVm>
    {
        private readonly IUserManager _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public CurrentUserHandler(IUserManager userManager, ICurrentUserService currentUser, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AuthUserVm> Handle(CurrentUserCommand request, CancellationToken cancellationToken)
        {
            var (Result, User) = await _userManager.GetUserById(_currentUser.UserId);

            if (!Result.Succeeded)
                throw new NotFoundException("User Id", _currentUser.UserId);

            return _mapper.Map<AuthUserVm>(User);
        }
    }
}
