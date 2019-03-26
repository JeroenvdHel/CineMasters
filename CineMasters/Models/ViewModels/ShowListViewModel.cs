using Cinemasters.Models.Helpers;
using CineMasters.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.ViewModels
{
    public class ShowListViewModel
    {
        public IEnumerable<Show> Shows { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
