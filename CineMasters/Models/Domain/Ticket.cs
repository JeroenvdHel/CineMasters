using CineMasters.Models.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Domain
{
    public class Ticket
    {
        [BsonId(IdGenerator = typeof(CustomIdGenerator))]
        public string InternalId { get; set; }
        public long Id { get; set; }
        public Show Show { get; set; }
        public string Seat { get; set; }
        public bool IsChild { get; set; } = false;
        public bool IsStudent { get; set; } = false;
        public bool IsSenior { get; set; } = false;
        public bool IsVip { get; set; } = false;
        public decimal TicketPrice { get; set; }
        public Int64 TicketCode { get; set; }
        public string PaymentId { get; set; }


        public Ticket(Show show)
        {
            Show = show;
        }


        public decimal CalculatePrice(decimal showPrice)
        {
            int currentYear = DateTime.Now.Year;
            var publicHolidays = DateSystem.GetPublicHoliday(new DateTime(currentYear), new DateTime(currentYear+2), "NL");
                        
            //Check if movie length is longer then 120 minutes. If so, up price by 50 cent
            if (Show.Movie.Length > 120)
            {
                showPrice += 0.5M;
            }

            //Check if show is in 3D. If so, ad 2.50 to price
            if (Show.ThreeDimensional)
            {
                showPrice += 2.5M;
            }

            //Check if ticket is for child. If so, if show is before 18:00 and language is Dutch, subtract 1.5 from price.
            if (IsChild)
            {
                if (!IsAfterSix(Show.DateTime) && (Show.Movie.Language == Language.Nederlands))
                {
                    showPrice -= 1.5M;
                }
            }

            if (IsStudent)
            {
                if (!IsVrZaZo(Show.DateTime))
                {
                    showPrice -= 1.5M;
                }
            }

            if (IsSenior)
            {
                if (!IsInHoliday(Show.DateTime))
                {
                    if (!IsVrZaZo(Show.DateTime))
                    {
                        showPrice -= 1.5M;
                    }
                }     
            }

            if (IsVip)
            {
                showPrice += 4.0M;
            }
            return showPrice;
        }

        private bool IsInHoliday(DateTime showDate) {
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
