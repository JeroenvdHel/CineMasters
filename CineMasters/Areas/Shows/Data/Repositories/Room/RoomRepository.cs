using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Shows.Models;
using CineMasters.Config;
using MongoDB.Bson;
using MongoDB.Driver;


namespace CineMasters.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMongoDataContext _context;

        public RoomRepository(IMongoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context
                .Rooms
                .Find(_ => true)
                .ToListAsync();

        }

        public async Task<Room> GetRoom(long id)
        {
            FilterDefinition<Room> filter =
                Builders<Room>.Filter.Eq(r => r.Id, id);
            return await _context
                .Rooms
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task CreateRoom(Room room)
        {
            await _context.Rooms.InsertOneAsync(room);
        }

        public async Task<bool> UpdateRoom(Room room)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Rooms
                        .ReplaceOneAsync(
                            filter: r => r.Id == room.Id,
                            replacement: room
                        );
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteRoom(long id)
        {
            FilterDefinition<Room> filter =
                Builders<Room>.Filter.Eq(r => r.Id, id);
            DeleteResult deleteResult =
                await _context
                .Rooms
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
            if (_context.Rooms.CountDocumentsAsync(new BsonDocument()).Result <= (long)0)
            {
                return await Task.FromResult(1);
            }
            var list = _context.Rooms
                .AsQueryable<Room>()
                .OrderByDescending(s => s.Id);

            return await Task.FromResult(list.FirstOrDefault().Id + 1);

        }
    }
}
