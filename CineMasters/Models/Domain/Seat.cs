namespace CineMasters.Models.Domain
{
    public class Seat
    {
        public char RowLetter { get; set; }
        public int SeatNumber { get; set; }
        public SeatStatus Status { get; set; }
    }

    public enum SeatStatus
    {
        Free,
        Reserved,
        Occupied
    }
}