using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CineMasters.Models.Domain
{
    public class Show
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

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
