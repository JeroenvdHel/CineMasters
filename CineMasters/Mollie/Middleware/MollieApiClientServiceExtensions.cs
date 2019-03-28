using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie.Middleware
{
    public static class MollieApiClientServiceExtensions
    {
        public static IServiceCollection AddMollieApi(this IServiceCollection services, string apiKey)
        {
            services.AddScoped<IPaymentClient, PaymentClient>();
            services.AddScoped<IPaymentClient, PaymentClient>(x => new PaymentClient(apiKey));

            return services;
        }
    }
}
