using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CineMasters.Models.Domain
{
    public class Person
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}