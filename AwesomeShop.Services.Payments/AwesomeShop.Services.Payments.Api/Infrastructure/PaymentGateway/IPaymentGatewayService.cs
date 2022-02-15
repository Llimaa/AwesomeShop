using System.Threading.Tasks;
using AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway.Dtos;

namespace AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway
{
    public interface IPaymentGatewayService
    {
        Task<bool> Process(CreditCardInfo info);
    }
}