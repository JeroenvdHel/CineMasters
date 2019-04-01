using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Shows.Models;
using CineMasters.Config;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;


namespace CineMasters.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly IMongoDataContext _context;

        public ShowRepository(IMongoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Show>> GetAllShows()
        {
            var query = from show in _context.Shows.AsQueryable()
                        join movie in _context.Movies.AsQueryable()
                            on show.MovieId equals movie.Id
                        join room in _context.Rooms.AsQueryable()
                            on show.RoomId equals room.Id
                        select new Show
                        {
                            InternalId = show.InternalId,
                            Id = show.Id,
                            DateTime = show.DateTime,
                            MovieId = show.MovieId,
                            Movie = movie,
                            RoomId = show.RoomId,
                            Room = room,
                            OccupiedSeats = show.OccupiedSeats,
                            ThreeDimensional = show.ThreeDimensional
                        };

            return await Task.FromResult(query.ToList());
        }

        public async Task<IEnumerable<Show>> GetShowsForMovie(long id)
        {
            long movieId = id;
            FilterDefinition<Show> filter = Builders<Show>.Filter.Eq(s => s.MovieId, movieId);
            return await _context.Shows.Find(filter).ToListAsync();
        }

        public async Task<Show> GetShow(long id)
        {
            var query = from show in _context.Shows.AsQueryable()
                        join movie in _context.Movies.AsQueryable()
                            on show.MovieId equals movie.Id
                        join room in _context.Rooms.AsQueryable()
                            on show.RoomId equals room.Id
                        where show.Id == id
                        select new Show
                        {
                            InternalId = show.InternalId,
                            Id = show.Id,
                            DateTime = show.DateTime,
                            MovieId = show.MovieId,
                            Movie = movie,
                            RoomId = show.RoomId,
                            Room = room,
                            OccupiedSeats = show.OccupiedSeats,
                            ThreeDimensional = show.ThreeDimensional
                        };

            return await Task.FromResult(query.First());
        }

        public async Task CreateShow(Show show)
        {
            await _context.Shows.InsertOneAsync(show);
        }

        public async Task<bool> UpdateShow(Show show)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Shows
                        .ReplaceOneAsync(
                            filter: g => g.Id == show.Id,
                            replacement: show
                        );
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteShow(long id)
        {
            FilterDefinition<Show> filter =
                Builders<Show>.Filter.Eq(s => s.Id, id);
            DeleteResult deleteResult =
                await _context
                .Shows
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
            if (_context.Shows.CountDocumentsAsync(new BsonDocument()).Result <= (long)0)
            {
                return await Task.FromResult(1);
            }
            var list = _context.Shows
                .AsQueryable<Show>()
                .OrderByDescending(s => s.Id);

            return await Task.FromResult(list.FirstOrDefault().Id + 1);

        }
    }
}
