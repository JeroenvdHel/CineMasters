using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Domain
{
    public class Ticket
    {
        public Show Show { get; set; }
        public bool IsChild { get; set; }
        public bool IsStudent { get; set; }
        public bool IsSenior { get; set; }
        public bool IsVip { get; set; }
        public decimal Price { get; set; }
        public decimal CouponDiscount { get; set; }

        public decimal CalculatePrice()
        {
            int currentYear = DateTime.Now.Year;
            var publicHolidays = DateSystem.GetPublicHoliday(new DateTime(currentYear), new DateTime(currentYear+2), "NL");
            decimal price = 8.50M;
            
            //Check if movie length is longer then 120 minutes. If so, up price by 50 cent
            if (Show.Movie.Length > 120)
            {
                price = 9M;
            }

            //Check if show is in 3D. If so, ad 2.50 to price
            if (Show.ThreeDimensional)
            {
                price += 2.5M;
            }

            if (IsChild)
            {
                if (!IsAfterSix(Show.DateTime) && (Show.Movie.Language == Language.Nederlands))
                {
                    price -= 1.5M;
                }
            }

            if (IsStudent)
            {
                if (!IsVrZaZo(Show.DateTime))
                {
                    price -= 1.5M;
                }
            }

            if (IsSenior)
            {
                if (!IsInHoliday(Show.DateTime))
                {
                    if (!IsVrZaZo(Show.DateTime))
                    {
                        price -= 1.5M;
                    }
                }     
            }

            if (IsVip)
            {

            }
            return price;
        }

        public void ExtractCouponFromPrice(decimal discount) {
            CouponDiscount = discount;
            Price -= discount;
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
