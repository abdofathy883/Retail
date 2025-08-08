using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CheckOutService
    {
        private readonly IPaymentFactory paymentFactory;
        public CheckOutService(IPaymentFactory payment)
        {
            paymentFactory = payment;
        }

        //public async Task HandlePayment(Order order)
        //{
        //    var provider = order.PaymentProvider;
        //    var gateway = paymentFactory.GetGetway(provider);

        //    //.......
        //}
    }
}
