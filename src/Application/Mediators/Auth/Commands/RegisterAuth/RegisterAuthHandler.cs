using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.ViewModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.RegisterAuth
{
    public class RegisterAuthHandler : IRequestHandler<RegisterAuthCommand, AuthVm>
    {
        private readonly IUserManager _manager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUser;

        public RegisterAuthHandler(IUserManager manager, IConfiguration configuration, ICurrentUserService currentUser)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<AuthVm> Handle(RegisterAuthCommand request, CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            {
                var recaptcha = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_configuration.GetValue<string>("RecaptchaKey")}&response={request.Recaptcha}&remoteip=${_currentUser.Ip}");
                var data = JsonConvert.DeserializeObject<RecaptchaJson>(await recaptcha.Content.ReadAsStringAsync());

                if (!data.Success)
                    return default;
            }

            var (Result, Auth) = await _manager.CreateUserAsync(request.Username, request.Email, request.Password);
            return Result.Succeeded ? Auth : default;
        }
    }
}
