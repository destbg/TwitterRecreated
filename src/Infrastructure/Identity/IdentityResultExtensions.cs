using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result) =>
            result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));

        public static Result ToApplicationResult(this SignInResult result) =>
            result.Succeeded
                ? Result.Success()
                : Result.Failure();
    }
}
