namespace AwesomeShop.Services.Payments.Api.Infrastructure.PaymentGateway.Dtos
{
    public class CreditCardInfo
    {
        public CreditCardInfo(string cardNumber, string fullName, string expirationDate, string cvv)
        {
            this.CardNumber = cardNumber;
            this.FullName = fullName;
            this.ExpirationDate = expirationDate;
            this.Cvv = cvv;

        }
        public string CardNumber { get; set; }
        public string FullName { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
    }
}