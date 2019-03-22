using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineMasters.Models.Domain;
using MongoDB.Driver;

namespace CineMasters.Config
{
    public interface IMongoDataContext
    {
        IMongoCollection<Movie> Movies { get; }
    }
}
