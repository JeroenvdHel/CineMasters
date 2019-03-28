using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using CineMasters.Models.ViewModels;
using CineMasters.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

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
            checkout.Show = _showRepo.GetShow(checkout.ShowId).Result;
            checkout.AddTicketsToList();

            RoomViewModel model = new RoomViewModel
            {
                Checkout = checkout
            };
            model.InitializeRoomSeats();
            return View("SelectSeats", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveSelectedSeats(RoomViewModel model)
        {
            return new ObjectResult(model);
            //List<Seat> SelectedSeats = new List<Seat>();
            //int counter = 1;
            //foreach (var item in formCollection)
            //{
            //    if (counter < (formCollection.Count - 1))
            //    {
            //        char c = item.Key[0];
            //        int i = Convert.ToInt32(item.Key.Substring(2));
                    
            //        if (formCollection[item.Key] == "Geselecteerd")
            //        {
            //            Seat s = new Seat
            //            {
            //                RowLetter = c,
            //                SeatNumber = i,
            //                Status = SeatStatus.Reserved
            //            };
            //            SelectedSeats.Add(s);
            //        }
            //    }
            //    counter++;
            //}
            //var checkout = formCollection["Checkout"];
            //Show show = _showRepo.GetShow(long.Parse(formCollection["showId"])).Result;
            //decimal totalPrice = Decimal.Parse(formCollection["totalprice"]);

            //bool seatsReservedInDb = SaveSeats(SelectedSeats, show);

            //bool paymentResult = PaymentSucceeded(true);

            //if (paymentResult)
            //{
            //    SelectedSeats = ChangeSeatStatus(SelectedSeats, SeatStatus.Occupied).ToList();
            //}
            //else
            //{
            //    SelectedSeats = ChangeSeatStatus(SelectedSeats, SeatStatus.Free).ToList();
            //}
            //bool seatsOccupiedOrFreedInDb = SaveSeats(SelectedSeats, show);

            //return new ObjectResult(seatsOccupiedOrFreedInDb);

            ////string paymentUrl = MakePayment(show, SelectedSeats).Result;

            ////PaymentOverviewModel model = new PaymentOverviewModel(show, SelectedSeats, paymentUrl);

        }

        public bool ReserveSeats(List<Seat> seats, Show show)
        {
            show.OccupiedSeats.AddRange(seats);

            return _showRepo.UpdateShow(show).Result;
        }

        public bool SaveSeats(List<Seat> seatList, Show show)
        {
            // Check of OccupiedSeats is an empty List<Seat>
            if (show.OccupiedSeats == null)
            {
                // If empty, fill it with seatList parameter
                show.OccupiedSeats = seatList;
            }
            else
            {
                // If not empty, check if there are same seats in both Lists.
                foreach (var seat in seatList)
                {
                    Seat seatToUpdate = show.OccupiedSeats.FirstOrDefault(s =>
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
                        show.OccupiedSeats.Add(seat);
                    }
                }
            }
            // Update database
            return _showRepo.UpdateShow(show).Result;
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
        
        //public async Task<string> MakePayment(Show show, List<Seat> selectedSeats)
        //{
        //    Checkout checkout = new Checkout();

        //    foreach (var seat in selectedSeats)
        //    {
        //        checkout.
        //    }
        //}
    }
}