using Cinemasters.Models.Helpers;
using CineMasters.Areas.Shows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Areas.Shows.Models.ViewModels
{
    public class ShowListViewModel
    {
        public IEnumerable<Show> Shows { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
