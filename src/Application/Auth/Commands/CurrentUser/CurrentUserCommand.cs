using Application.Common.ViewModels;
using MediatR;

namespace Application.Auth.Commands.CurrentUser
{
    public class CurrentUserCommand : IRequest<AuthUserVm>
    {
    }
}
