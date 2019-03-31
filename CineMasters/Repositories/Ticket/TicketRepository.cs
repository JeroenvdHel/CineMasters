using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Config;
using CineMasters.Models.Domain;
using MongoDB.Bson;
using MongoDB.Driver;


namespace CineMasters.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoDataContext _context;

        public TicketRepository(IMongoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllTickets()
        {
            return await _context
                .Tickets
                .Find(_ => true)
                .ToListAsync();

        }

        public async Task<Ticket> GetTicket(long id)
        {
            FilterDefinition<Ticket> filter =
                Builders<Ticket>.Filter.Eq(r => r.Id, id);
            return await _context
                .Tickets
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTicket(Ticket room)
        {
            await _context.Tickets.InsertOneAsync(room);
        }

        public async Task<bool> UpdateTicket(Ticket room)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Tickets
                        .ReplaceOneAsync(
                            filter: r => r.Id == room.Id,
                            replacement: room
                        );
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTicket(long id)
        {
            FilterDefinition<Ticket> filter =
                Builders<Ticket>.Filter.Eq(r => r.Id, id);
            DeleteResult deleteResult =
                await _context
                .Tickets
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        /// <summary>
        /// Method to get the highest "id" in the movie collection
        /// and to return this long "id" + 1
        /// </summary>
        /// <returns>long</returns>
        public async Task<long> GetNextId()
        {
            if (_context.Tickets.CountDocumentsAsync(new BsonDocument()).Result <= (long)0)
            {
                return await Task.FromResult(1);
            }
            var list = _context.Tickets
                .AsQueryable<Ticket>()
                .OrderByDescending(s => s.Id);

            return await Task.FromResult(list.FirstOrDefault().Id + 1);

        }
    }
}
