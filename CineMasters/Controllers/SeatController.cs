using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using CineMasters.Models.Helpers;
using CineMasters.Models.ViewModels;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Newtonsoft.Json;

namespace CineMasters.Controllers
{
    public class SeatController : Controller
    {
        private readonly IShowRepository _showRepo;

        public SeatController(IShowRepository showRepo)
        {
            _showRepo = showRepo;
        }

//        public IActionResult Index()
//        {
//            return View();
//        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectSeats([FromForm] Checkout checkout)
        {
            Show show = _showRepo.GetShow(checkout.ShowId).Result;
            checkout.Show = show;
            checkout.AddTicketsToList();

            //Store checkout in session
            HttpContext.Session.SetObjectAsJson("checkout", checkout);
            
            RoomViewModel model = new RoomViewModel
            {
                Checkout = checkout
            };
            model.InitializeRoomSeats();
            return View("SelectSeats", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveSelectedSeats(IFormCollection formCollection)
        {
            //Get checkout from session
            Checkout checkout = HttpContext.Session.GetObjectFromJson<Checkout>("checkout");

            List<Seat> selectedSeats = new List<Seat>();
            int counter = 0;
            foreach (var item in formCollection)
            {
                if (counter < (formCollection.Count - 1))
                {
                    char c = item.Key[0];
                    int i = Convert.ToInt32(item.Key.Substring(2));

                    if (formCollection[item.Key] == "Geselecteerd")
                    {
                        Seat s = new Seat
                        {
                            RowLetter = c,
                            SeatNumber = i,
                            Status = SeatStatus.Reserved
                        };
                        selectedSeats.Add(s);
                    }
                    checkout.Tickets.ElementAt(counter).Seat = item.Key;
                    checkout.Tickets.ElementAt(counter).TicketCode =
                        Int64.Parse(checkout.CheckoutCode.ToString()+counter.ToString());
                }
                counter++;
            }

            checkout.SelectedSeats = selectedSeats;
            HttpContext.Session.SetObjectAsJson("checkout", checkout);

            //bool seatsReservedInDb = SaveSeats(checkout);

            //bool paymentResult = PaymentSucceeded(true);

            string paymentResponse = MakePayment(checkout).Result;
            //Store Tickets in db

            PaymentOverviewModel model = new PaymentOverviewModel(checkout, paymentResponse);

            return View("PaymentOverview", model);
        }

        public bool SaveSeats(Checkout checkout)
        {
            // Check of OccupiedSeats is an empty List<Seat>
            if (checkout.Show.OccupiedSeats == null)
            {
                // If empty, fill it with seatList parameter
                checkout.Show.OccupiedSeats = checkout.SelectedSeats;
            }
            else
            {
                // If not empty, check if there are same seats in both Lists.
                foreach (var seat in checkout.SelectedSeats)
                {
                    Seat seatToUpdate = checkout.Show.OccupiedSeats.FirstOrDefault(s =>
                        s.RowLetter == seat.RowLetter && 
                        s.SeatNumber == seat.SeatNumber
                        );
                    // If seats exists in OccupiedSeats, update SeatStatus to Status from seatList
                    if (seatToUpdate != null)
                    {
                        seatToUpdate.Status = seat.Status;
                    }
                    // If seats does not exists, add it to OccupiedSeats
                    else
                    {
                        checkout.Show.OccupiedSeats.Add(seat);
                    }
                }
            }
            // Update database
            return _showRepo.UpdateShow(checkout.Show).Result;
        }

        private bool PaymentSucceeded(bool b)
        {
            return b;
        }

        private List<Seat> ChangeSeatStatus(List<Seat> seatList, SeatStatus newStatus)
        {
            List<Seat> result = new List<Seat>();
            foreach (var seat in seatList)
            {
                result.Add(new Seat
                {
                    RowLetter = seat.RowLetter,
                    SeatNumber = seat.SeatNumber,
                    Status = newStatus

                });
            }
            return result;
        }

        public async Task<string> MakePayment(Checkout checkout)
        {
            string totalPrice = checkout.GetTotalPrice().ToString();

            IPaymentClient paymentClient = new PaymentClient("test_spzpNiFENpG6uUWDMnuJxvJwwsh2M8");

            PaymentRequest paymentRequest = new PaymentRequest()
            {
                Amount = new Amount(Currency.EUR, totalPrice),
                Description = checkout.Show.InternalId,
                RedirectUrl = "https://localhost:5001/",
                WebhookUrl = "https://c88aab43.ngrok.io/checkout/test/"
                //+ checkout.CheckoutCode
            };
            PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
            checkout.PaymentId = paymentResponse.Id;

            //Store Tickets in db
            //return paymentResponse;
            return paymentResponse.Links.Checkout.Href;
        }
    }
}