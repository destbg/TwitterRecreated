using Application.Common.Models;
using Application.Common.ViewModels;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<(Result Result, AuthVm Auth)> LoginUserAsync(string username, string password);
        Task<(Result Result, AuthVm Auth)> CreateUserAsync(string userName, string email, string password);
        Task<Result> DeleteUserAsync(string userId);
        Task<(Result Result, AppUser User)> GetUserByUsername(string username);
        string NormalizeName(string name);
    }
}
