using CineMasters.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.ViewModels
{
    public class PaymentOverviewModel
    {
        public Show Show { get; set; }
        public List<Seat> SelectedSeats { get; set; }
        public string Url { get; set; }

        public PaymentOverviewModel(Show show, List<Seat> seats, string url)
        {
            Show = show;
            SelectedSeats = seats;
            Url = url;
        }
    }
}
