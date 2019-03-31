using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Domain
{
    public class Checkout
    {
        public long ShowId { get; set; }
        public Show Show { get; set; }
        public List<Seat> SelectedSeats { get; set; }
        public List<Ticket> Tickets { get; set; }

        public int StandardRate { get; set; }
        public int ChildRate { get; set; }
        public int StudentRate { get; set; }
        public int SeniorRate { get; set; }
        public int VipRate { get; set; }
        public decimal CouponDiscount { get; set; } = 0M;
        public int CheckoutCode { get; set; }
        public string PaymentId { get; set; }


        public int GetTotalTickets()
        {
            return (StandardRate + ChildRate + StudentRate + SeniorRate + VipRate);
        }

        public decimal GetTotalPrice()
        {
            decimal result = 0M;
            foreach (var ticket in Tickets)
            {
                result += ticket.TicketPrice;
            }
            return result - CouponDiscount;
        }

        public void AddTicketsToList()
        {
            Tickets = new List<Ticket>();

            AddStandardTicket(Tickets, StandardRate);
            AddChildTicket(Tickets, ChildRate);
            AddStudentTicket(Tickets, StudentRate);
            AddSeniorTicket(Tickets, SeniorRate);
            AddVIPTicket(Tickets, VipRate);

        }


        private void AddStandardTicket(List<Ticket> ticketList, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Ticket ticket = new Ticket(Show);
                ticket.TicketPrice = ticket.CalculatePrice(8.5M);
                ticketList.Add(ticket);
            }
        }
        private void AddChildTicket(List<Ticket> ticketList, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Ticket ticket = new Ticket(Show)
                {
                    IsChild = true
                };
                ticket.TicketPrice = ticket.CalculatePrice(8.5M);
                ticketList.Add(ticket);
            }
        }
        private void AddStudentTicket(List<Ticket> ticketList, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Ticket ticket = new Ticket(Show)
                {   
                    IsStudent = true
                };
                ticket.TicketPrice = ticket.CalculatePrice(8.5M);
                ticketList.Add(ticket);
            }
        }
        private void AddSeniorTicket(List<Ticket> ticketList, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Ticket ticket = new Ticket(Show)
                {
                    IsSenior = true
                };
                ticket.TicketPrice = ticket.CalculatePrice(8.5M);
                ticketList.Add(ticket);
            }
        }
        private void AddVIPTicket(List<Ticket> ticketList, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Ticket ticket = new Ticket(Show)
                {
                    IsVip = true
                };
                ticket.TicketPrice = ticket.CalculatePrice(8.5M);
                ticketList.Add(ticket);
            }
        }



        private bool IsInHoliday(DateTime showDate)
        {
            if (DateSystem.IsPublicHoliday(showDate, CountryCode.NL))
                return true;

            return false;
        }

        private bool IsVrZaZo(DateTime showDate)
        {
            DayOfWeek day = showDate.DayOfWeek;

            if ((day == DayOfWeek.Friday) || (day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
                return true;

            return false;
        }

        private bool IsAfterSix(DateTime showDate)
        {
            TimeSpan timeSpan = new TimeSpan(18, 0, 0);
            TimeSpan showTime = showDate.TimeOfDay;
            if (showTime < timeSpan)
                return true;

            return false;
        }
    }
}
