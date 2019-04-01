using CineMasters.Areas.Shows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Repositories
{
    public interface IShowRepository
    {
        // api/[GET]
        Task<IEnumerable<Show>> GetAllShows();
        
        // api/1/[GET]
        Task<Show> GetShow(long id);

        Task<IEnumerable<Show>> GetShowsForMovie(long id);
        
        // api/[POST]
        Task CreateShow(Show show);
        
        // api/[PUT]
        Task<bool> UpdateShow(Show show);
        
        // api/1/[DELETE]
        Task<bool> DeleteShow(long id);

        Task<long> GetNextId();

    }
}
