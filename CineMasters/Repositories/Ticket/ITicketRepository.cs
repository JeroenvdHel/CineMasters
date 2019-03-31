using CineMasters.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Repositories
{
    public interface ITicketRepository
    {

        Task<IEnumerable<Ticket>> GetAllTickets();
        

        Task<Ticket> GetTicket(long id);
        

        Task CreateTicket(Ticket room);
        

        Task<bool> UpdateTicket(Ticket room);
        

        Task<bool> DeleteTicket(long id);

        Task<long> GetNextId();

    }
}
