using CineMasters.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Areas.Shows.Controllers
{
    public class TicketController
    {
        private readonly ITicketRepository _ticketRepo;

        public TicketController(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }
    }
}
