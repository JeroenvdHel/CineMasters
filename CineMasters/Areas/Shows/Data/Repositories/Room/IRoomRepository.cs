using CineMasters.Areas.Shows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Repositories
{
    public interface IRoomRepository
    {

        Task<IEnumerable<Room>> GetAllRooms();
        

        Task<Room> GetRoom(long id);
        

        Task CreateRoom(Room room);
        

        Task<bool> UpdateRoom(Room room);
        

        Task<bool> DeleteRoom(long id);

        Task<long> GetNextId();

    }
}
