using bot.Models;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Options;

namespace bot.Services
{
    public class MercadoPagoService
    {
        private readonly string _accessToken;

        public MercadoPagoService(IOptions<PaymentGatewaySettings> options)
        {
            _accessToken = options.Value.AcessToken;
            MercadoPago.Config.MercadoPagoConfig.AccessToken = _accessToken;
        }

        public async Task<Payment> CreatePayment(decimal amount, string externalRerefence, string payerEmail = "jonhdoe@email.com", string description = "Software Payment")
        {
            ArgumentNullException.ThrowIfNull(description);
            var client = new PaymentClient();

            var request = new PaymentCreateRequest
            {
                TransactionAmount = amount,
                ExternalReference = externalRerefence,
                PaymentMethodId = "pix",
                Installments = 1,
                Description = description,
                Payer = new PaymentPayerRequest
                {
                    Email = payerEmail
                },
            };
            Payment payment = await client.CreateAsync(request);
            Console.WriteLine(payment.Id);
            return payment;
        }

        public async Task<Payment> GetPayment(long paymentId)
        {
            var client = new PaymentClient();
            Payment payment = await client.GetAsync(paymentId);
            return payment;
        }
    }
}
