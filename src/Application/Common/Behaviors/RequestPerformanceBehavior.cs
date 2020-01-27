using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            var type = typeof(TResponse);

            var name = !type.IsGenericType ? type.Name
                : $"{type.Name.Replace("`" + type.Name[^1], "")}<{string.Join(", ", type.GenericTypeArguments.Select(f => f.Name))}>";

            _logger.LogInformation("Response [{Name}] ({ElapsedMilliseconds} milliseconds) {@UserId}",
                name, timer.ElapsedMilliseconds, _currentUserService.UserId ?? _currentUserService.Ip);

            return response;
        }
    }
}
