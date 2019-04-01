using CineMasters.Areas.Shows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Repositories
{
    public interface IMovieRepository
    {
        // api/[GET]
        Task<IEnumerable<Movie>> GetAllMovies();
        
        // api/1/[GET]
        Task<Movie> GetMovie(long id);
        
        // api/[POST]
        Task Create(Movie movie);
        
        // api/[PUT]
        Task<bool> Update(Movie movie);
        
        // api/1/[DELETE]
        Task<bool> Delete(long id);

        Task<long> GetNextId();

    }
}
