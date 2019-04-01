using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Areas.Shows.Models.ViewModels
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public List<ShowDay> ShowDays { get; set; }

        // Inner class to manage showtimes per day
        public class ShowDay
        {
            public DateTime Date { get; set; }
            public List<KeyValuePair<DateTime, long>> ShowTimes { get; set; }

            public ShowDay(DateTime date)
            {
                Date = date;
                ShowTimes = new List<KeyValuePair<DateTime, long>>();
            }
        }
    }

    
}
