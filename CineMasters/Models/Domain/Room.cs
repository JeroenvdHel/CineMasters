using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Domain
{
    public class Room
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
    }
}
