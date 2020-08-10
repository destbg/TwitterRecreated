using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.ViewModels;
using MediatR;

namespace Application.Auth.Commands.MobileLogin
{
    public class MobileLoginHandler : IRequestHandler<MobileLoginCommand, AuthVm>
    {
        private readonly IUserManager _manager;
        private readonly ICurrentUserService _currentUser;

        public MobileLoginHandler(IUserManager manager, ICurrentUserService currentUser)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<AuthVm> Handle(MobileLoginCommand request, CancellationToken cancellationToken)
        {
            var (Result, Auth) = await _manager.LoginUserAsync(request.Username, request.Password);
            return Result.Succeeded ? Auth : default;
        }
    }
}
