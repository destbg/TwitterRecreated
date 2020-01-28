using Application.Common.Interfaces;
using Application.Common.ViewModels;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, UserVm>
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GetUserHandler(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper;
        }

        public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserByUsername(request.Username);
            return _mapper.Map<UserVm>(user);
        }
    }
}
