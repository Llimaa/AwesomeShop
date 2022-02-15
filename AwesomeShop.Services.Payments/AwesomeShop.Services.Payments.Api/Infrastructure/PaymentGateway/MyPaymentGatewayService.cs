using System.Threading.Tasks;
using AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway.Dtos;

namespace AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway
{
    public class MyPaymentGatewayService : IPaymentGatewayService
    {
        public Task<bool> Process(CreditCardInfo info)
        {
            return Task.FromResult(true);
        }
    }
}