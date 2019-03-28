using CineMasters.Models.Mollie.Models;
using Mollie.Api.Models.Payment.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie.Services
{
    public interface IPaymentOverviewClient
    {
        Task<OverviewModel<PaymentResponse>> GetList();
        Task<OverviewModel<PaymentResponse>> GetListByUrl(string url);
    }
}
