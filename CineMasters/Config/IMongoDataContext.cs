using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Areas.Shows.Models;
using MongoDB.Driver;

namespace CineMasters.Config
{
    public interface IMongoDataContext
    {
        IMongoCollection<Movie> Movies { get; }
        IMongoCollection<Show> Shows { get; }
        IMongoCollection<Room> Rooms { get; }
        IMongoCollection<Ticket> Tickets { get; }
    }
}
