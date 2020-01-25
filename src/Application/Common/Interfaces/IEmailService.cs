using Application.Email.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailDto message);
    }
}
