using AutoMapper;
using CineMasters.Models.Mollie.Models;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie.Automapper
{
    public class MollieApiProfile : Profile
    {
        public MollieApiProfile()
        {
            this.CreateMap<CreatePaymentModel, PaymentRequest>()
                .ForMember(x => x.Amount, m => m.MapFrom(x => new Amount(x.Currency, x.Amount.ToString(CultureInfo.InvariantCulture))));
        }
    }
}
