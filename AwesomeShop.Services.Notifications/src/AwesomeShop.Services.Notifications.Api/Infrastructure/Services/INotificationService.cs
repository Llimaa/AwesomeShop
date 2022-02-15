using System.Threading.Tasks;

namespace AwesomeShop.Services.Notifications.Api.Infrastructure.Services
{
    public interface INotificationService
    {
        Task SendAsync(string subject, string content, string toEmail, string toName);
    }
}