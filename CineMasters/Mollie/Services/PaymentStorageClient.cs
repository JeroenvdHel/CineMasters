using AutoMapper;
using CineMasters.Models.Mollie.Models;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CineMasters.Models.Mollie.Services
{
    public class PaymentStorageClient : IPaymentStorageClient
    {
        private readonly IPaymentClient _paymentClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentStorageClient(IPaymentClient paymentClient, IMapper mapper, IConfiguration configuration)
        {
            this._paymentClient = paymentClient;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task Create(CreatePaymentModel model)
        {
            PaymentRequest paymentRequest = this._mapper.Map<PaymentRequest>(model);
            paymentRequest.RedirectUrl = this._configuration.GetSection("Mollie:DefaultRedirectUrl").Value;


            await this._paymentClient.CreatePaymentAsync(paymentRequest);
        }
    }
}
