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
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoDataContext _context;

        public MovieRepository(IMongoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await _context
                .Movies
                .Find(_ => true)
                .ToListAsync();
        }

        public Task<Movie> GetMovie(long id)
        {
            FilterDefinition<Movie> filter =
                Builders<Movie>.Filter.Eq(m => m.Id, id);
            return _context
                .Movies
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task Create(Movie movie)
        {
            await _context.Movies.InsertOneAsync(movie);
        }

        public async Task<bool> Update(Movie movie)
        {
            ReplaceOneResult updateResult =
                await _context
                        .Movies
                        .ReplaceOneAsync(
                            filter: g => g.Id == movie.Id,
                            replacement: movie
                        );
            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            FilterDefinition<Movie> filter =
                Builders<Movie>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult =
                await _context
                .Movies
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<long> GetNextId()
        {
            return await _context
                .Movies
                .CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}
