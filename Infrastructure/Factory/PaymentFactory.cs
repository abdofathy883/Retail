using Core.Enums;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IServiceProvider serviceProvider;
        public PaymentFactory(IServiceProvider service)
        {
            serviceProvider = service;
        }
        public IPaymentService GetGetway(PaymentProvider paymentProvider)
        {
            return paymentProvider switch
            {
                PaymentProvider.Fawry => serviceProvider.GetRequiredService<FawryService>(),
                PaymentProvider.Paymob => serviceProvider.GetRequiredService<PaymobService>(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
