using Application.Common.Interfaces;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(EmailDto message) => Task.CompletedTask;
    }
}
