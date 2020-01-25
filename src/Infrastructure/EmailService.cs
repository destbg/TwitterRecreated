using Application.Common.Interfaces;
using Application.Email.Models;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(EmailDto message) => Task.CompletedTask;
    }
}
