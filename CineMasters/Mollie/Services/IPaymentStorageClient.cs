using CineMasters.Models.Mollie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie.Services
{
    public interface IPaymentStorageClient
    {
        Task Create(CreatePaymentModel model);
    }
}
