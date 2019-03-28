﻿using Mollie.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Mollie.Models
{
    public class OverviewModel<T> where T : IResponseObject
    {
        public List<T> Items { get; set; }
        public OverviewNavigationLinksModel Navigation { get; set; }
    }
}
