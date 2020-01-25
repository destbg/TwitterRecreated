using Application.Common.Interfaces;
using Application.Common.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.LoginAuth
{
    public class LoginAuthHandler : IRequestHandler<LoginAuthCommand, AuthVm>
    {
        private readonly IUserManager _manager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUser;

        public LoginAuthHandler(IUserManager manager, IConfiguration configuration)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<AuthVm> Handle(LoginAuthCommand request, CancellationToken cancellationToken)
        {
            //using (var client = new HttpClient())
            //{
            //    var recaptcha = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_configuration.GetValue<string>("RecaptchaKey")}&response={request.Recaptcha}&remoteip=${_currentUser.Ip}");
            //    var data = JsonConvert.DeserializeObject<Recaptcha>(await recaptcha.Content.ReadAsStringAsync());

            //    if (!data.Success)
            //        return default;
            //}

            var (Result, Auth) = await _manager.LoginUserAsync(request.Username, request.Password);
            return Result.Succeeded ? Auth : default;
        }
    }
}
