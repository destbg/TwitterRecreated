using Application.Common.Models;
using Application.Common.ViewModels;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<(Result Result, AuthVm Auth)> LoginUserAsync(string username, string password);
        Task<(Result Result, AuthVm Auth)> CreateUserAsync(string userName, string email, string password);
        Task<Result> DeleteUserAsync(string userId);
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetCurrentUser(string username);
        Task<Result> UpdateUser(AppUser user);
        Task<List<UserFollowVm>> SeachUsers(string search);
        Task<List<AppUser>> ValidateUsersnames(IEnumerable<string> usernames);
        string NormalizeName(string name);
    }
}
