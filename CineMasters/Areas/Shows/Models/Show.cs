using CineMasters.Models.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Areas.Shows.Models
{
    public class Show
    {
        [BsonId(IdGenerator = typeof(CustomIdGenerator))]
        public string InternalId { get; set; }

        public long Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime DateTime { get; set; }

        public long MovieId { get; set; }
        public Movie Movie { get; set; }

        public long RoomId { get; set; }
        public Room Room { get; set; }

        public List<Seat> OccupiedSeats { get; set; }

        [DisplayName("3D")]
        public bool ThreeDimensional { get; set; }
    }

}
