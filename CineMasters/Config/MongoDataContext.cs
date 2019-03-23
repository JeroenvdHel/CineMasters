using CineMasters.Models.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Config
{
    public class MongoDataContext : IMongoDataContext
    {
        private readonly IMongoDatabase _mongoDb;

        public MongoDataContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _mongoDb = client.GetDatabase(config.Database);
        }

        public IMongoCollection<Movie> Movies =>
            _mongoDb.GetCollection<Movie>("movies");

        public IMongoCollection<Show> Shows =>
            _mongoDb.GetCollection<Show>("shows");
    }
}
