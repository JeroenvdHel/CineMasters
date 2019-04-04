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
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoDataContext _context;

        public MovieRepository(IMongoDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies(string filter = null)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return await _context
                    .Movies
                    .Find(_ => true)
                    .ToListAsync();
            }
            return await _context
                .Movies
                .Find(Builders<Movie>.Filter.Text(filter))
                .ToListAsync();
        }

        public async Task<Movie> GetMovie(long id)
        {
            FilterDefinition<Movie> filter =
                Builders<Movie>.Filter.Eq(m => m.Id, id);
            return await _context
                .Movies
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public bool Create(Movie movie)
        {
            Task create = _context.Movies.InsertOneAsync(movie);
            create.Wait();
            return create.IsCompletedSuccessfully;
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

        /// <summary>
        /// Method to get the highest "id" in the movie collection
        /// and to return this long "id" + 1
        /// </summary>
        /// <returns>long</returns>
        public async Task<long> GetNextId()
        {
            if(_context.Movies.CountDocumentsAsync(new BsonDocument()).Result <= (long)0)
            {
                return await Task.FromResult(1);
            }
            var list = _context.Movies
                .AsQueryable<Movie>()
                .OrderByDescending(m => m.Id);

            return await Task.FromResult(list.FirstOrDefault().Id + 1);

        }
    }
}
