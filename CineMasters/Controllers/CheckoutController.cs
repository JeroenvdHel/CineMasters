using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CineMasters.Controllers
{
    public class CheckoutController : Controller
    {
        [HttpGet]
        public IActionResult Index(long showId)
        {
            Checkout checkout = new Checkout
            {
                ShowId = showId
            };

            return View("Index", checkout);
        }
    }
}