using CineMasters.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.ViewModels
{
    public class RoomViewModel
    {
        public Checkout Checkout { get; set; }
        public List<KeyValuePair<Seat, bool>> OccupiedSeats { get; set; }
        public IDictionary<char, List<KeyValuePair<Seat, bool>>> AllSeats { get; set; }
        public List<Seat> SelectedSeats { get; set; }
        public string Test { get; set; }

        public RoomViewModel()
        {
            OccupiedSeats = new List<KeyValuePair<Seat, bool>>();

            
        }

        public void InitializeRoomSeats() {
            if (Checkout.Show.OccupiedSeats != null)
            {
                foreach (var seat in Checkout.Show.OccupiedSeats)
                {
                    OccupiedSeats.Add(new KeyValuePair<Seat, bool>(seat, false));
                }
            }
            AllSeats = FillSeats(OccupiedSeats);
        }


        private Dictionary<char, List<KeyValuePair<Seat, bool>>> FillSeats(List<KeyValuePair<Seat, bool>> occupiedSeats)
        {
            Dictionary<char, List<KeyValuePair<Seat, bool>>> seatList = new Dictionary<char, List<KeyValuePair<Seat, bool>>>();
            Dictionary<char, List<KeyValuePair<Seat, bool>>> seatListOrdered = new Dictionary<char, List<KeyValuePair<Seat, bool>>>();

            for (int i = 1; i <= Checkout.Show.Room.Rows; i++)
            {
                List<KeyValuePair<Seat, bool>> oneRow = new List<KeyValuePair<Seat, bool>>();
                char c = GetCharInAlphabetForNumber(i, false);

                for (int j = 1; j <= Checkout.Show.Room.SeatsPerRow; j++)
                {
                    Seat seat = new Seat
                    {
                        RowLetter = c,
                        SeatNumber = j
                    };
                    seat.Status = GetSeatStatus(seat);
                    
                    oneRow.Add(new KeyValuePair<Seat, bool>(seat, false));
                }
                seatList.Add(c, oneRow);
            }
            seatList = seatList
                .OrderByDescending(s => s.Key)
                .ToDictionary(x => x.Key, y => y.Value);

            return seatList;
        }

        private char GetCharInAlphabetForNumber(int number, bool isCaps)
        {
            char c = (char)((isCaps ? 65 : 97) + (number - 1));
            return c;
        }

        private SeatStatus GetSeatStatus(Seat seat)
        {
            var item = OccupiedSeats.FirstOrDefault(s => s.Key.RowLetter == seat.RowLetter && s.Key.SeatNumber == seat.SeatNumber);
            if (item.Key == null)
                return SeatStatus.Free;

            return item.Key.Status;
        }
    }
}
