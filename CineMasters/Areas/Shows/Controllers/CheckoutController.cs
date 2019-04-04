using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Shows.Models;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace CineMasters.Controllers
{
    [Area("Shows")]
    public class CheckoutController : Controller
    {
        private readonly ITicketRepository _ticketRepo;

        public CheckoutController(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        [HttpGet]
        public IActionResult Index(long showId)
        {
            Checkout checkout = new Checkout
            {
                ShowId = showId,
                CheckoutCode = _getRandomCode()
            };

            return View("Index", checkout);
        }

        //[HttpGet]
        //public IActionResult Test() {

        //    int i = 0; ;

        //    return new ObjectResult("Geslaagd");
        //}


        private int _getRandomCode()
        {
            // Generate random code to print ticket at cinama portal
            // Returns integer between 0 and a billion
            Random random = new System.Random();
            int code = random.Next(0, 1000000000);
            return code;
        }
    }
}