using Mollie.Api.Models.Payment.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Areas.Shows.Models.ViewModels
{
    public class PaymentOverviewModel
    {
        public Checkout Checkout { get; set; }
        public string Url { get; set; }

        public PaymentOverviewModel(Checkout checkout, string url)
        {
            Checkout = checkout;
            Url = url;
        }
    }
}
