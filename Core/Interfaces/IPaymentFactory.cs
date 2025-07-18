using Core.Enums;

namespace Core.Interfaces
{
    public interface IPaymentFactory
    {
        IPaymentService GetGetway(PaymentProvider paymentProvider);
    }
}
