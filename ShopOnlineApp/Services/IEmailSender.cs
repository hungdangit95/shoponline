using System.Threading.Tasks;

namespace ShopOnlineApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
