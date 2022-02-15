using System.Threading.Tasks;
using AwesomeShop.Services.Notifications.Api.Infrastructure.Dtos;

namespace AwesomeShop.Services.Notifications.Api.Infrastructure.Persistence.Repositories
{
    public interface IMailRepository
    {
        Task<EmailTemplateDto> GetTemplate(string @event);
    }
}